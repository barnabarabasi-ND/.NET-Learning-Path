using MiniDeepThought.Configurations;
using MiniDeepThought.Domain;
using MiniDeepThought.Interfaces;
using MiniDeepThought.Models.UI;
using MiniDeepThought.Util;


namespace MiniDeepThought.Services
{
    //application layer
    public class AppServices(JobService jobService, IJobRunner jobRunner, IJobStore jobStore)
    {
        //with dependency injection, to inject objects instances to AppServices class
        private readonly JobService _jobService = jobService;
        private readonly IJobRunner _jobRunner = jobRunner;
        private readonly IJobStore _jobStore = jobStore;

        //keep here the CancellationTokenSource for each job, so that we can cancel the job later if needed
        private readonly Dictionary<Guid, CancellationTokenSource> jobTokens = new();


        public void DisplayMenuOptions(List<MenuOption> menuOptions)
        {
            foreach (var option in menuOptions)
            {
                Console.WriteLine($"({option.Id}) {option.Title}");
            }
        }

        public int? ReadOptionMainMenu(List<MenuOption> menuOptions)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Enter option.");
                return null;
            }

            int option;
            if (!int.TryParse(input, out option) || !menuOptions.Any(x => x.Id == option))
            {
                Console.WriteLine("Enter valid option.");
                return null;
            }

            return option;
        }

        public async Task SubmitQuestion(List<MenuOption> menuAlgorithms) 
        { 
            Console.WriteLine("Enter your Ultimate Question (1-200 chars):");
            string? questionText;
            while (true)
            {
                questionText = ConsoleHelpers.GetValidInputString(Console.ReadLine());
                
                if (questionText == null)
                {
                    continue;
                }

                if (questionText.Length > 200) {
                    Console.WriteLine("Enter maximum 200 characters.");
                    continue;
                }

                break;
            }


            //menu options algorithms
            Console.WriteLine("Choose algorithm (Trivial / SlowCount / RandomGuess):");
            DisplayMenuOptions(menuAlgorithms);

            int? inputAlgorithm;
            while (true)
            {
                inputAlgorithm = ConsoleHelpers.GetValidInputInt(Console.ReadLine());

                if (inputAlgorithm == null)
                {
                    continue;
                }
                if (!menuAlgorithms.Any(x => x.Id == inputAlgorithm))
                {
                    Console.WriteLine("Invalid algorithm option.");
                    continue;
                }
                break;
            }
            //get algorithm key by Id
            string algorithmKey = menuAlgorithms.First(x => x.Id == inputAlgorithm).Title;

            //create new job
            var newJob = _jobService.CreateJob(questionText, algorithmKey);

            Console.WriteLine($"Added new job: " +
                    $"JobId: {newJob.JobId} " +
                    $"| Status: {newJob.Status} " +
                    $"| Algorithm: {newJob.AlgorithmKey} " +
                    $"| Question: {newJob.QuestionText} " +
                    $"| Created Utc: {newJob.CreatedDate} " +
                    $"| Progress: {newJob.Progress}%"
                );

            //add new job to the file
            _jobStore.UpdateJobInFile(newJob);


            //create a cancellation token source for the new job and store it in the dictionary; it is needed for when we cancel the job
            var cancelTokenSource = new CancellationTokenSource();
            jobTokens[newJob.JobId] = cancelTokenSource;


            //------------------------------------------
            Console.WriteLine($"Executing algorithm \"{newJob.AlgorithmKey}\" for question \"{newJob.QuestionText}\" ...");
            Console.WriteLine($"Press C to cancel job execution...");

            //initialize progress and update the job in the file with the progress from callback
            IProgress<int> progress = new Progress<int>(value =>
            {
                newJob.Progress = value;

                Console.WriteLine($"Progress: {value}%");

                _jobStore.UpdateJobInFile(newJob);
            });


            //run the new job async
            //await _jobRunner.RunJobAsync(newJob, progress, cancelTokenSource.Token);
            //start job (NU await)
            var jobTask = _jobRunner.RunJobAsync(newJob, progress, cancelTokenSource.Token);

            while (jobTask.IsCompleted == false)
            {
                if (Console.KeyAvailable)
                {
                    var pressKey = Console.ReadKey(true);
                    if (pressKey.Key == ConsoleKey.C)
                    {
                        cancelTokenSource.Cancel();
                        //Console.WriteLine("Job execution cancelled.");
                        //break;
                    }
                }
                await Task.Delay(100);
            }

            //await the jobTask and dispose CancellationTokenSource
            try
            {
                await jobTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Job execution cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //good practice to remove the cancellation token source from the dictionary and dispose it after the job is completed or cancelled
                jobTokens.Remove(newJob.JobId);
                cancelTokenSource.Dispose();
            }
        }

        public Task ListJobs()
        {
            //get jobs list from file
            var jobs = _jobStore.GetJobsFromFile();
            if (jobs == null || !jobs.Any())
            {
                Console.WriteLine("Jobs file is empty.");
                return Task.CompletedTask;
            }

            //iterate jobs and display them
            foreach (var job in jobs)
            {
                Console.WriteLine(
                    $"JobId: {job.JobId} " +
                    $"| Status: {job.Status} " +
                    $"| Algorithm: {job.AlgorithmKey} " +
                    $"| Created Utc: {job.CreatedDate} " +
                    $"| Progress: {job.Progress}% " +
                    $"|\n Start time Utc: {job.StartDate} " +
                    $"| Finished time Utc: {job.FinishDate} " +
                    $"|\n Question: {job.QuestionText} " +
                    $"| Result: {job.Result}"
                );
            }

            return Task.CompletedTask; //if no async
        }

        public Task ViewResultByJobId()
        {
            Console.WriteLine("Enter Job Id:");
            string? input;
            Guid jobId;
            while (true)
            {
                input = ConsoleHelpers.GetValidInputString(Console.ReadLine());

                if (input == null)
                {
                    continue;
                }
                if (!Guid.TryParse(input, out jobId))
                {
                    Console.WriteLine("Invalid Job Id format.");
                    continue;
                }
                break;
            }

            var jobs = _jobStore.GetJobsFromFile();
            if (jobs == null || !jobs.Any())
            {
                Console.WriteLine("No jobs found.");
                return Task.CompletedTask;
            }
            var job = jobs.FirstOrDefault(x => x.JobId == jobId);
            if (job == null)
            {
                Console.WriteLine($"Job with Id {jobId} not found.");
                return Task.CompletedTask;
            }

            if (job.Status == JobStatus.Completed)
            {
                Console.WriteLine(
                    $"JobId: {job.JobId} " +
                    $"| Status: {job.Status} " +
                    $"| Algorithm: {job.AlgorithmKey} " +
                    $"| Created Utc: {job.CreatedDate} " +
                    $"| DurationMS: {(job.FinishDate.HasValue && job.StartDate.HasValue ? (job.FinishDate.Value - job.StartDate.Value).TotalMilliseconds : 0)} "
                );
            }
            else
            {
                Console.WriteLine("Job is not completeted yet. Enter Id for a completed job");
            }
            return Task.CompletedTask;
        }

        //not impemented because we are using a cancellation token for each job, so we can cancel the job from the SubmitQuestion method
        //public Task CancelRunningJob()
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Test methiod to run multiple jobs concurrently and update their progress in the file.
        /// </summary>
        /// <returns></returns>
        public async Task TestMultipleJobs(int noJobs)
        {
            Console.WriteLine("Press C to cancel running job...");

            //collect all tasks in a list to wait for them to finish
            var tasks = new List<Task>();

            //create a cancellation token source for all jobs
            var cancelTokenSource = new CancellationTokenSource();

            for (int i = 1; i <= noJobs; i++)
            {
                //new job
                var newJob = new Job
                {
                    JobId = Guid.NewGuid(),
                    QuestionText = $"Test question {i}",
                    AlgorithmKey = AlgorithmKey.SlowCount,
                    Status = JobStatus.Pending
                };

                _jobStore.UpdateJobInFile(newJob);

                string jobTitle = $"Job {i}";

                IProgress<int> progress = new Progress<int>(value =>
                {
                    newJob.Progress = value;
                    Console.WriteLine($"Job {jobTitle} progress: {value}%");
                    _jobStore.UpdateJobInFile(newJob);
                });

                var jobTask = _jobRunner.RunJobAsync(newJob, progress, cancelTokenSource.Token);
                tasks.Add(jobTask);

                await Task.Delay(1000); //1 sec between jobs
            }

            
            //collect all tasks in a single task to wait for them to finish
            var allTasks = Task.WhenAll(tasks);

            //listen for the input key to cancel all jobs execution
            while (allTasks.IsCompleted == false)
            {
                if (Console.KeyAvailable)
                {
                    var pressKey = Console.ReadKey(true);
                    if (pressKey.Key == ConsoleKey.C)
                    {
                        cancelTokenSource.Cancel();
                        Console.WriteLine($"All jobs have been cancelled.");
                    }
                }

                //necessary for cooperative loop instead break; stops the loop for a while and release thread to other tasks, otherwise it will be a busy loop and will consume CPU
                await Task.Delay(100);
            }

            //wait for all tasks to finish
            await allTasks;

            //good practice for large number of jobs
            cancelTokenSource.Dispose();
        }

        public Task ExitApp()
        {
            Environment.Exit(0);
            return Task.CompletedTask; //if no async
        }

    }
}

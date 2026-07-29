
using MiniDeepThought.Configurations;
using MiniDeepThought.Domain;
using MiniDeepThought.Interfaces;
using MiniDeepThought.Strategies;

namespace MiniDeepThought.Services
{
    //orchestration 
    public class JobRunner : IJobRunner
    {
        //JobRunner that runs one job on the main thread using async/await, accepts CancellationToken, updates progress, and saves.

        //with dependency injection
        //class recives the interface
        private readonly IJobStore jobStore;

        //constructor to inject the dependencies
        public JobRunner(IJobStore jobStore)
        {
            this.jobStore = jobStore;
        }

        /// <summary>
        /// Run the job asynchronously using the corresponding strategy from AlgorithmKey.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task RunJobAsync(Job job, IProgress<int> progress, CancellationToken cancellationToken)
        {
            //strategy pattern to select the coresponding algorithm based on the AlgorithmKey
            IAnswerStrategy strategy = job.AlgorithmKey switch
            {
                "Trivial" => new TrivialStrategy(),
                "SlowCount" => new SlowCountStrategy(),
                "RandomGuess" => new RandomGuessStrategy(),
                _ => throw new ArgumentException($"Invalid algorithm key: {job.AlgorithmKey}")
            };

            try
            {

                //set job initial status and save to file
                job.Status = JobStatus.Running;
                //job.Progress = 0;
                job.StartDate = DateTime.UtcNow;
                jobStore.UpdateJobInFile(job);

                //necessary in case of cancelling the job, because it might remain Running
                cancellationToken.ThrowIfCancellationRequested();

                //generate the answer by strategy, with 
                job.Result = await strategy.GenerateAnswerAsync(progress, job.QuestionText, cancellationToken);

                //set job completed status 
                //progress.Report(100);
                job.Status = JobStatus.Completed;
                job.FinishDate = DateTime.UtcNow;
            }
            catch (OperationCanceledException) //catch the cancellation exception and set the job status to cancelled
            {
                job.Status = JobStatus.Cancelled;
                job.FinishDate = DateTime.UtcNow;
            }
            catch(Exception ex) //catch other exception and set the job status to failed
            {
                job.Status = JobStatus.Failed;
                job.Result = ex.Message;
            }
            finally
            {
                //save the job status to file
                jobStore.UpdateJobInFile(job);
            }

            Console.WriteLine($"Job \"{job.QuestionText}\" result: {job.Result}");
        }

        //public IAnswerStrategy GetStrategy(string algorithmKey)
        //{
        //    IAnswerStrategy strategy = algorithmKey switch
        //    {
        //        "Trivial" => new TrivialStrategy(),
        //        "SlowCount" => new SlowCountStrategy(),
        //        "RandomGuess" => new SlowCountStrategy(),
        //        _ => throw new ArgumentException($"Invalid algorithm key: {algorithmKey}")
        //    };

        //    return strategy;
        //}
    }
}


using MiniDeepThought.Configurations;
using MiniDeepThought.Domain;
using MiniDeepThought.Interfaces;
using System.Text.Json;

namespace MiniDeepThought.Services
{
    /// <summary>
    /// Requirements: Jobs persist to and load from JSON correctly; for JSON load/save (all in one file).
    /// </summary>
    public class JobStore : IJobStore
    {
        //file full path must be sent in constructor, because it is used in unit tests with different file path 
        private readonly string _filePath;

        public JobStore(string filePath)
        {
            _filePath = filePath;
        }

        // Synchronize file access across threads/instances to avoid file-in-use errors.
        private static readonly object _fileLock = new object();
        //lock is used when executing one thread at a time, to avoid race conditions
        //must be a private field and never should be exposed to any method outside the class
        //minimize the code that is executed while holding a lock
        //lock statement is not compatible with await

        
        public void CreateFileStorageJobs()
        {
            if (!Directory.Exists(AppConfig.FolderFiles))
            {
                Directory.CreateDirectory(AppConfig.FolderFiles);
            }

            //1. with lock if no async/await
            // Ensure file exists. Use lock to avoid races creating the file concurrently.
            lock (_fileLock)
            {
                if (!File.Exists(_filePath))
                {
                    using (File.Create(_filePath)) { }
                }
            }
        }

        
        public List<Job> GetJobsFromFile()
        {
            CreateFileStorageJobs();

            var jobs = new List<Job>();

            string jsonFromFile;

            // Read file under a lock to avoid concurrent writes from other threads in this process.
            lock (_fileLock)
            {
                jsonFromFile = File.ReadAllText(_filePath);
            }

            if (string.IsNullOrWhiteSpace(jsonFromFile))
            {
                return jobs;
            }

            jobs = JsonSerializer.Deserialize<List<Job>>(jsonFromFile) ?? new List<Job>();

            return jobs;
        }

        
        public void SaveJobsListToFile(List<Job> listJobs)
        {
            try
            {
                CreateFileStorageJobs();

                string jsonJobs = JsonSerializer.Serialize(listJobs, new JsonSerializerOptions { WriteIndented = true });

                // Write file under lock to prevent simultaneous writers from different threads/instances.

                //lock (_fileLock)
                //{
                //    File.WriteAllText(AppConfig.FilePathJobs, jsonJobs); //this will
                //}

                //use FileShare.None for exclusive access; this prevents another process reading/writing the file while it is open
                using var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                using var writer = new StreamWriter(fs);

                writer.Write(jsonJobs);
                writer.Flush(); //moves the data from the StreamWriter buffer to the FileStream buffer
                fs.Flush(); //force writing buffer to disk

            }
            catch (IOException exIO)
            {
                throw new ApplicationException($"The file is in use by another process: {exIO.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error saving job to file: {ex.Message}", ex);
            }

        }

        
        public void UpdateJobInFile(Job job)
        {
            lock (_fileLock) //used for avoiding concurrency issues when writing to the file, but it will block other threads until the lock is released
            {
                var listJobs = GetJobsFromFile();

                //get the job in the list, by JobId
                var jobInFile = listJobs.FirstOrDefault(x => x.JobId == job.JobId);

                //if the job does not exists in the file, by GuiId, add it to the list of jobs
                if (jobInFile == null)
                {
                    //throw new ApplicationException($"Not found JobId={updateJob.JobId}.");
                    listJobs.Add(job);
                }
                else //otherwise update the existing job in the list of jobs
                {
                    //update the existing job
                    jobInFile.Status = job.Status;
                    jobInFile.Progress = job.Progress;
                    jobInFile.Result = job.Result;
                    jobInFile.StartDate = job.StartDate;
                    jobInFile.FinishDate = job.FinishDate;
                }

                //save the updated list of jobs to the file
                SaveJobsListToFile(listJobs);
            }
        }
    }
}

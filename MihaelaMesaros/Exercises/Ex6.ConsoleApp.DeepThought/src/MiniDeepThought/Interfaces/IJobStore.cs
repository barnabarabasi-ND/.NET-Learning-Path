
using MiniDeepThought.Domain;

namespace MiniDeepThought.Interfaces
{
    //interface for JobStore is needed because of IO operations, persistent storage, unit testing
    public interface IJobStore
    {
        /// <summary>
        /// Check existence of folder and file where jobs json is stored. If not exists, create folder and file.
        /// </summary>
        void CreateFileStorageJobs();

        /// <summary>
        /// Returns list of jobs from file. If file is empty, returns an empty list of jobs.
        /// </summary>
        /// <returns>List of jobs</returns>
        List<Job> GetJobsFromFile();

        /// <summary>
        /// Saves a list of jobs as json to the file. The entire content file will be overwritten.
        /// </summary>
        /// <param name="listJobs"></param>
        void SaveJobsListToFile(List<Job> jobs);

        /// <summary>
        /// Update Status, Progress, Result, StartDate, FinishDate for the specified job and save the updated list of jobs to the file. 
        /// If the job is not found by Guid, it will be added to the list of jobs and saved to the file.
        /// </summary>
        /// <param name="job">Job to be updated.</param>
        void UpdateJobInFile(Job job);

    }
}

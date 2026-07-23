using MiniDeepThought.Configurations;
using MiniDeepThought.Domain;
using MiniDeepThought.Services;
using System.Text.Json;

namespace MiniDeepThought.Tests
{
    public class JobStoreTests : IDisposable
    {
        private readonly string _folderFiles = AppConfig.FolderFiles;
        private readonly string _filePath;

        public JobStoreTests()
        {
            //will produce new file for each test
            //it gets different file name because xUnit creates new instance for test class, for each [Fact]
            _filePath = Path.Combine(_folderFiles, $"deepthought-jobs-test-{Guid.NewGuid()}.json");
        }

        // Deletes the test file after finished the test.
        // Useful for cleaning up after tests, but can be commented out if you want to keep the file for manual verification.
        public void Dispose()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [Fact]
        public void CreateFileStorageJobs_Should_CreateFolderAndFileWhenDontExist()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            // Act
            jobStore.CreateFileStorageJobs();

            // Assert
            Assert.True(Directory.Exists(_folderFiles));
            Assert.True(File.Exists(_filePath));
        }

        [Fact]
        public void CreateFileStorageJobs_Should_NotThrowWhenFolderAndFileExist()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            // Act
            var exception = Record.Exception(() => jobStore.CreateFileStorageJobs());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void GetJobsFromFile_ShouldReturnEmptyList_WhenFileIsEmpty()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            jobStore.CreateFileStorageJobs();
            File.WriteAllText(_filePath, string.Empty);

            // Act
            var jobs = jobStore.GetJobsFromFile();

            // Assert
            Assert.NotNull(jobs);
            Assert.Empty(jobs);
        }

        [Fact]
        public void GetJobsFromFile_ShouldReturnJobs_WhenFileContainsJobs()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            jobStore.CreateFileStorageJobs();
            var jobsAdd = new List<Job>
            {
                new Job { JobId = Guid.NewGuid(), QuestionText = "Question 1", Status = JobStatus.Pending, Progress = 0 },
                new Job { JobId = Guid.NewGuid(), QuestionText = "Question 2", Status = JobStatus.Pending, Progress = 0 }
            };

            jobStore.SaveJobsListToFile(jobsAdd);

            // Act
            var jobsInFile = jobStore.GetJobsFromFile();

            // Assert
            Assert.NotNull(jobsInFile);
            Assert.Equal(2, jobsInFile.Count);

            //verify each job properies values
            Assert.Equal(jobsAdd[0].JobId, jobsInFile[0].JobId);
            Assert.Equal(jobsAdd[0].QuestionText, jobsInFile[0].QuestionText);
            Assert.Equal(jobsAdd[0].Status, jobsInFile[0].Status);
            Assert.Equal(jobsAdd[0].Progress, jobsInFile[0].Progress);

            Assert.Equal(jobsAdd[1].JobId, jobsInFile[1].JobId);
            Assert.Equal(jobsAdd[1].QuestionText, jobsInFile[1].QuestionText);
            Assert.Equal(jobsAdd[1].Status, jobsInFile[1].Status);
            Assert.Equal(jobsAdd[1].Progress, jobsInFile[1].Progress);
        }

        [Fact]
        public void SaveJobsListToFile_ShouldSaveJobsToFile()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);
            jobStore.CreateFileStorageJobs();

            var jobsAdd = new List<Job>
            {
                new Job { JobId = Guid.NewGuid(), QuestionText = "Question 1", AlgorithmKey = "RandomGuess", Status = JobStatus.Pending, Progress = 0, CreatedDate = DateTime.UtcNow },
            };

            // Act
            jobStore.SaveJobsListToFile(jobsAdd);

            // Assert
            Assert.True(File.Exists(_filePath));

            var json = File.ReadAllText(_filePath);

            Assert.NotNull(json);
            Assert.NotEmpty(json);

            var savedJobs = JsonSerializer.Deserialize<List<Job>>(json);

            Assert.NotNull(savedJobs);
            Assert.Single(savedJobs);

            Assert.Equal(jobsAdd[0].JobId, savedJobs[0].JobId);
            Assert.Equal(jobsAdd[0].QuestionText, savedJobs[0].QuestionText);
            Assert.Equal(jobsAdd[0].AlgorithmKey, savedJobs[0].AlgorithmKey);
            Assert.Equal(jobsAdd[0].Status, savedJobs[0].Status);
            Assert.Equal(jobsAdd[0].Progress, savedJobs[0].Progress);
            Assert.Equal(jobsAdd[0].CreatedDate, savedJobs[0].CreatedDate);
        }

        [Fact]
        public void UpdateJobInFile_ShouldUpdateExistingJob()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            //add new job in file
            var jobToUpdate = new Job { JobId = Guid.NewGuid(), QuestionText = "Question 1", AlgorithmKey = "RandomGuess", Status = JobStatus.Pending, Progress = 0, Result = string.Empty };

            jobStore.SaveJobsListToFile(new List<Job> { jobToUpdate });

            //update job with new values
            jobToUpdate.Status = JobStatus.Completed;
            jobToUpdate.Progress = 100;
            jobToUpdate.Result = "42";
            jobToUpdate.StartDate = DateTime.UtcNow;
            jobToUpdate.FinishDate = DateTime.UtcNow.AddSeconds(5);

            // Act
            jobStore.UpdateJobInFile(jobToUpdate);

            // Assert
            var json = File.ReadAllText(_filePath);
            var jobsFromFile = JsonSerializer.Deserialize<List<Job>>(json);

            Assert.NotNull(jobsFromFile);

            var updatedJob = jobsFromFile.First(x => x.JobId == jobToUpdate.JobId);

            Assert.Equal(jobToUpdate.JobId, updatedJob.JobId);
            Assert.Equal(jobToUpdate.QuestionText, updatedJob.QuestionText);
            Assert.Equal(jobToUpdate.AlgorithmKey, updatedJob.AlgorithmKey);
            Assert.Equal(jobToUpdate.Result, updatedJob.Result);
            Assert.Equal(JobStatus.Completed, updatedJob.Status);
            Assert.Equal(100, updatedJob.Progress);
            Assert.Equal(jobToUpdate.CreatedDate, updatedJob.CreatedDate);
            Assert.Equal(jobToUpdate.StartDate, updatedJob.StartDate);
            Assert.Equal(jobToUpdate.FinishDate, updatedJob.FinishDate);
        }

    }
}

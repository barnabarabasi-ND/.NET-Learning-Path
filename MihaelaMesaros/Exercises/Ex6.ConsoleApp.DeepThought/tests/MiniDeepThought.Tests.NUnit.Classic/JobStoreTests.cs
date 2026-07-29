using MiniDeepThought.Configurations;
using MiniDeepThought.Domain;
using MiniDeepThought.Services;
using System.Text.Json;

using NUnit.Framework.Legacy; //for ClassicAssert

namespace MiniDeepThought.Tests
{
    public class JobStoreTests
    {
        private readonly string _folderFiles = AppConfig.FolderFiles;
        private string _filePath;


        [SetUp] //Runs before each test. Create new file for each test
        //[OneTimeSetUp] //Runs once. Create new file once for all tests.
        public void SetUp()
        {

            //it gets different file name because the test creates new instance for test class, for each [Fact]
            _filePath = Path.Combine(_folderFiles, $"deepthought-jobs-test-{Guid.NewGuid()}.json");
        }


        // Useful for cleaning up after tests, but can be commented out if you want to keep the file for manual verification.
        [TearDown] // Delete the test file after finished each test.
        //[OneTimeTearDown] // Delete the test file after finished all tests.
        public void DeleteFiles()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }


        [Test]
        public void CreateFileStorageJobs_Should_CreateFolderAndFileWhenDontExist()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            // Act
            jobStore.CreateFileStorageJobs();

            // Assert
            ClassicAssert.IsTrue(Directory.Exists(_folderFiles));
            ClassicAssert.True(File.Exists(_filePath));
        }

        [Test]
        public void CreateFileStorageJobs_Should_NotThrowWhenFolderAndFileExist()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);


            // Act & Assert
            ClassicAssert.DoesNotThrow(jobStore.CreateFileStorageJobs);
        }

        [Test]
        public void GetJobsFromFile_ShouldReturnEmptyList_WhenFileIsEmpty()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);

            jobStore.CreateFileStorageJobs();
            File.WriteAllText(_filePath, string.Empty);

            // Act
            var jobs = jobStore.GetJobsFromFile();

            // Assert
            ClassicAssert.IsNotNull(jobs);
            ClassicAssert.IsEmpty(jobs);
        }

        [Test]
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
            ClassicAssert.IsNotNull(jobsInFile);
            ClassicAssert.AreEqual(2, jobsInFile.Count);

            //verify each job properies values
            ClassicAssert.AreEqual(jobsAdd[0].JobId, jobsInFile[0].JobId);
            ClassicAssert.AreEqual(jobsAdd[0].QuestionText, jobsInFile[0].QuestionText);
            ClassicAssert.AreEqual(jobsAdd[0].Status, jobsInFile[0].Status);
            ClassicAssert.AreEqual(jobsAdd[0].Progress, jobsInFile[0].Progress);

            ClassicAssert.AreEqual(jobsAdd[1].JobId, jobsInFile[1].JobId);
            ClassicAssert.AreEqual(jobsAdd[1].QuestionText, jobsInFile[1].QuestionText);
            ClassicAssert.AreEqual(jobsAdd[1].Status, jobsInFile[1].Status);
            ClassicAssert.AreEqual(jobsAdd[1].Progress, jobsInFile[1].Progress);
        }

        [Test]
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
            ClassicAssert.True(File.Exists(_filePath));

            var json = File.ReadAllText(_filePath);

            ClassicAssert.NotNull(json);
            ClassicAssert.IsNotEmpty(json);

            var savedJobs = JsonSerializer.Deserialize<List<Job>>(json);

            ClassicAssert.NotNull(savedJobs);
            ClassicAssert.That(savedJobs.Count(), Is.EqualTo(1));

            ClassicAssert.AreEqual(jobsAdd[0].JobId, savedJobs[0].JobId);
            ClassicAssert.AreEqual(jobsAdd[0].QuestionText, savedJobs[0].QuestionText);
            ClassicAssert.AreEqual(jobsAdd[0].AlgorithmKey, savedJobs[0].AlgorithmKey);
            ClassicAssert.AreEqual(jobsAdd[0].Status, savedJobs[0].Status);
            ClassicAssert.AreEqual(jobsAdd[0].Progress, savedJobs[0].Progress);
            ClassicAssert.AreEqual(jobsAdd[0].CreatedDate, savedJobs[0].CreatedDate);
        }

        [Test]
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

            ClassicAssert.NotNull(jobsFromFile);

            var updatedJob = jobsFromFile.First(x => x.JobId == jobToUpdate.JobId);

            ClassicAssert.AreEqual(jobToUpdate.JobId, updatedJob.JobId);
            ClassicAssert.AreEqual(jobToUpdate.QuestionText, updatedJob.QuestionText);
            ClassicAssert.AreEqual(jobToUpdate.AlgorithmKey, updatedJob.AlgorithmKey);
            ClassicAssert.AreEqual(jobToUpdate.Result, updatedJob.Result);
            ClassicAssert.AreEqual(JobStatus.Completed, updatedJob.Status);
            ClassicAssert.AreEqual(100, updatedJob.Progress);
            ClassicAssert.AreEqual(jobToUpdate.CreatedDate, updatedJob.CreatedDate);
            ClassicAssert.AreEqual(jobToUpdate.StartDate, updatedJob.StartDate);
            ClassicAssert.AreEqual(jobToUpdate.FinishDate, updatedJob.FinishDate);
        }

    }
}

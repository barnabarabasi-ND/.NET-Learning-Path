using MiniDeepThought.Configurations;
using MiniDeepThought.Domain;
using MiniDeepThought.Services;
using System.Text.Json;

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
            Assert.That(Directory.Exists(_folderFiles), Is.True);
            Assert.That(File.Exists(_filePath), Is.True);
        }

        [Test]
        public void CreateFileStorageJobs_Should_NotThrowWhenFolderAndFileExist()
        {
            // Arrange
            var jobStore = new JobStore(_filePath);


            // Act & Assert
            Assert.That(jobStore.CreateFileStorageJobs, Throws.Nothing);
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
            Assert.That(jobs, Is.Not.Null);
            Assert.That(jobs, Is.Empty);
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
            Assert.That(jobsInFile, Is.Not.Null);
            Assert.That(jobsInFile.Count, Is.EqualTo(2));

            //verify each job properies values
            Assert.That(jobsAdd[0].JobId, Is.EqualTo(jobsInFile[0].JobId));
            Assert.That(jobsAdd[0].QuestionText, Is.EqualTo(jobsInFile[0].QuestionText));
            Assert.That(jobsAdd[0].Status, Is.EqualTo(jobsInFile[0].Status));
            Assert.That(jobsAdd[0].Progress, Is.EqualTo(jobsInFile[0].Progress));

            Assert.That(jobsAdd[1].JobId, Is.EqualTo(jobsInFile[1].JobId));
            Assert.That(jobsAdd[1].QuestionText, Is.EqualTo(jobsInFile[1].QuestionText));
            Assert.That(jobsAdd[1].Status, Is.EqualTo(jobsInFile[1].Status));
            Assert.That(jobsAdd[1].Progress, Is.EqualTo(jobsInFile[1].Progress));
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
            Assert.That(File.Exists(_filePath), Is.True);

            var json = File.ReadAllText(_filePath);

            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.Not.Empty);

            var savedJobs = JsonSerializer.Deserialize<List<Job>>(json);

            Assert.That(savedJobs, Is.Not.Null);
            Assert.That(savedJobs.Count(), Is.EqualTo(1));

            Assert.That(jobsAdd[0].JobId, Is.EqualTo(savedJobs[0].JobId));
            Assert.That(jobsAdd[0].QuestionText, Is.EqualTo(savedJobs[0].QuestionText));
            Assert.That(jobsAdd[0].AlgorithmKey, Is.EqualTo(savedJobs[0].AlgorithmKey));
            Assert.That(jobsAdd[0].Status, Is.EqualTo(savedJobs[0].Status));
            Assert.That(jobsAdd[0].Progress, Is.EqualTo(savedJobs[0].Progress));
            Assert.That(jobsAdd[0].CreatedDate, Is.EqualTo(savedJobs[0].CreatedDate));
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

            Assert.That(jobsFromFile, Is.Not.Null);

            var updatedJob = jobsFromFile.First(x => x.JobId == jobToUpdate.JobId);

            Assert.That(jobToUpdate.JobId, Is.EqualTo(updatedJob.JobId));
            Assert.That(jobToUpdate.QuestionText, Is.EqualTo(updatedJob.QuestionText));
            Assert.That(jobToUpdate.AlgorithmKey, Is.EqualTo(updatedJob.AlgorithmKey));
            Assert.That(jobToUpdate.Result, Is.EqualTo(updatedJob.Result));
            Assert.That(updatedJob.Status, Is.EqualTo(JobStatus.Completed));
            Assert.That(updatedJob.Progress, Is.EqualTo(100));
            Assert.That(jobToUpdate.CreatedDate, Is.EqualTo(updatedJob.CreatedDate));
            Assert.That(jobToUpdate.StartDate, Is.EqualTo(updatedJob.StartDate));
            Assert.That(jobToUpdate.FinishDate, Is.EqualTo(updatedJob.FinishDate));
        }

    }
}

using System.IO.Compression;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VariousDemos.Services;

namespace VariousDemos.Demos
{
    internal class DemoFileStreamAsync
    {
        static string baseFolder = @"C:\Endava\EndevLocal\Learning";
        static string appFolder = Path.Combine(baseFolder, "DemoFiles");
        static string fileNameLog = "log.txt";
        static string fileNameLogCompressed = "log.txt.gz";
        static string fileNameLogRestored = "log-restored.txt";

        //for writing in log file by multiple users, to avoid race conditions; 1=only one request can write to the log file at a time
        //we want only one instance - one shared semaphore for all requests
        private static readonly SemaphoreSlim logSemaphore = new SemaphoreSlim(1);

        internal static async Task Run(ILogger<DemoFileStreamAsync> logger)
        {
            logger.LogInformation("Starting DemoFileStreamAsync");

            string filePathAnimals = Path.Combine(appFolder, "animals.json");
            string filePathLog = Path.Combine(appFolder, fileNameLog);
            string filePathLogCompressed = Path.Combine(appFolder, fileNameLogCompressed);
            string filePathLogRestored = Path.Combine(appFolder, fileNameLogRestored);

            var animals = new List<Animal> {
                new Dog {Name = "Dog 1",Age = 5,IsTrained = true},
                new Dog {Name = "Dog 2",Age = 2,IsTrained = false},
                new Cat {Name = "Cat 1",Age = 3,IsIndoor = true},
                new Cat {Name = "Cat 2",Age = 4,IsIndoor = false}
            };

            Directory.CreateDirectory(appFolder);

            try
            {
                ////----- FileStream -----
                //Console.WriteLine("FileStream\n=====================");
                //save list to file, with FileStream only
                //await SaveAnimalsToFileAsync(fileFullPathAnimals, animals);

                //await WriteToLogAsync(Path.Combine(appFolder, fileNameLog), "Saved animals to file (FileStream).");

                //get from file to list, with FileStream only
                //List<Animal> listAnimals = await GetAnimalsFromFileAsync(fileFullPathAnimals);

                //----- MemoryStream -----
                Console.WriteLine("\nMemoryStream + FileStream\n=====================");
                //save list to file, with MemoryStream + FileStream
                await SaveAnimalsMSToFileAsync(filePathAnimals, animals);

                await WriteToLogAsync(filePathLog, "Saved animals to file (MemoryStream).");

                //get from file to list, with FileStream + MemoryStream
                var listAnimals = await GetAnimalsMSFromFileAsync(filePathAnimals);

                //----- GZipStream -----
                //compress
                Console.WriteLine("\nGZipStream\n=====================");
                await CompressLogAsync(filePathLog, filePathLogCompressed);

                var originalSize = new FileInfo(filePathLog).Length / 1024.0;
                var compressedSize = new FileInfo(filePathLogCompressed).Length / 1024.0;
                Console.WriteLine($"Original: {originalSize:N2} KB");
                Console.WriteLine($"Compressed: {compressedSize:N2} KB");
                await WriteToLogAsync(filePathLog, "Compressed log file.");

                //decompress
                await DecompressLogAsync(filePathLogCompressed, filePathLogRestored);
                await WriteToLogAsync(filePathLog, "Decompressed log file.");


                //----- threads -----
                Console.WriteLine("\nThreads\n=====================");
                Thread animalThread = new Thread(() =>
                {
                    Console.WriteLine($"Thread started: {Environment.CurrentManagedThreadId}");

                    foreach (Animal animal in animals)
                    {
                        Console.WriteLine(animal.Speak());
                        Thread.Sleep(500); //blocks the thread
                    }

                    Console.WriteLine($"Thread finished.");
                });
                animalThread.Start(); //starts the new thread
                Console.WriteLine($"[{DateTime.Now}] Main thread waiting | Thread: {Environment.CurrentManagedThreadId}");
                animalThread.Join(); //blocks current thread until animalThread finishes
                Console.WriteLine($"[{DateTime.Now}] Main thread continues | Thread: {Environment.CurrentManagedThreadId}");


                //----- tasks / async -----
                Console.WriteLine("\nTasks\n=====================");

                var tasksCPU = new List<Task<int>>();

                foreach (Animal animal in listAnimals)
                {
                    Task<int> task = Task.Run(() => CalculateSomethingCPUIntensive(animal)); //programs to execute the method on threadpool

                    tasksCPU.Add(task);
                }

                await Task.WhenAll(tasksCPU); //wait for all tasks to complete


                //----- SemaphoreSlim -----
                Console.WriteLine("\nSemaphoreSlim\n=====================");
                using SemaphoreSlim semaphore = new SemaphoreSlim(2); //limits concurrency: max 2 requests can run at the same time

                var tasksSemaphore = listAnimals.Select(async animal =>
                {
                    Console.WriteLine($"{animal.Name} -> WAITING for semaphore | Available: {semaphore.CurrentCount} | Thread: {Environment.CurrentManagedThreadId}");
                    logger.LogInformation("{AnimalName} -> WAITING for semaphore | Available: {Available} | Thread: {ThreadId}", animal.Name, semaphore.CurrentCount, Environment.CurrentManagedThreadId);

                    await semaphore.WaitAsync(); //--->max 2 operations continue after this line, others will wait for semaphore to be released; doen't block the thread

                    Console.WriteLine($"{animal.Name} -> ENTERED semaphore | Available: {semaphore.CurrentCount} | Thread: {Environment.CurrentManagedThreadId}");
                    logger.LogInformation("{AnimalName} -> ENTERED semaphore | Available: {Available} | Thread: {ThreadId}", animal.Name, semaphore.CurrentCount, Environment.CurrentManagedThreadId);


                    try
                    {
                        await ProcessAnimalAsync(animal);
                    }
                    //catch
                    finally
                    {
                        semaphore.Release(); //---> imprtant, because if not released, the other tasks will wait forever

                        Console.WriteLine($"{animal.Name} -> RELEASED semaphore | Available: {semaphore.CurrentCount} | Thread: {Environment.CurrentManagedThreadId}");
                    }
                });

                await Task.WhenAll(tasksSemaphore); //wait for all tasks to complete


                Console.WriteLine("\nMultiple users write to log:\n");
                //simulates multiple users writing concurrently to the same log file.
                //SemaphoreSlim(1) ensures that only one user writes at a time, preventing race conditions.
                var userTasks = new List<Task>();
                var numberUsers = 7;
                foreach (int userIndex in Enumerable.Range(1, numberUsers))
                {
                    Task userTask = WriteUserToLogAsync(userIndex, filePathLog);

                    userTasks.Add(userTask);
                }

                await Task.WhenAll(userTasks);

                //another solution, ILogger for many users


                //----- Parallel -----
                Console.WriteLine("\nParallel\n=====================");

                //Parallel.For(0, listAnimals.Count, i => { ... }); - permits loops to be executed in parallel; it blocks the calling thread until all iterations are complete
                //or:
                Parallel.ForEach(listAnimals, animal =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Parallel START {animal.Name} | Thread: {Environment.CurrentManagedThreadId}");

                    CalculateSomethingCPUIntensive(animal);

                    Console.WriteLine($"[{DateTime.Now}] Parallel END {animal.Name} | Thread: {Environment.CurrentManagedThreadId}");
                });

                Console.WriteLine("Parallel finished.");


                //----- CancellationToken -----
                Console.WriteLine("\nCancellationToken\n=====================");

                using CancellationTokenSource cts = new CancellationTokenSource(); //controls cancelling

                //cts.Cancel();
                //or
                cts.CancelAfter(TimeSpan.FromSeconds(3));

                try
                {
                    await WriteLogsWithCancellationAsync(filePathLog, cts.Token); //passing cancellation token where need to know that cancellation is requested
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Log writing was cancelled.");
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
                logger.LogError(ex, "File not found: {Message}", ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Invalid JSON: {ex.Message}");
                logger.LogError(ex, "Invalid JSON: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                logger.LogError(ex, "General error: {Message}", ex.Message);
            }
            finally
            {
                Console.WriteLine("\nApplication finished.");
                logger.LogInformation("Application finished.");
            }
        }



        #region FileStream
        //Save list of animals to json file: list animals -> JSON -> FileStream -> file
        //Better when just save to file, without other operations, less memory usage.
        private static async Task SaveAnimalsToFileAsync(string filePath, List<Animal> animals)
        {
            await using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            //create file if not exists or overwrite content, write to file, no sharing other processes while open here, buffer size for multiple small writes, async operations enabled WriteAsync
            //await using = dispose asynchronously = await stream.DisposeAsync(); and because used async file operations
            //works without await, will dispose anyway after SerializeAsync below

            await JsonSerializer.SerializeAsync(stream, animals, new JsonSerializerOptions { WriteIndented = true }); //uses WriteAsync
        }

        //Read list of animals from json file: file -> FileStream -> JSON -> list animals
        private static async Task<List<Animal>> GetAnimalsFromFileAsync(string filePath)
        {
            //ex new exception, stack trace will be lost
            //if (!File.Exists(filePath))
            //{
            //    throw new FileNotFoundException($"File does not exists.", filePath);
            //}

            try
            {
                await using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
                //open existing file for reading, open file for reading, allow other processes/threads to read while open here, buffer size for multiple small reads, async operations enabled ReadAsync
                //await stream.DisposeAsync();

                var animals = await JsonSerializer.DeserializeAsync<List<Animal>>(stream);

                return animals ?? new List<Animal>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error on deserialize: {ex.Message}");

                //ex rethrow - exception propagation, keeps original stack trace
                //throw;
                //throw ex; //stack trace will be lost

                //wrapping - new exception + original as InnerException
                throw new JsonException("Could not load data from JSON.", ex);
            }
        }
        #endregion


        #region MemoryStream
        //Save list of animals to json file: list animals -> JSON -> MemoryStream -> FileStream -> file
        //Better when need to do other operations with the data before saving to file, more memory usage.
        private static async Task SaveAnimalsMSToFileAsync(string filePath, List<Animal> animals)
        {
            using MemoryStream memStream = new();
            await JsonSerializer.SerializeAsync(memStream, animals, new JsonSerializerOptions { WriteIndented = true });

            //some other operations:
            Console.WriteLine($"SaveAnimalsMSToFileAsync - JSON size: {memStream.Length} bytes");

            memStream.Position = 0; //reset position before read

            using (StreamReader reader = new(memStream, leaveOpen: true)) //leaveOpen: true to keep the memory stream open after reader is disposed
            {
                //read and display memory stream content
                Console.WriteLine("JSON content:");
                Console.WriteLine(await reader.ReadToEndAsync());
            }

            await using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            //copy memory stream content to file stream
            memStream.Position = 0; //reset position before copy
            await memStream.CopyToAsync(fileStream);
        }

        //Read list of animals from json file: file -> FileStream -> MemoryStream -> list animals
        private static async Task<List<Animal>> GetAnimalsMSFromFileAsync(string filePath)
        {
            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            using MemoryStream memStream = new();

            //copy file content into memory
            await fileStream.CopyToAsync(memStream);

            //some other operations:
            Console.WriteLine($"GetAnimalsMSFromFileAsync - JSON size: {memStream.Length} bytes");

            if (memStream.Length == 0)
            {
                return new List<Animal>();
            }

            //memStream.Position = 0;
            //using (StreamReader reader = new(memStream, leaveOpen: true))
            //{
            //    Console.WriteLine("JSON content:");
            //    Console.WriteLine(await reader.ReadToEndAsync());
            //}

            //reset position again before deserialization
            memStream.Position = 0;

            //deserialize from memory stream
            var animals = await JsonSerializer.DeserializeAsync<List<Animal>>(memStream);

            return animals ?? new List<Animal>();
        }

        //animal -> JSON -> MemoryStream
        private async Task<MemoryStream> SerializeToMemoryAsync(Animal animal)
        {
            var memoryStream = new MemoryStream();

            await JsonSerializer.SerializeAsync(memoryStream, animal, animal.GetType());

            memoryStream.Position = 0;

            return memoryStream;
        }
        #endregion


        #region Compresson with GZipStream
        //Compress log file:
        //file -> FileStream -> GZipStream -> FileStream -> log.txt.gz
        //GzipStream = wrapper around another decorator/stream; it doesn't save the file; it compresses bytes on the fly; the result is like an archive with one file
        private static async Task CompressLogAsync(string sourcePath, string compressedPath)
        {
            //open source file for reading
            await using FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            //create a new file for compressed data, overwrite if exists
            await using FileStream destinationStream = new FileStream(compressedPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            //create a GZipStream for compression, wrapping the destination stream
            await using GZipStream gzipStream = new GZipStream(destinationStream, CompressionMode.Compress);

            //copy data from source stream to GZipStream, which compresses it and writes to destination stream
            await sourceStream.CopyToAsync(gzipStream);
        }

        //Decompress log file:
        //file.gz -> FileStream -> GZipStream -> FileStream -> file restored
        private static async Task DecompressLogAsync(string compressedPath, string destinationPath)
        {
            await using FileStream sourceStream = new FileStream(compressedPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            await using GZipStream gzipStream = new GZipStream(sourceStream, CompressionMode.Decompress);

            await using FileStream destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            await gzipStream.CopyToAsync(destinationStream);
        }
        #endregion


        #region Misc
        //Display animal information with a delay
        private static async Task ProcessAnimalAsync(Animal animal)
        {
            Console.WriteLine($"Start {animal.Name} | Thread: {Environment.CurrentManagedThreadId}");

            await Task.Delay(Random.Shared.Next(1000, 2000));

            Console.WriteLine(animal.Speak());

            Console.WriteLine($"End {animal.Name} | Thread: {Environment.CurrentManagedThreadId}");
        }


        private static async Task WriteToLogAsync(string logPath, string message)
        {
            //we can use FileStream when we need control on the stream
            //using FileStream fileStream = new FileStream(logPath, FileMode.Open, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            //await using StreamWriter writer = new StreamWriter(fileStream);

            //or we can use StreamWriter directly, it opens the stream
            await using StreamWriter writer = new StreamWriter(logPath, append: true);

            await writer.WriteLineAsync($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.fff} - {message}");
        }



        private static async Task WriteUserToLogAsync(int userId, string logPath)
        {
            Console.WriteLine($"User {userId} -> WAITING to write to log");

            await logSemaphore.WaitAsync();

            try
            {
                Console.WriteLine($"User {userId} -> writing to log");

                await Task.Delay(1000);

                await WriteToLogAsync(logPath, $"User {userId} wrote with SemaphoreSlim");
            }
            finally
            {
                logSemaphore.Release();
                Console.WriteLine($"User {userId} -> finished");
            }
        }

        #endregion



        #region Genereate data for demo.

        // Simulates a CPU-intensive calculation and logs the thread on which it executes.
        private static int CalculateSomethingCPUIntensive(Animal animal)
        {
            Console.WriteLine($"-Calculating for {animal.Name} | Thread: {Environment.CurrentManagedThreadId}");

            int result = 0;
            foreach (int index in Enumerable.Range(0, 50_000_000))
            {
                result += index % 10;
            }
            Console.WriteLine($"-Finished calculation for {animal.Name} | Thread: {Environment.CurrentManagedThreadId}");

            return result;
        }

        // Writes 10 log entries at one-second intervals, stopping if cancellation is requested.
        private static async Task WriteLogsWithCancellationAsync(string logPath, CancellationToken cancellationToken)
        {
            var numberLogEntries = 10;

            foreach (int index in Enumerable.Range(1, numberLogEntries))
            {
                cancellationToken.ThrowIfCancellationRequested(); //check if cancellation is requested, if yes, throw OperationCanceledException

                await WriteToLogAsync(logPath, $"Cancellation demo - Log {index}/10");

                Console.WriteLine($"Log {index}/10 written.");

                await Task.Delay(1000, cancellationToken);
            }
        }
        #endregion
    }
}

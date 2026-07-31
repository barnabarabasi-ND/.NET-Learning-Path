using ConcurrentCollectionsDemo;
using System.Collections.Concurrent;

IProducerConsumerCollection<Job> pendingJobs = new ConcurrentBag<Job>();
var jobStates = new ConcurrentDictionary<Guid, JobState>();

var jobs = new[]
{
    new Job(Guid.NewGuid(), "Import customers", DateTimeOffset.Now),
    new Job(Guid.NewGuid(), "Generate invoices", DateTimeOffset.Now),
    new Job(Guid.NewGuid(), "Send notifications", DateTimeOffset.Now)
};

var resourcePool = new ConcurrentBag<WorkerResource>();

resourcePool.Add(new WorkerResource { Id = 1 });
resourcePool.Add(new WorkerResource { Id = 2 });

Console.WriteLine("Producing jobs...");

Task[] producerTasks = jobs
    .Select(job => Task.Run(() => ProduceJob(job)))
    .ToArray();

await Task.WhenAll(producerTasks);

Console.WriteLine();
Console.WriteLine("All producers completed.");
Console.WriteLine("Processing jobs...");

Task[] consumerTasks =
{
    Task.Run(() => ConsumeJobs(workerId: 1)),
    Task.Run(() => ConsumeJobs(workerId: 2))
};

await Task.WhenAll(consumerTasks);

Console.WriteLine();
Console.WriteLine("All consumers completed.");

bool removed = pendingJobs.TryTake(out Job? missingJob);

Console.WriteLine();
Console.WriteLine($"Removed from empty collection: {removed}");
Console.WriteLine($"Returned job is null: {missingJob is null}");

Console.WriteLine();
Console.WriteLine("Final states:");

foreach (Job job in jobs)
{
    Console.WriteLine($"{job.Name}: {jobStates[job.Id]}");
}

void ProduceJob(Job job)
{
    bool registered = jobStates.TryAdd(
        job.Id,
        JobState.Registered);

    if (!registered)
    {
        Console.WriteLine(
            $"Duplicate job rejected: {job.Name}");

        return;
    }

    bool markedPending = jobStates.TryUpdate(
        job.Id,
        JobState.Pending,
        JobState.Registered);

    if (!markedPending)
    {
        Console.WriteLine(
            $"Could not mark job as pending: {job.Name}");

        return;
    }

    bool firstAdded = pendingJobs.TryAdd(job);
    bool duplicateAdded = pendingJobs.TryAdd(job);

    Console.WriteLine(
        $"Producer task {Task.CurrentId}: " +
        $"{job.Name}, first added: {firstAdded}, " +
        $"duplicate added: {duplicateAdded}");
}

void ConsumeJobs(int workerId)
{
    while (pendingJobs.TryTake(out Job? job))
    {
        JobState previousState = jobStates[job.Id];

        Console.WriteLine(
            $"Worker {workerId} took {job.Name}, " +
            $"previous state: {previousState}");

        bool started = jobStates.TryUpdate(
            job.Id,
            JobState.Running,
            JobState.Pending);

        if (!started)
        {
            Console.WriteLine(
                $"Worker {workerId} rejected duplicate " +
                $"{job.Name}. Current state: {jobStates[job.Id]}");

            continue;
        }

        if (!resourcePool.TryTake(out WorkerResource? resource))
        {
            resource = new WorkerResource
            {
                Id = Random.Shared.Next(100, 1000)
            };

            Console.WriteLine(
                $"Worker {workerId} created resource {resource.Id}.");
        }

        try
        {
            Console.WriteLine(
                $"    Worker {workerId} is using resource {resource.Id}.");

            Console.WriteLine(
                $"        Running: {job.Name}");

            Thread.Sleep(250);

            bool completed = jobStates.TryUpdate(
                job.Id,
                JobState.Completed,
                JobState.Running);

            Console.WriteLine(
                completed
                    ? $"        Completed: {job.Name}"
                    : $"        Could not complete: {job.Name}");
        }
        finally
        {
            resource.Reset();
            resourcePool.Add(resource);

            Console.WriteLine(
                $"    Worker {workerId} returned resource " +
                $"{resource.Id} to the pool.");
        }
    }

    Console.WriteLine(
        $"Worker {workerId} found no more jobs.");
}
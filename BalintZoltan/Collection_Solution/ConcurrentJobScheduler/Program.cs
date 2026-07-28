using ConcurrentJobScheduler;

var jobsReg = new JobRegistry();
var acceptedJobs = new AcceptedJobTracker();

Console.WriteLine($"Job Wakeup added :{jobsReg.RegisterJob(1, "Wakeup")}");
Console.WriteLine($"Job Eat added :{jobsReg.RegisterJob(2, "Eat")}");
Console.WriteLine($"Job Work added :{jobsReg.RegisterJob(3, "Work")}");
Console.WriteLine($"Job Sleep added :{jobsReg.RegisterJob(1, "Sleep")}");

Console.WriteLine("Job list :");
jobsReg.PrintJobs();

if (jobsReg.TryGetJob(1, out var jobName))
{
    Console.WriteLine($"Found job: {jobName}");
}
Console.WriteLine($"Remove job 1 :{jobsReg.RemoveJob(1)}");
jobsReg.PrintJobs();

Console.WriteLine();
Console.WriteLine("====================");
Console.WriteLine();

int jobId = 2;

if (jobsReg.TryGetJob(jobId, out string? name))
{
    Console.WriteLine($"Add {name} to accepted job : {acceptedJobs.TryAccept(jobId)}");
    Console.WriteLine($"    Accepted job count: {acceptedJobs.Count}");
    Console.WriteLine($"Add {name} to accepted job : {acceptedJobs.TryAccept(jobId)}");
    Console.WriteLine($"    Accepted job count: {acceptedJobs.Count}");
    Console.WriteLine($"Job {name} is accepted? : {acceptedJobs.IsAccepted(jobId)}");
    Console.WriteLine($"Job {name} is removed? : {acceptedJobs.Remove(jobId)}");
    Console.WriteLine($"    Accepted job count: {acceptedJobs.Count}");
    Console.WriteLine($"Job {name} is accepted? : {acceptedJobs.IsAccepted(jobId)}");
    Console.WriteLine($"Job {name} is removed? : {acceptedJobs.Remove(jobId)}");
    Console.WriteLine($"    Accepted job count: {acceptedJobs.Count}");
}
else
{
    Console.WriteLine($"The {jobId} job not found in the Job List");
}

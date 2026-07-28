
var jobs = new Dictionary<int, string>();

jobs.Add(1, "Cleanup");
jobs.Add(2, "Backup");
jobs.Add(3, "Email");

Console.WriteLine("Registered jobs:");

foreach (var job in jobs)
{
    Console.WriteLine($"{job.Key} -> {job.Value}");
}

if (jobs.TryGetValue(2, out var jobName))
{
    Console.WriteLine($"Found: {jobName}");
}

Console.WriteLine("Find Job 10");

if (jobs.TryGetValue(10, out var jobName2))
{
    Console.WriteLine(jobName2);
}
else
{
    Console.WriteLine("Job not found.");
}

Console.WriteLine("Job 3 exist : " + jobs.ContainsKey(3));
Console.WriteLine("Job 100 exist : " + jobs.ContainsKey(100));

Console.WriteLine("Remove Job 2.");
if (jobs.Remove(2))
{
    Console.WriteLine("Job removed.");
}

Console.WriteLine();

foreach (var job in jobs)
{
    Console.WriteLine($"{job.Key} -> {job.Value}");
}


Console.WriteLine("===================");

LinkedList<string> jobs = new();                        // No index, 

jobs.AddLast("Email");
jobs.AddLast("Backup");
jobs.AddLast("Cleanup");

Console.WriteLine("Jobs:");

foreach (string job in jobs)
{
    Console.WriteLine(job);
}

jobs.AddFirst("Urgent");

foreach (string job in jobs)
{
    Console.WriteLine(job);
}

Console.WriteLine("===================");

LinkedList<string> jobs2 = new();
jobs2.AddLast("Email");
jobs2.AddLast("Backup");
jobs2.AddLast("Cleanup");
jobs2.AddLast("Report");

var node = jobs2.Find("Cleanup");
if (node is not null)
{
    jobs2.Remove(node);
    jobs2.AddFirst(node);
}

foreach (string job in jobs2)
{
    Console.WriteLine(job);
}
Console.WriteLine($"First: {jobs2.First?.Value}");
Console.WriteLine($"Last: {jobs2.Last?.Value}");
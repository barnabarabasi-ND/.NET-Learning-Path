namespace ConcurrentJobScheduler;
public class JobRegistry
{
    private readonly Dictionary<int, string> _jobs = new();
    public bool RegisterJob(int id, string name)
    {
        return _jobs.TryAdd(id, name);
    }
    public bool TryGetJob(int id, out string? name)
    {
        return _jobs.TryGetValue(id, out name);
    }
    public bool RemoveJob(int id)
    {
        return _jobs.Remove(id);
    }
    public void PrintJobs()
    {
        foreach (var job in _jobs)
        {
            Console.WriteLine($"Job key: {job.Key}       Value:{job.Value}");
        }
    }
}
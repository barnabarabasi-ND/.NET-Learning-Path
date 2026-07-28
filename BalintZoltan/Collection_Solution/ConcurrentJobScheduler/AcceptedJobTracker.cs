namespace ConcurrentJobScheduler;
public class AcceptedJobTracker
{
    private readonly HashSet<int> _acceptedJobs = new();
    public int Count => _acceptedJobs.Count;
    public bool TryAccept(int jobId)
    {
        return _acceptedJobs.Add(jobId);
    }
    public bool IsAccepted(int jobId)
    {  
        return _acceptedJobs.Contains(jobId);
    }
    public bool Remove(int jobId)
    {
        return _acceptedJobs.Remove(jobId);
    }
}
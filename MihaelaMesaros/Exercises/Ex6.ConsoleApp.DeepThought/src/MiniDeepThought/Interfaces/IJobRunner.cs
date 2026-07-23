using MiniDeepThought.Domain;

namespace MiniDeepThought.Interfaces
{
    public interface IJobRunner
    {
        Task RunJobAsync(Job job, IProgress<int> progress, CancellationToken cancellationToken);
    }
}

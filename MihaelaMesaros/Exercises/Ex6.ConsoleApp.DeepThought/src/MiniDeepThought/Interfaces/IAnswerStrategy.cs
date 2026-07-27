
namespace MiniDeepThought.Interfaces
{
    public interface IAnswerStrategy
    {
        Task<string> GenerateAnswerAsync(IProgress<int> progress, string? questionText, CancellationToken cancellationToken);
    }
}

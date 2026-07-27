
using MiniDeepThought.Interfaces;
using MiniDeepThought.Util;

namespace MiniDeepThought.Strategies
{
    public class SlowCountStrategy : IAnswerStrategy
    {
        //Requirement: loops from 1..N with small delays, reports progress, returns "42" at the end.
        public async Task<string> GenerateAnswerAsync(IProgress<int> progress, string? questionText = null, CancellationToken cancellationToken = default)
        {
            await ProgressHelper.SimulateProgress(progress, 2000, cancellationToken);
            
            return "42";
        }
    }
}

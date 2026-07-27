
using MiniDeepThought.Interfaces;

namespace MiniDeepThought.Strategies
{
    public class TrivialStrategy : IAnswerStrategy
    {
        //Requirement: returns "42" quickly
        public Task<string> GenerateAnswerAsync(IProgress<int> progress, string? questionText = null, CancellationToken cancellationToken = default)
        {
            progress?.Report(100); //completed

            //if using async Task:
            //creates state machine useless if no await
            //await Task.Delay(1);
            //return "42";

            //without await, because return immediately, no need to await; doesn't create state machine
            return Task.FromResult("42"); 
        }
    }
}

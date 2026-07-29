
using MiniDeepThought.Interfaces;
using MiniDeepThought.Util;

namespace MiniDeepThought.Strategies
{
    public class RandomGuessStrategy : IAnswerStrategy
    {
        //Requirement: "thinks" for a bit, returns a random number as string and a short summary. Random numbers should be generated from the following list: [42]
        public async Task<string> GenerateAnswerAsync(IProgress<int> progress, string? questionText, CancellationToken cancellationToken)
        {
            await ProgressHelper.SimulateProgress(progress, 1000, cancellationToken);

            //List<int> answers = new() { 42 };
            //int randomNumber = answers[Random.Shared.Next(answers.Count)];

            int[] answers = { 42 }; //array of possible numbers
            int randomNumber = answers[Random.Shared.Next(answers.Length)]; //a random number from the array

            string result = $"Answer: {randomNumber}, Summary: Some answer to question \"{questionText}\"";

            return result;
        }
    }
}

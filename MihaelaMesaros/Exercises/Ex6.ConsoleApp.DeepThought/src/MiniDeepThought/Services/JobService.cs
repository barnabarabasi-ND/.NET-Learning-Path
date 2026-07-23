
using MiniDeepThought.Domain;

namespace MiniDeepThought.Services
{
    public class JobService
    {
        /// <summary>
        /// Creates a new Job object.
        /// </summary>
        /// <param name="questionText">Question string.</param>
        /// <param name="algorithm">Algorithm name string.</param>
        /// <returns></returns>
        public Job CreateJob(string questionText, string algorithm) 
        {
            var newJob = new Job()
            {
                QuestionText = questionText,
                AlgorithmKey = algorithm,
            };

            return newJob;
        }
    }
}

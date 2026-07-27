
namespace MiniDeepThought.Util
{
    public static class ProgressHelper
    {
        /// <summary>
        /// Simulates progress for a job by reporting progress in steps with a specified delay between each step.
        /// </summary>
        /// <param name="progress">The progress reporter to report progress updates.</param>
        /// <param name="delay">The delay in milliseconds between each progress update.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SimulateProgress(IProgress<int> progress, int delay, CancellationToken cancellationToken) 
        {
            //number of steps from 100% progress
            var steps = 5;
            for (int p = 0; p < steps; p++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(delay, cancellationToken); //wait ms; here the operation is cancelled

                progress?.Report((p + 1) * 100 / steps); //sends the progress value to the progress reporter, the one which started the operation
            }
        }
    }
}


namespace DemoConfigureAwait.Services
{
    internal class DemoSynchronizationContext : SynchronizationContext
    {
        //override the Post method to handle the continuation of async operations
        public override void Post(SendOrPostCallback callback, object? state)
        {
            Console.WriteLine($"\n>>> Post() called on thread {Environment.CurrentManagedThreadId}");

            //callback(state);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(this);

                callback(state);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Post: {ex.Message}");
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

    }
}

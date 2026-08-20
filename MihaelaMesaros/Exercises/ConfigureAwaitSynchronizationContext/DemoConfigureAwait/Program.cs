using DemoConfigureAwait.Services;

////1. Console app does not have SynchronizationContext, it will be null.

//Console.WriteLine($"START");
//Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId}");
//Console.WriteLine($"SynchronizationContext: {SynchronizationContext.Current?.GetType().Name ?? "null"}");

//Console.WriteLine("\nBefore await");

//await Task.Delay(2000);
////continuing on a thread from thread pool...

//Console.WriteLine("\nAfter await");
//Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId}");
//Console.WriteLine($"SynchronizationContext: {SynchronizationContext.Current?.GetType().Name ?? "null"}");
//Console.WriteLine("");
//Console.WriteLine("");


//2. Adding own synchronization context

//create own SynchronizationContext
var syncContext = new DemoSynchronizationContext();

//set it as the current SynchronizationContext
SynchronizationContext.SetSynchronizationContext(syncContext);

Console.WriteLine("START");
Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId}");
Console.WriteLine($"Context: {SynchronizationContext.Current?.GetType().Name ?? "null"}");

Console.WriteLine("\nBefore await");

//====================================

await Task.Delay(2000).ConfigureAwait(false); //synchronization context will not be captured, the continuation does not need to resume on the captured SynchronizationContext

Console.WriteLine("\nAfter await ConfigureAwait false");
Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId}");
Console.WriteLine($"Context: {SynchronizationContext.Current?.GetType().Name ?? "null"}");

//====================================

////using normal await; synchronization context will be captured, continuation will run on the captured context
////await Task.Delay(2000); //the default behavior is to capture the synchronization context
//await Task.Delay(2000).ConfigureAwait(true);
//Console.WriteLine("\nAfter await ConfigureAwait true");
//Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId}");
//Console.WriteLine($"Context: {SynchronizationContext.Current?.GetType().Name ?? "null"}");






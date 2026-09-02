using Model.ExpensiveWorker;
using Model.WorkerPool;

Console.WriteLine("No Object Pool");
for (int i = 0; i < 10; i++)
{
    var workerObj = new ExpensiveWorker();
    Console.Write($"Object {i}: - created");
    workerObj.Data = " Nr. " + i;
    workerObj.Process();
    Console.WriteLine($" - finished.");
}

Console.WriteLine("Object Pool");
var pool = new WorkerPool();
ExpensiveWorker? worker = null;
for (int i = 0; i < 10; i++)
{
    worker = pool.Get();
    worker.Data = " Nr. " + i;
    try
    {
        worker.Process();
        Console.WriteLine($" - finished.");
    }
    catch (NullReferenceException e)
    {
        Console.WriteLine("Error in process method: " + e.Message);
    }
    finally
    {
        // Object reset
        if (worker != null)
        {
            worker.Data = "";
            pool.Return(worker);
        }
    }
}

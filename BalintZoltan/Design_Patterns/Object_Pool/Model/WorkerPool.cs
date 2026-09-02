namespace Model.WorkerPool;

using ExpensiveWorker;
public class WorkerPool
{
    private readonly Queue<ExpensiveWorker> _workers = new();
    public ExpensiveWorker Get()
    {
        if (_workers.Count > 0)
        {
            return _workers.Dequeue();
        }
        Console.Write($"Object {_workers.Count}: - created");
        return new ExpensiveWorker();
    }
    public void Return(ExpensiveWorker worker)
    {
        _workers.Enqueue(worker);
    }
}
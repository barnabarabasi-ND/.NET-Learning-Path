namespace Model.ExpensiveWorker;

public class ExpensiveWorker
{
    public string Data { get; set; } = "";
    public ExpensiveWorker()
    {
        // Expensive initialization
        Thread.Sleep(500);
    }

    public void Process()
    {
        Console.Write($"{Data} - runing ");
        // Process work
    }
}

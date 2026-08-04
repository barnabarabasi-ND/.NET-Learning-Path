Task<int> task = Task.Run(() =>
{
    Console.WriteLine("Working...");
    Thread.Sleep(2000);
    return 0;
});

int value = task.Result;                    // Blocking !!!
Console.WriteLine("Task returned value: "+value);

task.Wait();                                // Blocking !!!


int result = await Task.Run(() =>
{
    Thread.Sleep(5000);
    return 42;
});
Console.WriteLine("Task returned value: " + result);

Example();

async Task Example()
{
    Console.WriteLine("A");

    await Task.Delay(3000);

    Console.WriteLine("B");
}
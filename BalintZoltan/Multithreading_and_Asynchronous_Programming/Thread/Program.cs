using System.Runtime.InteropServices;


[DllImport("Kernel32.dll")]

static extern int GetCurrentThreadId();
Thread t = new Thread(() =>
{
    Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Started . . .");
    Console.WriteLine($"Current Native Thread Id: {GetCurrentThreadId()}");
    Thread.Sleep(2000);
    Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Finished");
});

Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Started");

t.Start();

Console.WriteLine("Join . . .");
t.Join();                                             // Wait until Thread finished

Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Continues");

Console.WriteLine($"Current Native Thread Id: {GetCurrentThreadId()}");
Console.WriteLine("Press any key.....");
Console.ReadKey();




ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine("Hello from ThreadPool 1");
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine("[ThreadPool 1]" + i);
        Thread.Sleep(500);
    }
});
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine("Hello from ThreadPool 2");
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine("[ThreadPool 2]" + i);
        Thread.Sleep(500);
    }
});

Console.WriteLine("Main");
for (int i = 0; i < 10; i++)
{
    Console.WriteLine("[Main]" + i);
    Thread.Sleep(500);
}
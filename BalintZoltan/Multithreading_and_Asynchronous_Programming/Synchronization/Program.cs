using System.Runtime.InteropServices;
[DllImport("Kernel32.dll")]

static extern int GetCurrentThreadId();
HashSet<int> threadID = new HashSet<int>();
object obj = new();

Console.WriteLine("Hello, World!");
int counter = 0;

Parallel.For(0, 100000, i =>
{
// With lock
    //lock (obj)
    //{
    //    counter++;
    //    threadID.Add(GetCurrentThreadId());
    //}

// Without lock

    //counter++;
    //threadID.Add(GetCurrentThreadId());

//Interlocked
    Interlocked.Increment(ref counter);                                     // Only simple execution , Not for : threadID.Add(GetCurrentThreadId());
});
Console.WriteLine("Number of thread : " + threadID.Count());
Console.WriteLine(counter);
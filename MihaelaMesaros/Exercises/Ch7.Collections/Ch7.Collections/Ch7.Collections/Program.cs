using System.Collections.Concurrent;
using System.Diagnostics;

Console.WriteLine("-----------------------------");
Console.WriteLine("Stack");
Stack<string> stack = new(); //IEnumerable<T>

stack.Push("1st");
stack.Push("2nd");
stack.Push("3rd");

foreach (string item in stack)
{
    Console.WriteLine(item);
}

Console.WriteLine($"Read from top: {stack.Peek()}");

Console.WriteLine($"Remove and return from top: {stack.Pop()}");
Console.WriteLine($"Remove and return from top: {stack.Pop()}");  

stack.Clear();
//Console.WriteLine(stack.Peek());

if (stack.TryPeek(out string value))
{
    Console.WriteLine(value);
} else
{
    Console.WriteLine("Stack is empty.");
}
if (stack.TryPop(out string value2))
{
    Console.WriteLine(value2);
}
else
{
    Console.WriteLine("Stack is empty.");
}


Console.WriteLine("-----------------------------");
Console.WriteLine("Queue");
Queue<string> queue = new();

queue.Enqueue("1st");
queue.Enqueue("2nd");
queue.Enqueue("3rd");
foreach (string item in queue)
{
    Console.WriteLine(item);
}

Console.WriteLine($"Read from front: {queue.Peek()}");

Console.WriteLine($"Remove and return from front: {queue.Dequeue()}");
Console.WriteLine($"Remove and return from front: {queue.Dequeue()}");

Console.WriteLine($"Read from top: {queue.Peek()}");
queue.Clear();

if (queue.TryPeek(out string value3))
{
    Console.WriteLine(value3);
}
else
{
    Console.WriteLine("Queue is empty.");
}
if (queue.TryDequeue(out string value4))
{
    Console.WriteLine(value4);
}
else
{
    Console.WriteLine("Queue is empty.");
}


Console.WriteLine("-----------------------------");
Console.WriteLine("Linked list");
//when need to insert/remove in the middle of collection
LinkedList<string> list = new();

list.AddLast("add last 1");
list.AddLast("add last 2");
list.AddFirst("add first 1");

foreach (string name in list)
{
    Console.WriteLine(name);
}

Console.WriteLine("");
list.AddLast("add last 3");
list.AddLast("add last 4");

// find node
LinkedListNode<string>? node = list.Find("add last 3");
list.AddAfter(node!, "add last 33");

foreach (string name in list)
{
    Console.WriteLine(name);
}

Console.WriteLine("");
Console.WriteLine("Remove abc");
list.Remove("abc");
foreach (string name in list)
{
    Console.WriteLine(name);
}

Console.WriteLine("");
Console.WriteLine("RemoveFirst");
list.RemoveFirst();
foreach (string name in list)
{
    Console.WriteLine(name);
}

Console.WriteLine("");
Console.WriteLine("RemoveLast");
list.RemoveLast();
foreach (string name in list)
{
    Console.WriteLine(name);
}


Console.WriteLine("-----------------------------");
Console.WriteLine("Dictionary");
Dictionary<int, string> students = new();

students.Add(1, "Ana");
students.Add(2, "Maria");
students.Add(3, "Ion");

Console.WriteLine(students[1]); // by key
Console.WriteLine(students.ContainsKey(2));
Console.WriteLine(students.ContainsValue("Ion"));

if (students.TryGetValue(3, out string? name2))
{
    Console.WriteLine(name2);
}

students[2] = "George";
Console.WriteLine(students[2]);

students.Remove(1);
Console.WriteLine(students.Count);
foreach (KeyValuePair<int, string> student in students)
{
    Console.WriteLine($"{student.Key} - {student.Value}");
}
students.Clear();
if (students.TryGetValue(3, out string? name3))
{
    Console.WriteLine(name3);
}
else
{
    Console.WriteLine("Element not found");
}


Console.WriteLine("-----------------------------");
Console.WriteLine("Hashset");
HashSet<string> fruits = new();

fruits.Add("Apple");
fruits.Add("Kiwi");
fruits.Add("Apple"); //will not be added
foreach (string fruit in fruits)
{
    Console.WriteLine(fruit);
}
Console.WriteLine(fruits.Contains("Apple"));
fruits.Remove("Apple");
foreach (string fruit in fruits)
{
    Console.WriteLine(fruit);
}
fruits.Clear();
Console.WriteLine(fruits.Count);



Console.WriteLine("-----------------------------");
Console.WriteLine("Compare List, Dictionary, Hashset");
List<int> list2 = new();
Dictionary<int, int> dictionary = new();
HashSet<int> hashSet = new();

for (int i = 0; i < 10000000; i++)
{
    list2.Add(i);
    dictionary.Add(i, i);
    hashSet.Add(i);
}

Stopwatch sw = new();

// List<T>
sw.Restart();
bool foundList = list2.Contains(1000);
sw.Stop();
Console.WriteLine($"List.Contains(): {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"List.Contains(): {sw.ElapsedTicks} ticks"); //Stopwatch ticks are based on the hardware timer frequency, used for high-precision performance measurements
sw.Reset();

// Dictionary<TKey,TValue>
sw.Restart();
bool foundDictionary = dictionary.ContainsKey(1000);
sw.Stop();
Console.WriteLine($"Dictionary.ContainsKey(): {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"Dictionary.ContainsKey(): {sw.ElapsedTicks} ticks");
sw.Reset();

// HashSet<T>
sw.Restart();
bool foundHashSet = hashSet.Contains(1000);
sw.Stop();
Console.WriteLine($"HashSet.Contains(): {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"HashSet.Contains(): {sw.ElapsedTicks} ticks");
sw.Reset();



Console.WriteLine("-----------------------------");
Console.WriteLine("List vs ConcurrentBag");

//1000 threads try to modify list:
//not thread safe
var list1 = new List<int>();
Parallel.For(0, 1000, i =>
{
    list1.Add(i);
});
Console.WriteLine($"list1: {list1.Count}"); //=> incorrect result

//protected with lock, only one thread can add
var listL = new List<int>();
object locker = new();

//runs the code on mutiple threads in parallel, only one thread can add
Parallel.For(0, 1000, i =>
{
    lock (locker)
    {
        listL.Add(i);
    }
});
Console.WriteLine($"list with lock: {listL.Count}");

//thread safe
var bag = new ConcurrentBag<int>(); //manages synchronization internally
//runs the code on mutiple threads in parallel, all threads can add
Parallel.For(0, 1000, i =>
{
    bag.Add(i);
});
Console.WriteLine($"ConcurrentBag: {bag.Count}");


//-------
//not thread safe, throws excepion
var dictNTS = new Dictionary<int, string>();
try
{
    Parallel.For(0, 2000, i =>
    {
        dictNTS[i] = $"Item {i}";
    });
}
catch (Exception ex)
{
    Console.WriteLine($"Dictionary exception: {ex.Message}");
}

//thread safe
var dictTS = new ConcurrentDictionary<int, string>();
Parallel.For(0, 2000, i =>
{
    dictTS.TryAdd(i, $"Item {i}");
});
Console.WriteLine($"ConcurrentDictionary: {dictTS.Count}");



Console.WriteLine("-----------------------------");
Console.WriteLine("Queue vs ConcurrentQueue");

var queueNTS = new Queue<int>();
try
{
    Parallel.For(0, 3000, i =>
    {
        queueNTS.Enqueue(i);
    });
    Console.WriteLine($"Queue: {queueNTS.Count}");
}
catch (Exception ex)
{
    Console.WriteLine($"Queue exception: {ex.Message}");
}

var queueTS = new ConcurrentQueue<int>();
Parallel.For(0, 3000, i =>
{
    queueTS.Enqueue(i);
});
Console.WriteLine($"ConcurrentQueue: {queueTS.Count}");


Console.WriteLine("-----------------------------");
Console.WriteLine("Stack vs ConcurrentStack");

var stackNTS = new Stack<int>();
Parallel.For(0, 4000, i =>
{
    stackNTS.Push(i);
});
Console.WriteLine($"Stack: {stackNTS.Count}");

var stackTS = new ConcurrentStack<int>();
Parallel.For(0, 4000, i =>
{
    stackTS.Push(i);
});
Console.WriteLine($"ConcurrentStack: {stackTS.Count}");


Console.WriteLine("-----------------------------");
Console.WriteLine("Producer-Consumer: Queue vs BlockingCollection");

var queueNTSPC = new Queue<int>();
try
{
    // Consumer
    int firstInQ = queueNTSPC.Dequeue(); // exception because it is empty
}
catch (Exception ex)
{
    Console.WriteLine($"Queue exception: {ex.Message}");
}

var jobsBlockColl = new BlockingCollection<int>();
// Producer
jobsBlockColl.Add(10);
// Consumer
int removeItem = jobsBlockColl.Take(); // waits if the collection is empty



Console.WriteLine("-----------------------------");
Console.WriteLine("IProducerConsumerCollection");
//interface that defines methods to manipulate thread-safe collections

IProducerConsumerCollection<string> collConBag = new ConcurrentBag<string>();
collConBag.TryAdd("A");
collConBag.TryAdd("B");
collConBag.TryAdd("C");
while (collConBag.TryTake(out var item))
{
    Console.WriteLine($"Removed from ConcurrentBag: {item}");
}

IProducerConsumerCollection<int> collConQ = new ConcurrentQueue<int>();
collConQ.TryAdd(10);
collConQ.TryAdd(20);
while (collConQ.TryTake(out var item))
{
    Console.WriteLine($"Removed from ConcurrentQueue: {item}");
}

using System.Collections.Concurrent;

namespace Chapter7Collections;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        ListExamples();
        StackExamples();
        QueueExamples();
        LinkedListExamples();
        DictionaryExamples();
        HashSetExamples();

        Console.WriteLine();
        await BlockingCollectionExamples();
    }

    private static void ListExamples()
    {
        /*
         * List<T> is the default collection to use when you need to store elements contiguously
         * and access them directly and you don't have other specific constraints.
         * 
         * Elements of the list can be accessed directly by their index.
         * 
         * Adding and removing elements at the end is very efficient, but doing so at the beginning or middle
         * is costly because it involves moving at least some of the elements.
         */

        var numbers = new List<int> { 1, 2, 3 };    // 1 2 3
        numbers.Add(5);                             // 1 2 3 5
        numbers.AddRange([7, 11]);                  // 1 2 3 5 7 11
        numbers.Insert(5, 1);                       // 1 2 3 5 7 1 11
        numbers.Insert(5, 1);                       // 1 2 3 5 7 1 1 11
        numbers.InsertRange(1, [13, 17, 19]);       // 1 13 17 19 2 3 5 7 1 1 11

        numbers.Remove(1);              // 13 17 19 2 3 5 7 1 1 11
        numbers.RemoveRange(2, 3);      // 13 17 5 7 1 1 11
        numbers.RemoveAll(e => e < 10); // 13 17 11
        numbers.RemoveAt(1);            // 13 11
        numbers.Clear();                // empty

        numbers = [1, 2, 3, 5, 7, 11];  // 1 2 3 5 7 11
        numbers.Find(e => e < 10);      // 1
        numbers.FindLast(e => e < 10);  // 7
        numbers.FindAll(e => e < 10);   // 1 2 3 5 7

        numbers = [1, 1, 2, 3, 5, 8, 11];   // 1 1 2 3 5 8 11
        numbers.FindIndex(e => e < 10);     // 0
        numbers.FindLastIndex(e => e < 10); // 5
        numbers.IndexOf(5);                 // 4
        numbers.LastIndexOf(1);             // 1
        numbers.BinarySearch(8);            // 5

        numbers = [1, 5, 3, 11, 8, 1, 2];   // 1 5 3 11 8 1 2
        numbers.Sort();                     // 1 1 2 3 5 8 11
        numbers.Reverse();                  // 11 8 5 3 2 1 1
    }

    private static void StackExamples()
    {
        /*
         * Stack<T> is the typical choice when you need a sequential list with the elements typically discarded after being retrieved in a LIFO manner.
         * Elements are added and removed from the top of the stack, both operations requiring constant time.
         */

        var numbers = new Stack<int>([1, 2, 3]);    // 3 2 1
        numbers.Push(5);                            // 5 3 2 1
        numbers.Push(7);                            // 7 5 3 2 1
        numbers.Pop();                              // 5 3 2 1
        var n = numbers.Peek();                     // n = 5
        numbers.Push(11);                           // 11 5 3 2 1
        numbers.Clear();                            // empty
    }

    private static void QueueExamples()
    {
        /*
         * Queue<T> is a good choice when you need a sequential list with the elements also discarded after being retrieved but in a FIFO manner.
         * Elements are added at the end and removed from the top of the queue. Both operations are very fast.
         */

        var numbers = new Queue<int>([1, 2, 3]);    // 1 2 3
        numbers.Enqueue(5);                         // 1 2 3 5
        numbers.Enqueue(7);                         // 1 2 3 5 7
        numbers.Dequeue();                          // 2 3 5 7
        var n = numbers.Peek();                     // n = 2
        numbers.Enqueue(11);                        // 2 3 5 7 11
        numbers.Clear();                            // empty
    }

    private static void LinkedListExamples()
    {
        /*
         * LinkedList<T> is useful when you need to add and remove many elements from the middle of the list and do it quickly.
         * However, this comes at the expense of the ability to randomly access the elements of the list (by their index).
         * The linked list does not store its elements contiguously and you must traverse the list from one end in order to find an element.
         */

        var numbers = new LinkedList<int>();    // empty
        var n2 = numbers.AddFirst(2);           // 2
        var n1 = numbers.AddFirst(1);           // 1 2
        var n7 = numbers.AddLast(7);            // 1 2 7
        var n11 = numbers.AddLast(11);          // 1 2 7 11
        var n3 = numbers.AddAfter(n2, 3);       // 1 2 3 7 11
        var n5 = numbers.AddBefore(n7, 5);      // 1 2 3 5 7 11

        var fn1 = numbers.Find(5);
        var fn2 = numbers.FindLast(5);
        Console.WriteLine(fn1 == fn2);              // True
        Console.WriteLine(numbers.Contains(3));     // True
        Console.WriteLine(numbers.Contains(13));    // False

        numbers.RemoveFirst();  // 2 3 5 7 11
        numbers.RemoveLast();   // 2 3 5 7
        numbers.Remove(3);      // 2 5 7
        numbers.Remove(n5);     // 2 7
        numbers.Clear();        // empty
    }

    private static void DictionaryExamples()
    {
        /*
         * Dictionary<TKey, TValue> should be used when you need to store values associated with a key.
         * Inserts, deletes, and lookups are very fast – they require constant time, regardless of the size of the dictionary.
         * 
         * The implementation uses a hash table, which means the keys are hashed and therefore the type of the key must implement GetHashCode() and Equals().
         * Alternatively, you need to provide an IEqualityComparer implementation upon the construction of the dictionary object.
         * 
         * The elements of a dictionary are stored unordered, which prevents you from traversing the values in the dictionary in a particular order.
         */

        var languages = new Dictionary<int, string>()
        {

            [1] = "C#",
            [2] = "Java",
            [3] = "Python",
            [4] = "C++"
        };

        languages.Add(5, "JavaScript");
        languages.TryAdd(5, "JavaScript");
        languages[6] = "F#";
        languages[5] = "TypeScript";

        Console.WriteLine($"Has 5: {languages.ContainsKey(5)}");
        Console.WriteLine($"Has C#: {languages.ContainsValue("C#")}");
        
        if (languages.TryGetValue(1, out var lang))
        {
            Console.WriteLine(lang);
        }
        else
        {
            Console.WriteLine("Not found!");
        }

        foreach (var kvp in languages)
        {
            Console.WriteLine($"[{kvp.Key}] = {kvp.Value}");
        }

        languages.Remove(5);
        languages.Clear();
    }

    private static void HashSetExamples()
    {
        /*
         * HashSet<T> is the collection you can use when you need a list of unique values.
         * Inserts, deletes, and lookups are very efficient.
         * 
         * The elements are stored unordered but contiguously.
         * 
         * A hash set is logically similar to a dictionary, where the values are also the keys, although it is a non-associative container.
         * For this reason, the type of its elements must implement GetHashCode() and Equals(),
         * or, alternatively, you must provide an IEqualityComparer implementation upon the construction of the hash set.
         */

        var numbers = new HashSet<int>() { 11, 3, 8 };  // 11 3 8
        numbers.Add(1);                                 // 11 3 8 1
        numbers.Add(1);                                 // 11 3 8 1
        numbers.Add(2);                                 // 11 3 8 1 2
        numbers.Add(5);                                 // 11 3 8 1 2 5

        Console.WriteLine(numbers.Contains(1));
        Console.WriteLine(numbers.Contains(7));

        numbers.Remove(1);                      // 11 3 8 2 5
        numbers.RemoveWhere(n => n % 2 == 0);   // 11 3 5
        numbers.Clear();                        // empty

        var a = new HashSet<int>() { 1, 2, 5, 6, 9 };   // 1 2 5 6 9
        var b = new HashSet<int>() { 1, 2, 3, 4 };      // 1 2 3 4

        var s1 = new HashSet<int>(a);
        s1.IntersectWith(b);            // 1 2

        var s2 = new HashSet<int>(a);
        s2.UnionWith(b);                // 1 2 5 6 9 3 4

        var s3 = new HashSet<int>(a);
        s3.ExceptWith(b);               // 5 6 9

        var s4 = new HashSet<int>(a);
        s4.SymmetricExceptWith(b);      // 4 3 5 6 9
    }

    private static async Task BlockingCollectionExamples()
    {
        using var bc = new BlockingCollection<int>();

        using var producer = Task.Run(() => {
            int a = 1, b = 1;
            
            bc.Add(a);
            bc.Add(b);

            for (int i = 0; i < 10; ++i)
            {
                int c = a + b;
                bc.Add(c);

                a = b;
                b = c;
            }

            bc.CompleteAdding();
        });

        using var consumer1 = Task.Run(() => {
            try
            {
                while (true)
                {
                    Console.WriteLine($"[1] {bc.Take()}");
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("[1] collection completed");
            }

            Console.WriteLine("[1] work done");
        });

        using var consumer2 = Task.Run(() => {
            foreach (var n in bc.GetConsumingEnumerable())
            {
                Console.WriteLine($"[2] {n}");
            }

            Console.WriteLine("[2] work done");
        });

        await Task.WhenAll(producer, consumer1, consumer2);
    }
}

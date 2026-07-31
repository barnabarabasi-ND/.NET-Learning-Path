
/********************
 * LIST
 * - dynamic
 * - mutable -> not read-only
 * - index based
 * *****************/

    var numbers_0 = new List<int>();
    // OR
    //var numbers_0 = new List<int> { 1, 2, 3, 5, 7, 11 };

    var words = new List<string>();
    // OR
    //var words = new List<string> { "one", "two" };

    var numbers = new List<int> { 1, 2, 3 };        // 1 2 3
    numbers.Add(5);                                 // 1 2 3 5
    numbers.AddRange(new int[] { 7, 11 });          // 1 2 3 5 7 11
    numbers.Insert(5, 1);                           // 1 2 3 5 7 '1' 11
    numbers.Insert(5, 1);                           // 1 2 3 5 7 '1' 1 11
    numbers.InsertRange(1,new int[] { 13, 17, 19 });// 1 '13 17 19' 2 3 5 7 1 1 11

    numbers.Remove(1);                              // (1) 13 17 19  2  3  5  7  1 1 11  // ! first occurrence of the element
    numbers.RemoveRange(2, 3);                      // 13 17 (19 2 3) 5  7  1  1 11
    numbers.RemoveAll(e => e < 10);                 // 13 17 (5  7  1  1) 11
    numbers.RemoveAt(1);                            // 13 (17) 11
    numbers.Clear();                                // empty

    numbers.AddRange(new int []{ 1, 2, 3, 5, 7, 11 });
    var a = numbers.Find(e => e < 10);              // 1    // ! first occurrence of the element
    var b = numbers.FindLast(e => e < 10);          // 7
    var c = numbers.FindAll(e => e < 10);           // 1 2 3 5 7

    // Zero based.
    numbers.Clear();                                // empty
    numbers.AddRange(new int[] { 1, 1, 2, 3, 5, 8, 11 });
    var a1 = numbers.FindIndex(e => e < 10);        // 0       // ! first occurrence of the element, Zero based.
    var b1 = numbers.FindLastIndex(e => e < 10);    // 5
    var c1 = numbers.IndexOf(5);                    // 4
    var d1 = numbers.LastIndexOf(1);                // 1
    var e1 = numbers.BinarySearch(8);               // 5

    numbers.Clear();                                // empty
    numbers.AddRange(new int[] { 1, 5, 3, 11, 8, 1, 2 });
    numbers.Sort();                                 // 1 1 2 3 5 8 11
    numbers.Reverse();                              // 11 8 5 3 2 1 1

/********************
 * STACK
 * - LIFO
 * - dynamic
 * - mutable -> not read-only
 * *****************/

    var arr = new string[] { "Ankit", "Marius", "Raffaele" };
    Stack<string> names_stack = new Stack<string>(arr);
    Stack<int> numbers1 = new Stack<int>();

    var numbers2 = new Stack<int>(new int[] { 1, 2, 3 });               // 3 2 1
    numbers2.Push(5);                                                   // '5' 3 2 1
    numbers2.Push(7);                                                   // '7' 5 3 2 1
    numbers2.Pop();                                                     // (7) 5 3 2 1
    var n = numbers2.Peek();                                            // 5 3 2 1          n = 5
    numbers2.Push(11);                                                  // '11' 5 3 2 1
    numbers2.TryPop(out int peek);                                      // (11) 5 3 2 1     peek = 11   return : true
    numbers2.TryPeek(out int peek2);                                    // 5 3 2 1          peek1 = 5   return : true
    numbers2.Clear();                                                   // empty

/********************
 * QUEUE
 * -FIFO
 * - dynamic
 * - mutable -> not read-only
 * *****************/

    //var arr = new string[] { "Ankit", "Marius", "Raffaele" };
    Queue<string> names_queue = new Queue<string>(arr);
    Queue<int> numbers3 = new Queue<int>();

    var numbers4 = new Queue<int>(new int[] { 1, 2, 3 });               // 1 2 3
    numbers4.Enqueue(5);                                                // 1 2 3 '5'
    numbers4.Enqueue(7);                                                // 1 2 3 5 '7'
    numbers4.Dequeue();                                                 // (1) 2 3 5 7
    var n1 = numbers4.Peek();                                           // 2 3 5 7          n1 = 2
    numbers4.Enqueue(11);                                               // 2 3 5 7 '11'
    numbers4.TryDequeue(out int result);                                // (2) 3 5 7        result = 2  return : true
    numbers4.TryPeek(out int result2);                                  // 3 5 7            result2 = 3 return : true
    numbers4.Clear();                                                   // empty
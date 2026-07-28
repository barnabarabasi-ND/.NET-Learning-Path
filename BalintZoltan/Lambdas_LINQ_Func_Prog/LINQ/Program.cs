//**********************************
//              LINQ
//   ( Language Integrated Query )
//**********************************

List<int> numbers = new() { 1, 2, 3, 4, 5, 6 };

// Normal
    List<int> result = new();

    foreach (int n in numbers)
    {
        if (n % 2 == 0)
            result.Add(n);
    }

    Console.Write($"Even numbers[normal]:");
    foreach (int n in result)
    {
        Console.Write($" {n} ");
    }
    Console.WriteLine();

// LINQ
    var result2 = numbers.Where(n => n % 2 == 0);           // Where always FILTER

    Console.Write($"Even numbers[LINQ]:");
    foreach (int n in result2)
    {
        Console.Write($" {n} ");
    }
    Console.WriteLine();

    var even = numbers.Where(x => x % 2 == 0);              // 2 4 6
    var doubled = numbers.Select(x => x * 2);               // 2 4 6 8 10 12    Select ALL element transform

// Method Chaining
    var result3 =
    numbers                                                 // 1 2 3 4 5 6
        .Where(x => x > 2)                                  // 3 4 5 6
        .Select(x => x * 10);                               // 30 40 50 60 


    var students = new List<Student>
    {
        new Student { Name="Anna", Age=20 },
        new Student { Name="Béla", Age=17 },
        new Student { Name="Csilla", Age=22 }
    };

    var adults =
        students
            .Where(s => s.Age >= 18)                            // Anna - 20 , Csilla - 22
            .Select(s => s.Name);                               // Anna , Csilla

// Deferred Execution
    //numbers.Add(8);
    var even2 = numbers.Where(x => x % 2 == 0);                 // Only prepare for action
    numbers.Add(8);
    Console.Write($"Even numbers[Deferred Execution]:");
    foreach (var x in even2)
    {
        Console.Write($" {x} ");                                // Filtering
    }
    Console.WriteLine();

// Immediate Execution
    numbers.Add(10);
    var even3 = numbers.Where(x => x % 2 == 0).ToList();
    //numbers.Add(10);
    Console.Write($"Even numbers[Immediate Execution]:");
    foreach (var x in even3)
    {
        Console.Write($" {x} ");
    }
    Console.WriteLine();

// Method Syntax
    var numbers1 = new List<int>
    {
        5,1,8,2,7
    };
    var result4 =
        numbers
            .Where(x => x > 2)
            .OrderBy(x => x)
            .Select(x => x * 10);

    Console.Write($"Result[Method Syntax]:");
    foreach (var x in result4)
    {
        Console.Write($" {x} ");
    }
    Console.WriteLine();

// Query Syntax
    var result5 =
        from x in numbers
        where x > 2
        orderby x
        select x * 10;
    Console.Write($"Result[Query Syntax]:");
    foreach (var x in result5)
    {
        Console.Write($" {x} ");
    }
    Console.WriteLine();


class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
}
//*****************
//   Imperativ
//*****************
    List<int> numbers = new() { 1, 2, 3, 4 };

    List<int> result = new();

    foreach (var n in numbers)
    {
        result.Add(n * 2);
        //Console.WriteLine($"Imperativ : {result[^1]}");     //  ^1 - Last value
    }
    Console.Write("Imperativ : ");
    foreach (var item in result)
    {
        Console.Write(item+" ");
    }
    Console.WriteLine();

//*****************
//   Functional
//*****************
    var result2 = numbers.Select(n => n * 2);

    Console.Write("Functional : ");
    foreach (var item in result)
    {
        Console.Write(item+" ");
    }
    Console.WriteLine();

//*****************
//  Pure Function
//*****************
    int Square(int x)
    {
        return x * x;
    }

    Console.WriteLine($"Square 5 = {Square(5)}");
    Console.WriteLine($"Square 5 = {Square(5)}");       //  No Side-effect

//*****************
//Non Pure Function
//*****************
int counter = 0;

    int Next()
    {
        counter++;
        return counter;
    }
    Console.WriteLine($"Next : {Next()}");
    Console.WriteLine($"Next : {Next()}");              // Side-effect
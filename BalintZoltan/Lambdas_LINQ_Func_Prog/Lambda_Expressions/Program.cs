//*****************
// Normal Function
//*****************
    int Square(int x)
    {
        return x * x;
    }

    int Apply(int x, Func<int, int> operation)
    {
        return operation(x);
    }

    int result = Apply(5, Square);
    Console.WriteLine($"Square 5 = {result}");   // 25

//*****************
//     Lambdas
//*****************
    
    int result2 = Apply(5, x => x * x);
    Console.WriteLine($"Square 5 = {result2}");   // 25

//*****************
//  Lambda + Func
//*****************
    Func<int, int> square = x => x * x;
    Console.WriteLine($"Square 5 = {square(5)}");

//*****************
// Lambda + Action
//*****************

    Action<string> print = s => Console.WriteLine(s);
    print("Hello");
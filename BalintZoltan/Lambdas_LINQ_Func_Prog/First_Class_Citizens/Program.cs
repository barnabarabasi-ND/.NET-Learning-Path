int Square(int x)
{
    return x * x;
}

Func<int, int> square = Square;

Console.WriteLine($"Square 5 = {square(5)}");   // 25

int Apply(int x, Func<int, int> operation)
{
    return operation(x);
}

int result = Apply(5, Square);
Console.WriteLine($"Square 5 = {result}");   // 25
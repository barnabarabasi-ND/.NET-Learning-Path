using Contravariant.Models;

Square sqr1 = new Square(4);
Square sqr2 = new Square(5);
Console.WriteLine($"{sqr1.Length} > {sqr2.Length} : {SquareComparison.IsBigger(sqr1, sqr2, new SquareComparer())}");
Console.WriteLine($"{sqr1.Length} > {sqr2.Length} : {SquareComparison.IsBigger(sqr1, sqr2, new ShapeComparer())}");
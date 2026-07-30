using Chapter6Generics.Interfaces;

namespace Chapter6Generics.Classes;

//internal class Circle : Shape<double>
//{
//    public double Radius { get; set; }
//    public override double Area => Math.PI * Radius * Radius;

//    public Circle(double radius)
//    {
//        Radius = radius;
//    }
//}

internal class Circle : IShape
{
    public double Radius { get; set; }
    public double Area => Math.PI * Radius * Radius;

    public Circle(double radius)
    {
        Radius = radius;
    }
}

using Chapter6Generics.Interfaces;

namespace Chapter6Generics.Structs;

/*
 * All of the rules that apply to generic classes also apply to generic structures.
 * Because value types do not support inheritance, structures cannot derive from other generic types
 * but can implement any number of generic or non-generic interfaces.
 */
internal struct Circle : IShape<double>
{
    public double Radius { get; set; }
    public double Area => Math.PI * Radius * Radius;

    public Circle(double radius)
    {
        Radius = radius;
    }
}

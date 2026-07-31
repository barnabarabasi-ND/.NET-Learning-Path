using Contravariant.Interfaces;

namespace Contravariant.Models;

public class Square : IShape
{
    public double Length { get; set; }
    public Square(int length)
    {
        Length = length;
    }
    public double Area => Length * Length;
}

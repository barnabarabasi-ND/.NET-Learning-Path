using Chapter6Generics.Interfaces;

namespace Chapter6Generics.Structs;

/*
 * All of the rules that apply to generic classes also apply to generic structures.
 * Because value types do not support inheritance, structures cannot derive from other generic types
 * but can implement any number of generic or non-generic interfaces.
 */
internal struct Square : IShape<int>
{
    public int Length { get; set; }
    public int Area => Length * Length;

    public Square(int length)
    {
        Length = length;
    }
}

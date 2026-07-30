namespace Chapter6Generics.Classes;

internal abstract class Shape<T>
{
    public abstract T Area { get; }
}

internal class Shape
{
}

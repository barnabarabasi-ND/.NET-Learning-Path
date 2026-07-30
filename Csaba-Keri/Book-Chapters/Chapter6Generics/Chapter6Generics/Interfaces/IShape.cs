namespace Chapter6Generics.Interfaces;

internal interface IShape<T>
{
    T Area { get; }
}

internal interface IShape
{
    double Area { get; }
}

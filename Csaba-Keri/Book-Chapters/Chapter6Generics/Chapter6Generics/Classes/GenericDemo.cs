namespace Chapter6Generics.Classes;

internal class GenericDemo<T>
{
    public T Value { get; private set; }

    public GenericDemo(T value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"{typeof(T)} : {Value}";
    }
}

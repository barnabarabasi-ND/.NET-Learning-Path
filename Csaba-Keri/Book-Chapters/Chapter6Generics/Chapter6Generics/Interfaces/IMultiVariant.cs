namespace Chapter6Generics.Interfaces;

internal interface IMultiVariant<out T, in U>
{
    T Make();
    void Take(U arg);
}

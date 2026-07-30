namespace Chapter6Generics.Classes;

internal class ConflictingGenerics<T>
{
    /*
     * If a generic method has a type parameter that is the same as a type parameter of the class,
     * structure, or interface where it is defined, the compiler issues a warning
     * because the method type parameter hides the type parameter of the outer type.
     */
    public void DoSomething<T>(T arg)
    {
        Console.WriteLine();
    }
}

namespace Chapter6Generics.Classes;

internal class SquareComparison
{
    public static bool IsBigger(Square a, Square b, IComparer<Square> comparer)
    {
        return comparer.Compare(a, b) >= 0;
    }
}

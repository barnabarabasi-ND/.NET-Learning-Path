using Contravariant.Models;

public class SquareComparer : IComparer<Square>
{
    public int Compare(Square x, Square y)
    {
        if (x is null) return y is null ? 0 : -1;
        if (y is null) return 1;
        return x.Length.CompareTo(y.Length);
    }
}

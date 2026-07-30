namespace Chapter6Generics.Classes;

internal class CircleComparer : IComparer<Circle>
{
    public int Compare(Circle? x, Circle? y)
    {
        if (x is null) return y is null ? 0 : -1;
        if (y is null) return 1;
        return x.Radius.CompareTo(y.Radius);
    }
}

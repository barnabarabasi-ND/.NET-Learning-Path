using Chapter6Generics.Interfaces;

namespace Chapter6Generics.Classes;

internal class ShapeComparer : IComparer<IShape>
{
    public int Compare(IShape? x, IShape? y)
    {
        if (x is null) return y is null ? 0 : -1;
        if (y is null) return 1;
        return x.Area.CompareTo(y.Area);
    }
}

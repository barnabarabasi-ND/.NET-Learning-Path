using Chapter6Generics.Interfaces;

namespace Chapter6Generics.Classes;

internal class Square<T> : Shape<T>
{
    public T Length { get; set; }

    /*
     * ERROR: Operator '*' cannot be applied to operands of type 'T' and 'T'
     */
    //public override T Area => Length * Length;
    public override T Area => throw new NotImplementedException();

    public Square(T length)
    {
        Length = length;
    }
}

//internal class Square : Shape<int>
//{
//    public int Length { get; set; }
//    public override int Area => Length * Length;

//    public Square(int length)
//    {
//        Length = length;
//    }
//}

//internal class Square : IShape<int>
//{
//    public int Length { get; set; }

//    public int Area => Length * Length;

//    public Square(int length)
//    {
//        Length = length;
//    }
//}

internal class Square : IShape
{
    public double Length { get; set; }

    public double Area => Length * Length;

    public Square(double length)
    {
        Length = length;
    }
}

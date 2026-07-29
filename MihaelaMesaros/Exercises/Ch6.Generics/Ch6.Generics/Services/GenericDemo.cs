
using Ch6.Generics.Helpers;

namespace Ch6.Generics.Services
{
    //generic class that can accept any type of data
    internal class GenericDemo<T>
    {
        //generic property that can accept any type of data
        public T Value { get; private set; }

        //generic constructor that can accept any type of data
        public GenericDemo(T value)
        {
            Value = value;
        }

    }
}

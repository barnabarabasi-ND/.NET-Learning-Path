
using Ch6.Generics.Helpers;

namespace Ch6.Generics.Services
{
    public class Pair<T1, T2> 
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }

        public Pair(T1 val1, T2 val2) {
            Value1 = val1;
            Value2 = val2;
        }

        //the generic method has as parameter a delegate with the logic of adding values
        //usage: pair.AddValues((val1, val2 => val1 + val2))
        public TResult AddValues<TResult>(Func<T1, T2, TResult> funcAddValues )
        {
            return funcAddValues(Value1, Value2);
        }

    }


}

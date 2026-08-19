using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace VariousDemos.Demos
{
    internal class LambdaLinqFunctionalProgramming
    {
        delegate int DelegateMathOperation(int a, int b);
        private static int _counter;

        public static void Run()
        {
            Console.WriteLine("=== Functional Programming ===");

            //-----------------------
            //Pure function = return same result for same parameters, no side effects
            //static int Add(int a, int b)
            //{
            //    return a + b;
            //}

            //Impure Function - has side effects (modifies a global variable, writes to console)
            //static int Add(int a, int b)
            //{
            //    _counter++;
            //    return a + b;
            //}

            //Immutability - variables are not modified, create new variable
            //int original = 10;
            //int updated = original + 5;

            //Declarative programming
            //instead: foreach (var n in numbers)
            //{
            //    if (n % 2 == 0)
            //        even.Add(n);
            //}
            //use: var even = numbers.Where(x => x % 2 == 0);



            //-----------------------
            //Functions as First-Class Citizens
            //in variable
            Func<int, int> square = x => x * x;
            Console.WriteLine($"square(5) = {square(5)}");

            //Func as parameter
            ExecuteOperation(10, x => x * 2);

            //Func returned by function
            var funcMultiplierBy10 = FuncMultiplierBy10(10); //returned func which multiplies by 10
            Console.WriteLine($"funcMultiplierBy10(8) = {funcMultiplierBy10(8)}");

            //list of func
            List<Func<int, int>> listFuncOp =
            [
                x => x + x,
                x => x * x
            ];
            foreach (var funcOp in listFuncOp)
            {
                Console.WriteLine($"funcOp(10) = {funcOp(10)}");
            }

            List<int> listNumbers = [1, 2, 3, 4, 5, 6, 7, 8];
            Predicate<int> isEven = x => x % 2 == 0;
            List<int> listEvenNumbers = listNumbers.FindAll(isEven);
            Console.WriteLine($"even numbers: {string.Join(",", listEvenNumbers)} ");

            //-----------------------
            Action<int, int> printSum = (a, b) =>
            {
                Console.WriteLine($"{a} + {b} = {a + b}");
            };

            printSum(10, 20);

            var menuActions = new[]
            {
                //new {Id = 1, Title = "Sum", Execute = (Action)(() => Console.WriteLine(DateTime.Now)) },
                new {Id = 1, Title = "Sum", Execute = (Action<int, int>)((a, b) => {Console.WriteLine($"{a} + {b} = {a + b}"); }) },
                new {Id = 2, Title = "Multiply", Execute = (Action<int, int>)((a, b) => {Console.WriteLine($"{a} * {b} = {a * b}"); }) }
            };
            menuActions[1].Execute(100, 200);

            Dictionary<int, Action> menuActionsDict = new()
            {
                [1] = () => Console.WriteLine(DateTime.Now),
            };
            menuActionsDict[1]();

            //lambda + Func as First-Class Citizens
            Dictionary<int, Func<string, string, string>> menuFuncDict = new()
            {
                [1] = (a, b) => $"{a}{b}",
                [2] = (a, b) => $"{a.ToUpper()}{b.ToUpper()}"
            };
            Console.WriteLine($"{menuFuncDict[1]("abc", "def")}");

            //-----------------------

            //same delegate can be used for different methods with same signature - multicast delegate
            DelegateMathOperation delegateMathOperation;

            delegateMathOperation = AddValues;
            Console.WriteLine($"Delegate AddValues(10, 5) = {delegateMathOperation(10, 5)}");

            delegateMathOperation = MultiplyValues;
            Console.WriteLine($"Delegate MultiplyValues(10, 5) = {delegateMathOperation(10, 5)}");

            //anonymous delegate
            DelegateMathOperation delegateAnonymus = delegate (int a, int b)
            {
                return a + b;
            };
            Console.WriteLine($"delegateAnonymus(5, 8) = {delegateAnonymus(5, 8)}");

            //better with Func:
            Func<int, int, int> funcCalc = AddValues;
            Console.WriteLine($"func AddValues(10, 5) = {funcCalc(10, 5)}");
            funcCalc = MultiplyValues;
            Console.WriteLine($"func MultiplyValues(10, 5) = {funcCalc(10, 5)}");


            Console.WriteLine("");
            Console.WriteLine("=== Lambda, Linq, Functional Programming Demos ===");

            //lambda with async+await
            Func<Task<string>> funcAsyncGetTime = async () =>
            {
                await Task.Delay(1000);

                return DateTime.Now.ToLongTimeString();
            };
            //string time = await funcAsyncGetTime();
            string resultFuncAsyncGetTime = funcAsyncGetTime().GetAwaiter().GetResult();
            Console.WriteLine($"Lambda async/await = {resultFuncAsyncGetTime}");

            //lambda closure
            int factor = 10;
            Func<int, int> lambdaClosure = x => x * factor;
            Console.WriteLine($"lambdaClosure(5) = {lambdaClosure(5)}"); //50
            //closure risk: lambda catures variable reference, not the value + keeps the variable alive too long
            factor = 100;
            Console.WriteLine($"lambdaClosure(5) after factor change = {lambdaClosure(5)}"); //500

            Func<int, int> funcMultiplier = FuncMultiplierBy10(8); //method group, the funcion is not executed, just referenced
            Func<int, int> funcMultiplier2 = x => x * factor; //same, but with lambda expression

            //Partial Function Application = fixing some parameters of a function and returning a new function with fewer parameters
            Func<int, int> funcAdd5 = x => AddValues(5, x);
            Console.WriteLine($"Partial Function Application: {funcAdd5(10)}");

            //Currying = transforming a function that takes multiple arguments into a sequence of functions that each take a single argument
            //gets parameter "a" which returns a function that takes "b" and returns the result of a + b
            Func<int, Func<int, int>> funcCurrying = a => b => a + b;
            Console.WriteLine($"Curried Add: {funcCurrying(5)(10)}");

            //LINQ
            Console.WriteLine("Linq:");
            int[] numbers = { 8, 5, 2, 9, 1, 7, 4, 6, 3, 5 };
            string[] names = { "Amanda", "Ianis", "Mary", "Ingrid", "George", "george", "Amanda" };
            string[] words = { "Hi", "LINQ" };
            object[] values = {1,"Hello",2,true,5};

            Console.WriteLine($"OfType<int>: {string.Join(",", values.OfType<int>())}"); //1,2,5
            Console.WriteLine($"SelectMany: {string.Join(",", words.SelectMany(x => x))}"); //H,i,L,I,N,Q
            Console.WriteLine($"SkipWhile: {string.Join(",", numbers.SkipWhile(x => x < 5))}"); // 8,5,2,9,1,7,4,6,3,5
            Console.WriteLine($"TakeWhile: {string.Join(",", numbers.Take(5))}"); // 8,5,2,9,1
            Console.WriteLine($"TakeWhile: {string.Join(",", numbers.TakeWhile(x => x < 9))}"); // 8,5,2
            Console.WriteLine($"ElementAt(2): {numbers.ElementAt(2)}"); // 2
            Console.WriteLine($"First: {numbers.First()}"); // 8

            var customers = new[]
            {
                new { Id = 1, Name = "John" },
                new { Id = 2, Name = "Amy" },
                new { Id = 3, Name = "Jimmy" }
            };

            var orders = new[]
            {
                new { CustomerId = 1, Product = "Pizza" },
                new { CustomerId = 2, Product = "Hotdog" }
            };

            //inner join
            var result1 = customers.Join(
                orders, 
                c => c.Id, 
                o => o.CustomerId, 
                (c, o) => new
                {
                    c.Name,
                    o.Product
                }
            );
            foreach (var item in result1)
            {
                Console.WriteLine($"Join: Customer={item.Name}, Product={item.Product}");
            }

            //left join
            var result2 = customers.GroupJoin(
                orders,
                c => c.Id,
                o => o.CustomerId,
                (c, os) => new
                {
                    c.Name,
                    Orders = os
                }
            );
            foreach (var item in result2)
            {
                Console.WriteLine($"GroupJoin Customer={item.Name}, Orders:");
                foreach(var order in item.Orders)
                {
                    Console.WriteLine($"  Product={order.Product}");
                }
            }

            //creates a dictionary where the key is the result of the key selector function and the value is a collection of elements that share that key
            var lookup = numbers.ToLookup(x => x % 2 == 0);
            Console.WriteLine($"ToLookup true: {string.Join(",", lookup[true])}"); // 8,2,4,6
            Console.WriteLine($"ToLookup false: {string.Join(",", lookup[false])}"); // 5,9,1,7,3,5

            //var result = Enumerable.Range(1, 5); // 1,2,3,4,5

            //int[] a = { 1, 2, 3 };
            //int[] b = { 1, 2, 3 };
            //int[] c = { 1, 3, 2 };
            //bool result = a.SequenceEqual(b); // true
            //bool result = a.SequenceEqual(c); // false

            //combines the elements of a sequence into a single value by applying a function; is a reduction operation
            Console.WriteLine($"Aggregate: {numbers.Aggregate((a, b) => a + b)}"); // 50
            Console.WriteLine($"Aggregate: {names.Aggregate((a, b) => a + ", " + b)}"); // Amanda, Ianis, Mary, Ingrid, George
            //Monoid:
            var resultMonoid = names.Aggregate("", (acc, name) => acc == "" ? name : acc + ", " + name);
            Console.WriteLine($"Monoid: {resultMonoid}");


            //var result = numbers.AsEnumerable();
            //var result = numbers.AsQueryable();
            //ToLookup - immediate execution, returns a Lookup<TKey, TElement> collection that groups elements by a specified key
            Console.WriteLine($"ToLookup:");
            var resultLookup = names.ToLookup(x => x.ToUpper());
            foreach (var group in resultLookup)
            {
                Console.WriteLine(group.Key);
                foreach (var name in group)
                {
                    Console.WriteLine($"   {name}");
                }
            }
            Console.WriteLine($"GroupBy:");
            var resultGroupBy = names.GroupBy(x => x);
            foreach (var group in resultGroupBy)
            {
                Console.WriteLine(group.Key);
                foreach (var name in group)
                {
                    Console.WriteLine($"   {name}");
                }
            }


            //query syntax:
            //var result =
            //        from x in numbers
            //        where x > 4
            //        orderby x descending
            //        select x * x;

            //var result =
            //    from c in customers
            //    join o in orders
            //        on c.Id equals o.CustomerId
            //    select new
            //    {
            //        c.Name,
            //        o.Product
            //    };


        }



        static void ExecuteOperation(int value, Func<int, int> operation)
        {
            Console.WriteLine($"operation({value}) = {operation(value)}");
        }

        //method returns Func
        static Func<int, int> FuncMultiplierBy10(int factor)
        {
            return x => x * factor;
        }


        static int AddValues(int a, int b) => a + b;
        static int MultiplyValues(int a, int b) => a * b;


    }
}


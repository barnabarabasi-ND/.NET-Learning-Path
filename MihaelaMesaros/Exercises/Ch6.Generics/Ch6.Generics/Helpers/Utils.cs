using Ch6.Generics.Interfaces;
using Ch6.Generics.Models;
using System.Collections;
using System.Linq;

namespace Ch6.Generics.Helpers
{
    //generic method
    public static class Utils
    {

        public static string DisplayTypeValue<T>(T Value)
        {
            //if (Value is System.Collections.IEnumerable collection && !(Value is string))
            //{
            //    return $"Type: {typeof(T)}, collection={string.Join(",", collection.Cast<object>())}"; // collection.Cast<object>() converts the collection to an IEnumerable of objects 
            //}
            //else
            //{
                return $"Type: {typeof(T)}, Value={Value}";
            //}
        }

        //generic method overload
        public static string DisplayTypeValue<T>(IEnumerable<T> collection)
        {
            return $"Type: {typeof(T)}, collection={string.Join(",", collection.Cast<object>())}";
        }

        public static bool CompareValues<T>(T Value1, T Value2)
        {
            return Value1.Equals(Value2); //Equals for primitive types
        }


        public static void DisplayAnimals<T>(IEnumerable<T> animals) //using IEnumerable<T> for iteration
        {
            Console.WriteLine($"--- Iterate animals of type {typeof(T).Name}:");
            foreach (var animal in animals)
            {
                //properties info from Reflection
                var type = animal.GetType();
                Console.WriteLine($"Type: {type.Name}");
                var properties = type.GetProperties();
                foreach (var property in properties.OrderBy(p => p.MetadataToken)) //order by definition order
                {
                    var value = property.GetValue(animal);
                    Console.WriteLine($"{property.Name}: {value}");
                }
            }
            Console.WriteLine("");
        }

        public static void PrintAnimals(IReadRepository<Animal> repo)
        {
            Console.WriteLine($"--- Iterate Animals repo:");
            foreach (Animal animal in repo.GetAnimals())
            {
                //Console.WriteLine($"{animal.Name} {animal.Speak()}");
                var properties = animal.GetType().GetProperties();
                foreach (var property in properties.OrderBy(p => p.MetadataToken)) //order by definition order
                {
                    var value = property.GetValue(animal);
                    Console.WriteLine($"{property.Name}: {value}");
                }
            }
        }
    }
}

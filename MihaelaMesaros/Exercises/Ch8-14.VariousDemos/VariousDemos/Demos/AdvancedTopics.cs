using System.Reflection;
using System.Text.RegularExpressions;
using VariousDemos.Services;
using VariousDemos.Models;

namespace VariousDemos.Demos
{
    internal class AdvancedTopics
    {
        delegate void DelegateMessage(); //stores a reference to a method void and no parameter

        internal static void Run()
        {
            Console.WriteLine("=== Advanced Topics Demos ===");

            Console.WriteLine("----------------------------");
            Console.WriteLine("Delegates");

            DelegateMessage delegateMessage = MethodMessage; //create a delegate instance and assign a method to it
            delegateMessage(); //call the method through the delegate

            Console.WriteLine("----------------------------");
            Console.WriteLine("Events");


            //Publisher-Subscriber (Observer Pattern)
            var publisher = new EventPublisher();
            var subscriber = new EventSubscriber1();

            //subscribe to the event
            publisher.Finished += subscriber.OnFinished;
            publisher.DoWork1();

            //unsubscribe from the event
            publisher.Finished -= subscriber.OnFinished;
            publisher.DoWork2();


            //Anonymous types
            //properties are read-only and cannot be changed after creation (immutable)
            var person = new { Name = "John", Age = 30 };
            var employees = new[]
            {
                new { Name = "Ana", Age = 25 },
                new { Name = "John", Age = 35 },
                new { Name = "Maria", Age = 45 }
            };

            //Person person = new("John", 30);
            //public record Person(string Name, int Age);
            //anonymus vs record: record is like a class with properties, methods, and can be inherited; anonymous type is a simple object with properties only

            //Tuples - used for grouping values without having a class
            Tuple<string, int> person1 = Tuple.Create("John", 30); //is reference type and immutable, older version of tuples

            //value tuples (ValueTuple) are mutable and can be used to return multiple values from a method
            //ValueTuple is a struct, permit names, support deconstruction, and can be used in pattern matching
            (string Name, int Age) person2 = ("John", 30);
            //Console.WriteLine(person2.Name);

            //method returning a tuple containing more values
            var result = GetMinMax(new[] { 10, 4, 25, 8 });
            Console.WriteLine(result.Min); // 4
            Console.WriteLine(result.Max); // 25

            //Tuple vs ValueTuple

            //Pattern Matching
            Console.WriteLine("Pattern Matching:");
            object value = "Hello";
            if (value is string text) { }

            int age = 20;
            if (person is { Age: >= 18 and < 65 }) { } //Logical Pattern

            //if (number is 42) {} //Constant Pattern

            //Relational Pattern
            string category = age switch
            {
                < 13 => "Child",
                < 18 => "Teen",
                >= 18 => "Adult"
            };

            Animal animal = new Dog { Name = "Spike" };

            //Property Pattern + variable; can extract properties.
            if (animal is Dog { Name: var name, IsTrained: true }) { }

            //List Pattern (from C# 11)
            int[] values = { 1, 2, 3 };
            if (values is [1, 2, 3])
            //or:
            if (values is [1, _, 3])
            {
                Console.WriteLine("Middle doesn't matter");
            }


            //Switch
            Animal animal2 = new Dog { Name = "Spike" };
            switch (animal2)
            {
                case Dog dog:
                    Console.WriteLine($"Dog: {dog.Name}");
                    break;
                case Cat cat:
                    Console.WriteLine($"Cat: {cat.Name}");
                    break;
                default:
                    Console.WriteLine("Unknown");
                    break;
            }

            //Switch Expression (C# 8) more compact
            Console.WriteLine("Switch Expression:");
            string sound = animal2 switch
            {
                Dog => "Woof",
                Cat => "Meow",
                _ => "Unknown"
            };
            Console.WriteLine($"Sound: {sound}");

            string animalDescription = animal switch
            {
                Dog dog => $"Dog {dog.Name}",
                Cat cat => $"Cat {cat.Name}",
                _ => "Unknown"
            };
            Console.WriteLine($"Animal description: {animalDescription}");


            Console.WriteLine("Regular Expressions:");
            //Regular Expressions
            string text1 = "Order number is 12345";
            bool result1 = Regex.IsMatch(text1, @"\d+");
            Console.WriteLine($"Result1: {result1}"); //true

            string email = "john.doe.aaa-bbb@example.com";
            string pattern = @"^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,}$";
            bool isValid = Regex.IsMatch(email, pattern);
            Console.WriteLine($"Email is valid: {isValid}"); // True

            //first match
            string text2 = "Products: 123, 456, 789";
            Match match = Regex.Match(text2, @"\d+");
            Console.WriteLine($"First match: {match.Value}");

            //all matches
            string text3 = "Products: 123, 456, 789, abc";
            MatchCollection matches = Regex.Matches(text3, @"\d+");
            foreach (Match match3 in matches)
            {
                Console.WriteLine($"All matches: {match3.Value}");
            }

            //replace string
            string text4 = "My phone is 0712345678";
            string result4 = Regex.Replace(text4, @"\d", "*");
            Console.WriteLine($"Replace: {result4}");

            //split string
            string text5 = "abc,def;123 456";
            string[] splitText5 = Regex.Split(text5, @"[,;\s]+"); //split by comma, semicolon, or whitespace
            foreach (string item in splitText5)
            {
                Console.WriteLine($"Split item: {item}");
            }



            //Extension methods = permit adding functionality to exsting types without modifying them, by creating static methods in static classes with the "this" keyword in the first parameter

            string textStringExt = "this text must be capitalized.";
            string resultStringExt = textStringExt.Capitalize();
            Console.WriteLine($"Capitalize method extension: {resultStringExt}");

            var dogExt = new Dog { Name = "Blake" };
            Console.WriteLine($"SpeakLouder method extension: {dogExt.SpeakLouder()}");

        }

        private static void MethodMessage()
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}: Some message");
        }

        private static (int Min, int Max) GetMinMax(int[] numbers)
        {
            return (numbers.Min(), numbers.Max());
        }

    }
}

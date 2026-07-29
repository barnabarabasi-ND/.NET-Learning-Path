
namespace Ch6.Generics.Models
{
    public class Dog: Animal
    {
        //calling constructor of base class (Animal, initialize properties, Dog)
        public Dog(string name, int age, bool isTrained) : base(name, age) 
        {
            //Console.WriteLine($"{name} created.");
            IsTrained = isTrained;
        }

        //constructor without parameters, for new() constraint in AnimalFactory<T>
        public Dog() : base() 
        { 
            IsTrained = false; 
        }

        public bool IsTrained { get; set; }

        public override string Speak()
        {
            return "Woof";
        }

    }
}

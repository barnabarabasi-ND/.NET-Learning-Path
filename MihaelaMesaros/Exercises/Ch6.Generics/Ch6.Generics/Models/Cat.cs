
namespace Ch6.Generics.Models
{
    public class Cat : Animal
    {
        //constructor without parameters, for new() constraint in AnimalFactory<T>
        public Cat() : base()
        {
            Kg = 0;
        }

        public Cat(string name, int age, double kg) : base(name, age)
        {
            //Console.WriteLine($"{name} created.");
            Kg = kg;
        }

        public double Kg { get; set; }

        public override string Speak()
        {
            return "Meow";
        }

    }
}

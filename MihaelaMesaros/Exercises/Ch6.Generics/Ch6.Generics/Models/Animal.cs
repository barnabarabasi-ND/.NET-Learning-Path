
namespace Ch6.Generics.Models
{
    public class Animal : IComparable<Animal>
    {
        //public string Name { get; set; } //with object initializer 
        public string Name { get; }
        public double Age { get; }

        //constructor without parameters, for new() constraint in AnimalFactory<T>
        public Animal()
        {
            Name = string.Empty;
            Age = 0;
        }

        //Name and age are required, so we use constructor to keep the object valid.
        public Animal(string name, int age)
        {
            //guard clauses
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (age <= 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Age must be greater than 0.");


            Name = name;
            Age = age;

            //Console.WriteLine($"Animal created: {name}");
        }

        public virtual string Speak() {
            return "Base Speak.";
        }

        //implementing CompareTo from IComparable
        public int CompareTo(Animal? other)
        {
            return Age.CompareTo(other?.Age);
        }

    }
}

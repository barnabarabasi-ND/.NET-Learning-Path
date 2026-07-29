using PetShelter.Interfaces;

namespace PetShelter.Models;

internal class Dog : Animal, IFeedable
{
    private const decimal ExtraDailyCareCost = 3m;

    public bool IsTrained { get; }

    public Dog(int id, string name, int age, bool isTrained) : base(id, name, age)
    {
        IsTrained = isTrained;
    }

    public override void Speak()
    {
        Console.WriteLine("Woof!");
    }

    public override decimal DailyCareCost()
    {
        return base.DailyCareCost() + ExtraDailyCareCost;
    }

    public void Feed()
    {
        Console.WriteLine($"Dog {Name} has been fed.");
    }
}

using PetShelter.Interfaces;

namespace PetShelter.Models;

internal class Cat : Animal, IFeedable
{
    private const decimal ExtraDailyCareCost = 2m;

    public bool IsIndoor { get; }

    public Cat(int id, string name, int age, bool isIndoor) : base(id, name, age)
    {
        IsIndoor = isIndoor;
    }

    public override void Speak()
    {
        Console.WriteLine("Meow!");
    }

    public override decimal DailyCareCost()
    {
        return base.DailyCareCost() + ExtraDailyCareCost;
    }

    public void Feed()
    {
        Console.WriteLine($"Cat {Name} has been fed.");
    }
}

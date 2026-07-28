using PetShelter.Interfaces;

namespace PetShelter.Models;

internal class Bird : Animal, IFeedable, IFlyable
{
    private const decimal ExtraDailyCareCost = 1m;

    public double WingSpanCm { get; }

    public Bird(int id, string name, int age, double wingSpanCm) : base(id, name, age)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(wingSpanCm);

        WingSpanCm = wingSpanCm;
    }

    public override void Speak()
    {
        Console.WriteLine("Chirp!");
    }

    public override decimal DailyCareCost()
    {
        return base.DailyCareCost() + ExtraDailyCareCost;
    }

    public void Feed()
    {
        Console.WriteLine($"Bird {Name} has been fed.");
    }

    public void Fly()
    {
        Console.WriteLine($"Bird {Name} is flying with a wingspan of {WingSpanCm} cm.");
    }
}

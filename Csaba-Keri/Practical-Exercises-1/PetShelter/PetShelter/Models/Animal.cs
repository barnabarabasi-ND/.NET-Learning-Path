namespace PetShelter.Models;

internal abstract class Animal
{
    private const decimal BaseDailyCareCost = 5m;

    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public DateTime IntakeDate { get; }

    protected Animal(int id, string name, int age)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(age);

        Id = id;
        Name = name;
        Age = age;
        IntakeDate = DateTime.Now;
    }

    public abstract void Speak();

    public virtual decimal DailyCareCost() => BaseDailyCareCost;
}

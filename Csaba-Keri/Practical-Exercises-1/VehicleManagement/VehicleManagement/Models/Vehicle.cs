namespace VehicleManagement.Models;

internal abstract class Vehicle
{
    public string Brand { get; }
    public string Model { get; }
    public int Year { get; }

    protected Vehicle(string brand, string model, int year)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(brand);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentOutOfRangeException.ThrowIfNegative(year);

        Brand = brand;
        Model = model;
        Year = year;
    }

    public virtual void StartEngine()
    {
        Console.WriteLine("The vehicle engine starts.");
    }
}

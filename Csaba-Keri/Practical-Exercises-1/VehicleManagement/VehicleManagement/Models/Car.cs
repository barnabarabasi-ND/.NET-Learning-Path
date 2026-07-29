using VehicleManagement.Interfaces;

namespace VehicleManagement.Models;

internal class Car : Vehicle, IDriveable
{
    public int NumberOfDoors { get; }

    public Car(string brand, string model, int year, int numberOfDoors) : base(brand, model, year)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfDoors);

        NumberOfDoors = numberOfDoors;
    }

    public override void StartEngine()
    {
        Console.WriteLine("The car engine starts with a key.");
    }

    public void Drive()
    {
        Console.WriteLine("The car is driving on the road.");
    }
}

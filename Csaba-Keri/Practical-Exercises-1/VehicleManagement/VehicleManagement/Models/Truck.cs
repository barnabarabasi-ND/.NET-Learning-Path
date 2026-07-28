using VehicleManagement.Interfaces;

namespace VehicleManagement.Models;

internal class Truck : Vehicle, IDriveable
{
    public int CargoCapacity { get; }

    public Truck(string brand, string model, int year, int cargoCapacity) : base(brand, model, year)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(cargoCapacity);

        CargoCapacity = cargoCapacity;
    }

    public override void StartEngine()
    {
        Console.WriteLine("The truck engine rumbles to life.");
    }

    public void Drive()
    {
        Console.WriteLine("The truck is driving on the road.");
    }
}

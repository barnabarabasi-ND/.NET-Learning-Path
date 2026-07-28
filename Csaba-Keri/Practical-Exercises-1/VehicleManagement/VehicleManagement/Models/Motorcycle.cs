using VehicleManagement.Interfaces;

namespace VehicleManagement.Models;

internal class Motorcycle : Vehicle, IDriveable
{
    public bool HasSidecar { get; }

    public Motorcycle(string brand, string model, int year, bool hasSidecar) : base(brand, model, year)
    {
        HasSidecar = hasSidecar;
    }

    public override void StartEngine()
    {
        Console.WriteLine("The motorcycle engine starts with a button.");
    }

    public void Drive()
    {
        Console.WriteLine("The motorcycle is driving on the road.");
    }
}

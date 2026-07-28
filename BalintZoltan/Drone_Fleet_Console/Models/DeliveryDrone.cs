namespace Drone_Fleet_Console.Models;

using Drone_Fleet_Console.Models.Interfaces;


public class DeliveryDrone : Drone, INavigable, ICargoCarrier
{
    public override DroneTypeEnum Type => DroneTypeEnum.Delivery;
    public double CapacityKg { get; } = 12;
    public double CurrentLoadKg { get; private set; }
    public (double lat, double lon)? CurrentWaypoint { get; private set; } = (0, 0);
    public override void TakeOff()
    {
        if ((!IsAirborne) && (BatteryPercent >= BatteryTakeOffLimit))
        {
            Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) is taking off...");
            IsAirborne = true;
        }
        else
        {
            Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) is already airborne or has low battery.");
        }
    }
    public override void Land()
    {
        if (IsAirborne)
        {
            Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) is landing...");
            IsAirborne = false;
        }
        else
        {
            Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) is already landed.");
        }
    }
    public override bool RunSelfTest()
    {
        if (BatteryPercent >= BatteryTakeOffLimit)
        {
            if (CurrentLoadKg < CapacityKg)
                Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) self-test passed.");
            return true;
        }
        Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) self-test failed.");
        return false;
    }
    public void SetWaypoint(double lat, double lon)
    {
        Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) waypoint set: {lat}, {lon}.");
        CurrentWaypoint = (lat, lon);
    }
    public bool Load(double kg)
    {
        if (!IsAirborne)
        {
            if (CurrentLoadKg + kg < CapacityKg)
            {
                Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) loaded with {kg} kg.");
                CurrentLoadKg = CurrentLoadKg + kg;
                return true;
            }
            else
            {
                Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) cannot load {kg} kg — capacity reached. Load unsuccessful.");
                return false;
            }
        }
        else
        {
            Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) is airborne.");
            return false;
        }
    }
    public void UnloadAll()
    {
        Console.WriteLine($"[DeliveryDrone]: The {Name} drone (ID {Id}) unloaded successfully.");
        CurrentLoadKg = 0;
    }
}

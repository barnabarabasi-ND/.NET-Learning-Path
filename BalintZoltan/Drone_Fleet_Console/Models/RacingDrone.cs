namespace Drone_Fleet_Console.Models;

public class RacingDrone : Drone
{
    public override DroneTypeEnum Type => DroneTypeEnum.Racing;
    public override void TakeOff()
    {
        if ((!IsAirborne) && (BatteryPercent >= BatteryTakeOffLimit))
        {
            Console.WriteLine($"[RacingDrone]: The {Name} drone (ID {Id}) is taking off...");
            IsAirborne = true;
        }
        else
        {
            Console.WriteLine($"[RacingDrone]: The {Name} drone (ID {Id}) is already airborne or has low battery.");
        }
    }
    public override void Land()
    {
        if (IsAirborne)
        {
            Console.WriteLine($"[RacingDrone]: The {Name} drone (ID {Id}) is landing...");
            IsAirborne = false;
        }
        else
        {
            Console.WriteLine($"[RacingDrone]: The {Name} drone (ID {Id}) is already landed.");
        }
    }
    public override bool RunSelfTest()
    {
        if (BatteryPercent >= BatteryTakeOffLimit)
        {
            Console.WriteLine($"[RacingDrone]: The {Name} drone (ID {Id}) self-test passed.");
            return true;
        }
        Console.WriteLine($"[RacingDrone]: The {Name} drone (ID {Id}) self-test failed.");
        return false;
    }

}

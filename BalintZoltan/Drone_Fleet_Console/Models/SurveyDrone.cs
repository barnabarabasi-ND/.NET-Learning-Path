namespace Drone_Fleet_Console.Models;

using Drone_Fleet_Console.Models.Interfaces;
public class SurveyDrone : Drone, IPhotoCapture, INavigable
{
    public override DroneTypeEnum Type => DroneTypeEnum.Survey;
    private int _photoCountMax { get; } = 2000;
    public int PhotoCount { get; private set; } = 0;
    public (double lat, double lon)? CurrentWaypoint { get; private set; } = (0, 0);
    public override void TakeOff()
    {
        if ((!IsAirborne) && (BatteryPercent >= BatteryTakeOffLimit))
        {
            Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) is taking off...");
            IsAirborne = true;
        }
        else
        {
            Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) is already airborne or has low battery.");
        }
    }
    public override void Land()
    {
        if (IsAirborne)
        {
            Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) is landing...");
            IsAirborne = false;
        }
        else
        {
            Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) is already landed.");
        }
    }
    public override bool RunSelfTest()
    {
        if (BatteryPercent >= BatteryTakeOffLimit)
        {
            if (PhotoCount < _photoCountMax)
            {
                Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) self-test passed.");
                return true;
            }
        }
        Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) self-test failed.");
        return false;
    }
    public void TakePhoto()
    {
        if (IsAirborne)
        {
            if (PhotoCount < _photoCountMax)
            {
                Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) is taking a photo...");
                PhotoCount++;
                Console.WriteLine($"[SurveyDrone]: Photo #{PhotoCount} created.");
            }
            else
            {
                Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) photo storage is full.");
            }
        }
        else
        {
            Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) has landed.");
        }
    }
    public void SetWaypoint(double lat, double lon)
    {
        Console.WriteLine($"[SurveyDrone]: The {Name} drone (ID {Id}) waypoint set: {lat}, {lon}.");
        CurrentWaypoint = (lat, lon);
    }
}

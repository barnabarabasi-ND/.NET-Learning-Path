using Smart_Home_Console_Remote;
using Smart_Home_Console_Remote.Models.Interfaces;

public class SmartPlug : SmartDevice, IMeasurableLoad
{
    public double CurrentWatts { get; private set; } = 0;
    public double TotalWh { get; private set; } = 0;
    public override int GetStatus()
    {
        return base.IsPoweredOn ? 1 : 0;
    }
    public override bool SelfTest()
    {
        return true;
    }
    public void ResetEnergy()
    {
        TotalWh = 0;
    }
    public void UpdateEnergy(double watts, double hours)
    {
        if (watts < 0 || hours < 0)
        {
            throw new ArgumentOutOfRangeException("Watts and hours must be non-negative.");
        }
        CurrentWatts = watts;
        TotalWh += CurrentWatts * hours;
    }
}
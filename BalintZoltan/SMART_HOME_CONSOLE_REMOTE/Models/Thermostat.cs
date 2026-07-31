using Smart_Home_Console_Remote;
using Smart_Home_Console_Remote.Models;
using Smart_Home_Console_Remote.Models.Interfaces;

public class Thermostat : SmartDevice, ITemperatureControl
{
    public double TargetCelsius { get; private set; } = 0;
    double Temperature { get; set; } = 0;
    public override int GetStatus()
    {
        //if (TargetCelsius >= Temperature)
        //{  
        //    return 1; // Heating is on;
        //}
        //else
        //{
        //    return 0; // Heating is off;
        //}
        return base.IsPoweredOn ? 1 : 0;
    }
    public override bool SelfTest()
    {
        return true;
    }
    public void SetTarget(double celsius)
    {
        if (celsius < 10 || celsius > 30)
        {
            throw new ArgumentOutOfRangeException(nameof(celsius), "Temperature must be between 10 and 30.");
        }
        TargetCelsius = celsius;
    }
    public void UpdateTemperature(double currentTemp)
    {
        if (currentTemp < 10 || currentTemp > 30)
        {
            throw new ArgumentOutOfRangeException(nameof(currentTemp), "Current temperature must be between 10 and 30.");
        }
        Temperature = currentTemp;
    }
}

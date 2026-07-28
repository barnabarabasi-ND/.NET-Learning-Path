namespace Smart_Home_Console_Remote.Models.Interfaces;

interface ITemperatureControl
{
    double TargetCelsius { get; }
    void SetTarget(double celsius);
}
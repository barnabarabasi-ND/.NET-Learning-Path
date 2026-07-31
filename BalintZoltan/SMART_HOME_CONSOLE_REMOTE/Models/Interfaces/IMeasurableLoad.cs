namespace Smart_Home_Console_Remote.Models.Interfaces;

interface IMeasurableLoad
{
    double CurrentWatts { get; }
    double TotalWh { get; }
    void ResetEnergy();
    void UpdateEnergy(double watts, double hours);
}
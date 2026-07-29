
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines power monitoring features for energy-measuring devices.
    /// </summary>
    internal interface IMeasurableLoad
    {
        double CurrentWatts { get; } //values cannot be modified where using interface, but can be modified by class which implements interface
        double TotalWh { get; }

        void ResetEnergy();
        void CalculateEnergy(double watts);
    }
}

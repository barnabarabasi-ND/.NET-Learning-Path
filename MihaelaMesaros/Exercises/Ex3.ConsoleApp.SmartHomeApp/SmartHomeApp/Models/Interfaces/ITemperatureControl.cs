
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines control for temperature-targeted devices.
    /// </summary>
    internal interface ITemperatureControl
    {
        public double TargetCelsius { get; }
        void SetTarget(double celsius);
    }
}

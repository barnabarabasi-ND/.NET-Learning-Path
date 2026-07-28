
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines control for humidifier devices.
    /// </summary>
    internal interface IHumidityControl
    {
        int Humidity { get; }

        void SetHumidity(int percent);
    }
}

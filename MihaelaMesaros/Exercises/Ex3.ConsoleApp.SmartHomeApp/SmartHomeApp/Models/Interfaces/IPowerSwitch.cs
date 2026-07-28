
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines standard power control operations common to all devices.
    /// Allows turning any device on/off in a consistent way.
    /// </summary>
    internal interface IPowerSwitch
    {
        void PowerOn(); 
        void PowerOff();
        void SetTime(TimeOnly? startTime, TimeOnly? endTime);
    }
}

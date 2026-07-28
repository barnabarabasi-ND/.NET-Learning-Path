
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines diagnostic behavior for checking if a device is functional.
    /// Demonstrates polymorphism — called on all devices via interface.
    /// </summary>
    internal interface ISelfTest
    {
        bool SelfTest();
    }
}

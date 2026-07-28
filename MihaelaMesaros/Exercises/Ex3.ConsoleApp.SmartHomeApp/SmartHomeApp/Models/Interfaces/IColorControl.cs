
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines control for color-changing devices.
    /// </summary>
    internal interface IColorControl
    {
        string Color { get; }
        void SetColor(string color);
    }
}

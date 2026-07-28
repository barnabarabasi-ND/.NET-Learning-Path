
namespace SmartHomeApp.Models.Interfaces
{
    /// <summary>
    /// Defines adjustable brightness capability.
    /// </summary>
    internal interface IDimmable
    {
        int Brightness { get; }
        void SetBrightness(int value);
    }
}

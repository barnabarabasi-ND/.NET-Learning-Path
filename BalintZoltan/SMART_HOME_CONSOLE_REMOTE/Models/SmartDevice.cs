using Smart_Home_Console_Remote.Models.Interfaces;

namespace Smart_Home_Console_Remote;

public abstract class SmartDevice : IPowerSwitch, ISelfTest
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public bool IsPoweredOn { get; set; } = false;
    public abstract int GetStatus();
    public abstract bool SelfTest();
    public void PowerOn()
    {
        IsPoweredOn = true;
    }
    public void PowerOff()
    {
        IsPoweredOn = false;
    }
    public string ReadDeviceInfo()
    {
        string deviceInfo = string.Empty;
        Console.Write($"Enter the Device name :");
        deviceInfo = Console.ReadLine();
        return deviceInfo;
    }

}

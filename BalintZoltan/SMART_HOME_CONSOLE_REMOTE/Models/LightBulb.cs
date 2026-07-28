using Smart_Home_Console_Remote;
using Smart_Home_Console_Remote.Models;
using Smart_Home_Console_Remote.Models.Interfaces;
public class LightBulb: SmartDevice, IDimmable
{
    public int Brightness { get; private set; } = 0;
    public void SetBrightness(int value)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Brightness must be between 0 and 100.");
        }
        Brightness = value;
    }

    public override int GetStatus()
    {
        return base.IsPoweredOn ? 1 : 0;
    }
    public override bool SelfTest()
    {
        return true;
    }
}

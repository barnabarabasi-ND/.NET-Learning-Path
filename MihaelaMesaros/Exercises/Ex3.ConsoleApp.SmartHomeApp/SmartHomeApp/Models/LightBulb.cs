using SmartHomeApp.Models.Interfaces;

namespace SmartHomeApp.Models
{
    internal class LightBulb : SmartDevice, IDimmable
    {
        public int Brightness { get; private set; } = 50; //"private set" permits class changes the property
        //public int Brightness { get; private set; } = 50; //changeable via method
        //public int Brightness { get; init; } = 50; //if no change method

        public void SetBrightness(int brightness)
        {
            if (brightness < 0 || brightness > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(brightness));
            }

            Brightness = brightness;
        }

        public override bool SelfTest()
        {
            return (Brightness > 0 && Brightness <= 100);
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Brightness: {Brightness}%";
        }
    }
}

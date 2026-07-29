using SmartHomeApp.Models.Interfaces;

namespace SmartHomeApp.Models
{
    internal class Humidifier : SmartDevice, IHumidityControl
    {
        public int Humidity { get; private set; }

        public void SetHumidity(int percent)
        {
            Humidity = percent;
        }

        public override bool SelfTest()
        {
            return (Humidity > 0 && Humidity <= 100);
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Humidity: {Humidity}%";
        }

    }
}

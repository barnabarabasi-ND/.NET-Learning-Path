using SmartHomeApp.Models.Interfaces;

namespace SmartHomeApp.Models
{
    internal class Thermostat : SmartDevice, ITemperatureControl
    {
        public double TargetCelsius { get; private set; } = 21;

        public void SetTarget(double celsius)
        {
            TargetCelsius = celsius;
        }

        public override bool SelfTest()
        {
            return (TargetCelsius >= 10 && TargetCelsius <= 30);
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Target Celsius: {TargetCelsius}";
        }

    }
}

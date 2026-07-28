using SmartHomeApp.Models.Interfaces;

namespace SmartHomeApp.Models
{
    internal class SmartPlug : SmartDevice, IMeasurableLoad
    {
        public double CurrentWatts { get; private set; }
        public double TotalWh { get; private set; }

        public void ResetEnergy()
        {
            CurrentWatts = 0;
            TotalWh = 0;
        }

        public void CalculateEnergy(double watts)
        {
            CurrentWatts = watts;
            TotalWh += watts;
        }

        /// <summary>
        /// Indicates if there is energy consuming.
        /// </summary>
        /// <returns>true/false</returns>
        public override bool SelfTest()
        {
            return CurrentWatts >= 0 && TotalWh >= 0;
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Current Watts: {CurrentWatts}, TotalWh: {TotalWh}";
        }
    }
}

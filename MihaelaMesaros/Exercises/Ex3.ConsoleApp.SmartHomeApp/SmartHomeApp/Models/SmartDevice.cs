using SmartHomeApp.Models.Interfaces;

namespace SmartHomeApp.Models
{
    internal abstract class SmartDevice : IPowerSwitch, ISelfTest
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public bool IsOn { get; private set; } = false;

        public TimeOnly? StartTime { get; private set; } //must be nullable to avoid default 00:00

        public TimeOnly? EndTime { get; private set; }

        public string GetStatus()
        {
            return (IsOn ? "ON" : "OFF");
        }

        public void SetTime(TimeOnly? startTime, TimeOnly? endTime)
        {
            if (startTime is null || endTime is null)
            {
                throw new ArgumentException("Start time and end time must have values.");
            }
            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            StartTime = startTime;
            EndTime = endTime;
        }

        //public void PowerOn() => IsOn = true;
        public void PowerOn()
        {
            IsOn = true;
        }
        public void PowerOff()
        {
            IsOn = false;
        }

        public abstract bool SelfTest();

        public virtual string GetDetails()
        {
            return $"Power: {GetStatus()}, Time interval: {StartTime}-{EndTime}";
        }

    }
}

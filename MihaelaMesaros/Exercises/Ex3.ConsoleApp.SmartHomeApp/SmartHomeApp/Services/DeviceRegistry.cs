using SmartHomeApp.Models;

namespace Services
{
    internal class DeviceRegistry
    {
        private readonly List<SmartDevice> _devices = new(); //reference to the list cannot be changed; points to the same list; list can be changed
        int _firstId = 1;

        /// <summary>
        /// Add new device with incremented Id.
        /// </summary>
        /// <param name="device">Device object to be added.</param>
        public void AddDevice(SmartDevice device)
        {
            device.Id = _firstId++;
            _devices.Add(device);
        }

        /// <summary>
        /// Returns the list of all registered devices.
        /// </summary>
        /// <returns></returns>
        public List<SmartDevice> GetDevices()
        {
            return _devices;
        }

        /// <summary>
        /// Returns a device by its Id if it exists; otherwise, returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SmartDevice? GetDeviceById(int id)
        {
            if (_devices != null && _devices.Any())
            {
                var device = _devices.FirstOrDefault(x => x.Id==id);
                return device;
            }
            return null;
        }

        /// <summary>
        /// Switches the power state of the given device. If the device is on, it will be turned off; if it is off, it will be turned on.
        /// </summary>
        /// <param name="device"></param>
        public void TogglePower(SmartDevice device)
        {
            if (device.IsOn)
                device.PowerOff();
            else
                device.PowerOn();
        }

    }
}

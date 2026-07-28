
using SmartHomeApp.Models;
using SmartHomeApp.Models.Interfaces;
using Services;

var deviceRegistry = new DeviceRegistry();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. List devices");
    Console.WriteLine("2. Add device");
    Console.WriteLine("3. Toggle power");
    Console.WriteLine("4. Device actions");
    Console.WriteLine("5. Self - test all");
    Console.WriteLine("6. Dashboard summary");
    Console.WriteLine("7. Exit");

    string? option = Console.ReadLine();
    if (option != null)
    {
        option = option.Trim();
        switch (option)
        {
            case "1": //List devices
                {
                    var devices = deviceRegistry.GetDevices();
                    if (devices == null || !devices.Any())
                    {
                        Console.WriteLine("No device has been found.");
                    }

                    //iterate devices
                    foreach (var item in devices!)
                    {
                        Console.WriteLine(
                            $"Id: {item.Id}, " +
                            $"Type: {item.GetType().Name}, " +
                            $"Name: {item.Name}, " +
                            $"{item.GetDetails()}"
                        );
                    }

                    break;
                }
            case "2": //Add device
                {
                    while (true)
                    {
                        Console.Write("Choose type (LightBulb (L) / Thermostat (T) / SmartPlug (P) / ColorBulb (C) / Humdifier (H)): ");
                        string? inputType = Console.ReadLine();
                        if (string.IsNullOrEmpty(inputType))
                        {
                            Console.WriteLine("Type is required.");
                            continue;
                        }
                        inputType = inputType.Trim().ToUpperInvariant();

                        if (inputType != "L"
                            && inputType != "T"
                            && inputType != "P"
                            && inputType != "C"
                            && inputType != "H"
                        )
                        {
                            Console.WriteLine("Type is not valid.");
                            continue;
                        }

                        Console.Write("\nEnter name: ");
                        string? inputName;
                        while (true)
                        {
                            inputName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputName))
                            {
                                Console.WriteLine("Name is required.");
                                continue;
                            }
                            inputName = inputName.Trim();
                            break;
                        }

                        // Polymorphism: one base type, multiple derived types.
                        // device can refer any SmartDevice object
                        SmartDevice device =
                            inputType.ToUpperInvariant() switch
                            {
                                "L" => new LightBulb { Name = inputName }, //creates actual object
                                "T" => new Thermostat { Name = inputName },
                                "P" => new SmartPlug { Name = inputName },
                                "C" => new ColorBulb { Name = inputName },
                                "H" => new Humidifier { Name = inputName },
                                _ => throw new ArgumentException("Invalid device type.")
                            };

                        deviceRegistry.AddDevice(device);

                        Console.WriteLine($"Added: " +
                            $"{device.Id}, " +
                            $"{device.GetType().Name} " +
                            $"\"{device.Name}\", " +
                            $"{device.GetDetails()}"
                        );

                        break;
                    }
                    break;
                }
            case "3": //Toggle power
                {
                    SmartDevice? device = SelectDevice(deviceRegistry);

                    if (device == null)
                    {
                        Console.WriteLine("Device not found.");
                        break;
                    }

                    deviceRegistry.TogglePower(device);
                    Console.WriteLine($"{device.GetType().Name} \"{device.Name}\" is now {device.GetStatus()}");

                    //when the SmartPlug becomes off, increment total watts
                    if (device is IMeasurableLoad measureable && device.IsOn == false)
                    {
                        measureable.CalculateEnergy(10); //some default CurrentWatts value for consumed energy
                        Console.WriteLine($"{device.GetType().Name} \"{device.Name}\" consumption: CurrentWatts={measureable.CurrentWatts} TotalWh={measureable.TotalWh}");
                    }

                    break;
                }
            case "4": //Device actions
                {
                    //select a device by Id
                    SmartDevice? device = SelectDevice(deviceRegistry);

                    if (device == null)
                    {
                        Console.WriteLine("Device not found.");
                        break;
                    }

                    //display device details
                    Console.WriteLine(
                        $"Id: {device.Id}, " +
                        $"Name: {device.Name}, " +
                        $"{device.GetDetails()}"
                    );

                    Console.WriteLine();

                    //submenu counter
                    int optionAction = 1;

                    //display submenu for device actions
                    if (device is IDimmable)
                    {
                        Console.WriteLine($"{optionAction}. Set brightness");
                        optionAction++;
                    }

                    if (device is ITemperatureControl)
                    {
                        Console.WriteLine($"{optionAction}. Set target temperature");
                        optionAction++;
                    }

                    if (device is IMeasurableLoad)
                    {
                        Console.WriteLine($"{optionAction}. Reset energy counter");
                        optionAction++;
                    }

                    if (device is IColorControl)
                    {
                        Console.WriteLine($"{optionAction}. Set color");
                        optionAction++;
                    }

                    if (device is IHumidityControl)
                    {
                        Console.WriteLine($"{optionAction}. Set humidity");
                        optionAction++;
                    }

                    Console.WriteLine($"{optionAction}. Set time interval");
                    optionAction++;

                    Console.WriteLine("0. Back");

                    string? inputAction;
                    Console.Write("\nChoose action: ");
                    while (true)
                    {
                        inputAction = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(inputAction))
                        {
                            Console.WriteLine("Device action is required.");
                            continue;
                        }
                        inputAction = inputAction.Trim();
                        break;
                    }

                    if (inputAction == "0")
                    {
                        break; //exit submenu
                    }

                    optionAction = 1;

                    //set Brightness
                    if (device is IDimmable dimmableAction)
                    {
                        if (inputAction == optionAction.ToString())
                        {
                            while (true)
                            {
                                Console.Write("Brightness (0-100): ");

                                if (!int.TryParse(Console.ReadLine(), out int brightness) || brightness < 0 || brightness > 100)
                                {
                                    Console.WriteLine("Brightness is not valid number.");
                                    continue;
                                }

                                dimmableAction.SetBrightness(brightness);
                                Console.WriteLine($"Brightness updated to {brightness}.");
                                break;
                            }

                            break;
                        }

                        optionAction++;
                    }

                    //set Target temperature
                    if (device is ITemperatureControl thermostatAction)
                    {
                        if (inputAction == optionAction.ToString())
                        {
                            while (true)
                            {
                                Console.Write("Target temperature (10-30 C): ");

                                if (!double.TryParse(Console.ReadLine(), out double temperature) || temperature < 10 || temperature > 30)
                                {
                                    Console.WriteLine("Target temperature is not valid.");
                                    continue;
                                }

                                thermostatAction.SetTarget(temperature);
                                Console.WriteLine($"Target temperature updated to {temperature}.");
                                break;
                            }

                            break;
                        }

                        optionAction++;
                    }

                    //reset energy counter 
                    if (device is IMeasurableLoad measurableAction)
                    {
                        if (inputAction == optionAction.ToString())
                        {
                            while (true)
                            {
                                Console.Write("Reset energy counter? (Y/N): ");
                                string? inputReset = Console.ReadLine()?.Trim();
                                if (string.IsNullOrWhiteSpace(inputReset))
                                {
                                    Console.WriteLine("Reset counter is required.");
                                    continue;
                                }
                                inputReset = inputReset.Trim().ToUpperInvariant();
                                if (inputReset != "Y" && inputReset != "N")
                                {
                                    Console.WriteLine("Reset counter is not valid.");
                                    continue;
                                }

                                measurableAction.ResetEnergy();
                                Console.WriteLine("Energy counter reset.");
                                break;
                            }

                            break;
                        }

                        optionAction++;
                    }

                    //set Color
                    if (device is IColorControl colorAction)
                    {
                        if (inputAction == optionAction.ToString())
                        {
                            while (true)
                            {
                                Console.Write("Color: ");
                                string? color = Console.ReadLine()?.Trim();

                                if (string.IsNullOrWhiteSpace(color))
                                {
                                    Console.WriteLine("Color is required.");
                                    continue;
                                }

                                colorAction.SetColor(color);
                                Console.WriteLine($"Color updated to {color}.");
                                break;
                            }

                            break;
                        }

                        optionAction++;
                    }

                    //set Humidity
                    if (device is IHumidityControl humidityAction)
                    {
                        if (inputAction == optionAction.ToString())
                        {
                            while (true)
                            {
                                Console.Write("Humidity (0-100): ");

                                if (!int.TryParse(Console.ReadLine(), out int humidity) || humidity < 0 || humidity > 100)
                                {
                                    Console.WriteLine("Humidity is not valid.");
                                    continue;
                                }

                                humidityAction.SetHumidity(humidity);
                                Console.WriteLine($"Humidity updated to {humidity}%.");
                                break;
                            }

                            break;
                        }

                        optionAction++;
                    }

                    //set time start and time end
                    if (inputAction == optionAction.ToString())
                    {
                        while (true)
                        {
                            Console.Write("\nStart time (hh:mm): ");
                            string? inputStartTime = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputStartTime))
                            {
                                Console.WriteLine("Start time is required.");
                                continue;
                            }
                            inputStartTime = inputStartTime.Trim();
                            TimeOnly startTime;
                            if (!TimeOnly.TryParseExact(inputStartTime, "HH:mm", out startTime))
                            {
                                Console.WriteLine("Start time is not valid.");
                                continue;
                            }

                            Console.Write("\nEnd time (hh:mm): ");
                            string? inputEndTime = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputEndTime))
                            {
                                Console.WriteLine("End time is required.");
                                continue;
                            }
                            inputEndTime = inputEndTime.Trim();
                            TimeOnly endTime;
                            if (!TimeOnly.TryParseExact(inputEndTime, "HH:mm", out endTime))
                            {
                                Console.WriteLine("End time is not valid.");
                                continue;
                            }

                            //set start time and end time for the device
                            device.SetTime(startTime, endTime);
                            Console.WriteLine($"Start time and End time updated to {startTime}-{endTime}.");
                            break;
                        }

                        break;
                    }

                    optionAction++;

                    Console.WriteLine("Invalid action in submenu.");
                    break;
                }
            case "5": //Self - test all
                {
                    var listDevices = deviceRegistry.GetDevices();
                    if (listDevices == null || !listDevices.Any())
                    {
                        Console.WriteLine("Devices list is empty.");
                    }
                    //iterate devices for showing polymorfism
                    foreach (var item in listDevices!)
                    {
                        bool testResult = item.SelfTest();
                        Console.Write($"\n{item.GetType().Name} \"{item.Name}\" self test: {(testResult ? "Pass" : "Fail")}");
                    }

                    break;
                }
            case "6": //Dashboard summary
                {
                    var listDevices = deviceRegistry.GetDevices();
                    if (listDevices == null || !listDevices.Any())
                    {
                        Console.WriteLine("Devices list is empty.");
                    }

                    int totalNoDevices = listDevices!.Count;
                    int noDevicesOn = listDevices.Count(d => d.IsOn == true);
                    int noDevicesOff = listDevices.Count(d => d.IsOn == false);

                    Console.WriteLine($"Total devices: {totalNoDevices}");
                    Console.WriteLine($"Devices ON : {noDevicesOn}");
                    Console.WriteLine($"Devices OFF: {noDevicesOff}");

                    break;
                }
            case "7":
                return;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

}

//Select a device by input Id. Returns the device object by Id.
SmartDevice? SelectDevice(DeviceRegistry registry)
{
    Console.Write("\nEnter device Id: ");
    int deviceId;
    while (true)
    {
        string? inputId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputId))
        {
            Console.Write("\nEnter device Id: ");
            continue;
        }
        if (!int.TryParse(inputId, out deviceId) || deviceId <= 0)
        {
            Console.WriteLine("\nInvalid device Id");
            continue;
        }
        break;
    }
    var device = deviceRegistry.GetDeviceById(deviceId);

    return device;
}

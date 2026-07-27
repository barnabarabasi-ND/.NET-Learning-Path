using VehicleManagement.Interfaces;
using VehicleManagement.Models;
using VehicleManagement.UI;

namespace VehicleManagement;

internal sealed class VehicleManagementApp
{
    private static readonly string MenuSeparator = new('-', 50);

    private readonly Dictionary<string, MenuOption> _menuOptions;
    private readonly List<Vehicle> _vehicles = [];
    private bool _isRunning = false;

    public VehicleManagementApp()
    {
        _menuOptions = new()
        {
            ["1"] = new("Add Car", AddCar),
            ["2"] = new("Add Motorcycle", AddMotorcycle),
            ["3"] = new("Add Truck", AddTruck),
            ["4"] = new("List Vehicles", ListVehicles),
            ["5"] = new("Exit", Exit)
        };
    }

    private static string? ReadUserInput()
    {
        return Console.ReadLine()?.Trim();
    }

    private static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }

            Console.WriteLine("The value cannot be empty.");
        }
    }

    private static int ReadNonNegativeInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (int.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Please enter 0 or a positive whole number.");
        }
    }

    private static bool ReadBool(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (string.Equals(input, "y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine("Please enter 'y' or 'n'.");
        }
    }

    private static (string brand, string model, int year) ReadBaseVehicleProperties()
    {
        var brand = ReadRequiredString("Brand: ");
        var model = ReadRequiredString("Model: ");
        var year = ReadNonNegativeInt("Year: ");

        return (brand, model, year);
    }

    private static void WriteMenuSeparator()
    {
        Console.WriteLine();
        Console.WriteLine(MenuSeparator);
    }

    private static void WriteBaseVehicleProperties(Vehicle vehicle)
    {
        Console.WriteLine($"Type: {vehicle.GetType().Name}");
        Console.WriteLine($"Brand: {vehicle.Brand}");
        Console.WriteLine($"Model: {vehicle.Model}");
        Console.WriteLine($"Year: {vehicle.Year}");
    }

    private static void WriteTypeSpecificVehicleProperties(Vehicle vehicle)
    {
        switch (vehicle)
        {
            case Car car:
                Console.WriteLine($"Number of doors: {car.NumberOfDoors}");
                break;

            case Motorcycle motorcycle:
                Console.Write("Has sidecar: ");
                Console.WriteLine(motorcycle.HasSidecar ? "Yes" : "No");
                break;

            case Truck truck:
                Console.WriteLine($"Cargo capacity: {truck.CargoCapacity} kg");
                break;

            default:
                break;
        }
    }

    private static void WriteVehicleBehaviors(Vehicle vehicle)
    {
        Console.Write("Engine: ");
        vehicle.StartEngine();

        if (vehicle is IDriveable driveable)
        {
            Console.Write("Driving: ");
            driveable.Drive();
        }
    }

    private static void WriteVehicleDetails(Vehicle vehicle)
    {
        WriteBaseVehicleProperties(vehicle);
        WriteTypeSpecificVehicleProperties(vehicle);

        WriteVehicleBehaviors(vehicle);
    }

    private void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== MAIN MENU ===");

        foreach (var (key, option) in _menuOptions)
        {
            Console.WriteLine($"{key}. {option.Title}");
        }

        Console.WriteLine();
        Console.Write("Select an option: ");
    }

    private void AddCar()
    {
        Console.WriteLine("=== ADD CAR ===");
        Console.WriteLine();

        var (brand, model, year) = ReadBaseVehicleProperties();
        var numberOfDoors = ReadNonNegativeInt("Number of doors: ");

        Console.WriteLine();

        try
        {
            _vehicles.Add(new Car(brand, model, year, numberOfDoors));
            Console.WriteLine("Car added successfully.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"The car could not be added: {exception.Message}");
        }
    }

    private void AddMotorcycle()
    {
        Console.WriteLine("=== ADD MOTORCYCLE ===");
        Console.WriteLine();

        var (brand, model, year) = ReadBaseVehicleProperties();
        var hasSidecar = ReadBool("Does it have a sidecar? (y/n): ");

        Console.WriteLine();

        try
        {
            _vehicles.Add(new Motorcycle(brand, model, year, hasSidecar));
            Console.WriteLine("Motorcycle added successfully.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"The motorcycle could not be added: {exception.Message}");
        }
    }

    private void AddTruck()
    {
        Console.WriteLine("=== ADD TRUCK ===");
        Console.WriteLine();

        var (brand, model, year) = ReadBaseVehicleProperties();
        var cargoCapacityKg = ReadNonNegativeInt("Cargo capacity in kilograms: ");

        Console.WriteLine();

        try
        {
            _vehicles.Add(new Truck(brand, model, year, cargoCapacityKg));
            Console.WriteLine("Truck added successfully.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"The truck could not be added: {exception.Message}");
        }
    }

    private void ListVehicles()
    {
        Console.WriteLine("=== VEHICLES ===");

        if (_vehicles.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No vehicles have been added.");
            return;
        }

        for (var index = 0; index < _vehicles.Count; ++index)
        {
            var vehicle = _vehicles[index];

            Console.WriteLine();
            Console.WriteLine($"Vehicle #{index + 1}");

            WriteVehicleDetails(vehicle);
        }
    }

    private void Exit()
    {
        _isRunning = false;
        Console.WriteLine("The application is closing.");
    }

    public void Run()
    {
        _isRunning = true;
        Console.WriteLine("Vehicle Management System");

        while (_isRunning)
        {
            DisplayMenu();
            var selectedKey = ReadUserInput();

            if (string.IsNullOrEmpty(selectedKey))
            {
                Console.WriteLine("Please enter exactly one menu option.");
                WriteMenuSeparator();
                continue;
            }

            if (!_menuOptions.TryGetValue(selectedKey, out var option))
            {
                Console.WriteLine("Invalid menu option.");
                WriteMenuSeparator();
                continue;
            }

            Console.WriteLine();
            option.Execute();
            WriteMenuSeparator();
        }
    }
}

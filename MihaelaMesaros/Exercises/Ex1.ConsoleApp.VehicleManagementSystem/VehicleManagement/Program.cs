using System.Text.Json;
using VehicleManagement.Interfaces;
using VehicleManagement.Models;

List<Vehicle> listVehicles = new();

//initialize list of vehicles
listVehicles.Add(new Car() { Brand = "Toyota", Model = "Yaris Cross", Year = 2025, NumberOfDoors = 4 });
listVehicles.Add(new Car() { Brand = "Honda", Model = "Civic", Year = 2010, NumberOfDoors = 4 });
listVehicles.Add(new Motorcycle() { Brand = "Honda", Model = "Gold Wing", Year = 2009, HasSidecar = false });
listVehicles.Add(new Truck() { Brand = "Iveco", Model = "T-Way", Year = 2024, CargoCapacity = 1000 });
listVehicles.Add(new ElecticCar() { Brand = "Hyundai", Model = "Kona", Year = 2023, NumberOfDoors = 4, BatteryRange = 300});

//iterate list of vehicles for showing polymorfism and inherited interfaces
foreach (var vehicle in listVehicles) {
    Console.WriteLine("\n");

    vehicle.StartEngine(); //shows polymorphism

    //if (vehicle is IDriveable)
    //{
    //	((IDriveable)vehicle).Drive();
    //}
    if (vehicle is IDriveable drivable)
	{
		Console.WriteLine($"{vehicle.GetType().Name} {vehicle.Brand} {vehicle.Model} is IDrivable");
		drivable.Drive();
	}

    if (vehicle is IRefuelable refuelable)
    {
        Console.WriteLine($"{vehicle.GetType().Name} {vehicle.Brand} {vehicle.Model} is IRefuelable");
        refuelable.Refuel();
    }
}


//keep the application running until Exit
while (true) {
	//menu
	Console.WriteLine("\nChoose option:");
    Console.WriteLine("1. Add Car");
    Console.WriteLine("2. Add Motorcycle");
    Console.WriteLine("3. Add Truck");
    Console.WriteLine("4. Add Electric Car");
    Console.WriteLine("5. Save vehicles to JSON");
    Console.WriteLine("6. Load vehicles from JSON");
    Console.WriteLine("8. List Vehicles");
    Console.WriteLine("9. Filter vehicles by type");
    Console.WriteLine("0. Exit");

    string? option = Console.ReadLine();

	if (option != null)
	{
        option = option.Trim();
        switch (option)
		{
			case "1":
                AddVehicle(ReadVehicle(typeof(Car)));
				break;
            case "2":
                AddVehicle(ReadVehicle(typeof(Motorcycle)));
                break;
            case "3":
                AddVehicle(ReadVehicle(typeof(Truck)));
                break;
            case "4":
                AddVehicle(ReadVehicle(typeof(ElecticCar)));
                break;
            case "5":
                SerializeVehicles();
                break;
            case "6":
                DeserializeVehicles();
                break;
            case "8":
                DisplayListVehicles();
                break;
            case "9":
                FilterVehicles();
                break;
            case "0":
                return;
            default:
				Console.WriteLine("Invalid option");
				break;
        }
	}
}


#region Methods implementation

/// <summary>
/// Adds a new vehicle to the collection.
/// </summary>
/// <param name="vehicle">The vehicle to add.</param>
void AddVehicle(Vehicle vehicle)
{
    listVehicles.Add(vehicle);
}

/// <summary>
/// Displays list of vehicles from collection.
/// </summary>
void DisplayListVehicles(Type? vehicleType = null)
{
    foreach (Vehicle vehicle in listVehicles)
    {
        if (vehicleType != null && vehicle.GetType() != vehicleType)
        {
            continue;
        }

        Console.WriteLine($"\n");
        Console.WriteLine($"Type: {vehicle.GetType().Name}");
        Console.WriteLine($"Brand: {vehicle.Brand}");
        Console.WriteLine($"Model: {vehicle.Model}");
        Console.WriteLine($"Year: {vehicle.Year}");

        if (vehicle is Car car)
        {
            Console.WriteLine($"No. of doors: {car.NumberOfDoors}");
        }
        if (vehicle is Motorcycle motorcycle)
        {
            Console.WriteLine($"Has sidecar? {motorcycle.HasSidecar}");
        }
        if (vehicle is Truck truck)
        {
            Console.WriteLine($"Cargo capacity : {truck.CargoCapacity}");
        }
        if (vehicle is ElecticCar electricCar)
        {
            Console.WriteLine($"Battery range : {electricCar.BatteryRange}");
        }
    }
}

/// <summary>
/// Reads the vehicle information from the console.
/// </summary>
/// <param name="type">The type of vehicle to be created.</param>
/// <returns></returns>
Vehicle ReadVehicle(Type type)
{
    string? input;

    Console.WriteLine("Brand: ");
    string brand;
    while (true)
    {
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Brand is required.");
            continue;
        }
        brand = input.Trim();
        break;
    }

    Console.WriteLine("Model: ");
    string model;
    while (true)
    {
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Model is required.");
            continue;
        }
        model = input.Trim();
        break;
    }

    Console.WriteLine("Year: ");
    int year;
    while (true)
    {
        input = Console.ReadLine();
        if (!int.TryParse(input, out year) || year < 1900 || year > 2026)
        {
            Console.WriteLine("Enter valid year (1900-2026).");
            continue;
        }
        break;
    }


    int noDoors = 0;
    if (type == typeof(Car) || type == typeof(ElecticCar))
    {
        Console.WriteLine("Number of doors: ");
        while (true)
        {
            input = Console.ReadLine();
            if (!int.TryParse(input, out noDoors) || noDoors < 0 || noDoors > 10)
            {
                Console.WriteLine("Enter an integer value (0-10).");
                continue;
            }
            break;
        }
    }

    if (type == typeof(Car))
    {
        return new Car()
        {
            Brand = brand,
            Model = model,
            Year = year,
            NumberOfDoors = noDoors
        };
    }
    else if (type == typeof(Motorcycle))
    {
        Console.WriteLine("Has sidecar? (Y/N) ");
        bool hasSidecar;
        while (true)
        {
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Has sidecar is required.");
                continue;
            }
            input = input.Trim();

            if (!input.Equals("y", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("n", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            hasSidecar = input.Equals("Y", StringComparison.OrdinalIgnoreCase);
            break;
        }
         

        return new Motorcycle()
        {
            Brand = brand!,
            Model = model!,
            Year = year,
            HasSidecar = hasSidecar
        };
    }
    else if (type == typeof(Truck))
    {
        Console.WriteLine("Cargo capacity: ");

        decimal cargoCapacity;
        while (!decimal.TryParse(Console.ReadLine(), out cargoCapacity) || cargoCapacity < 0 || cargoCapacity > 1000000)
        {
            Console.WriteLine("Enter a numeric value (0-1000000).");
        }

        return new Truck()
        {
            Brand = brand,
            Model = model,
            Year = year,
            CargoCapacity = cargoCapacity
        };
    }
    else if (type == typeof(ElecticCar))
    {
        Console.WriteLine("Battery range: ");
        int batteryRange;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out batteryRange) || batteryRange < 0 || batteryRange > 10000)
            {
                Console.WriteLine("Enter an integer value (0-10000).");
                continue;
            }
            break;
        }

        return new ElecticCar()
        {
            Brand = brand,
            Model = model,
            Year = year,
            NumberOfDoors = noDoors,
            BatteryRange = batteryRange
        };
    }
    else
        return new Car()
        {
            Brand = brand!,
            Model = model!,
            Year = year
        };
}

/// <summary>
/// Serializes created list of vehicles to json. Supports polymorphic serialization.
/// </summary>
void SerializeVehicles()
{
    string json = JsonSerializer.Serialize(listVehicles);
    Console.WriteLine(json);
}

/// <summary>
/// Deserializes single line json to list of vehicles. Supports polymorphic deserialization.
/// </summary>
void DeserializeVehicles()
{
    Console.WriteLine("Enter JSON single line: ");
    string json;
    while (true)
    {
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Single line JSON is required.");
            continue;
        }
        json = input.Trim();

        if (input.Contains('\n') || input.Contains('\r'))
        {
            Console.WriteLine("Enter single line JSON.");
            continue;
        }
        try
        {
            JsonDocument.Parse(input);
        }
        catch
        {
            Console.WriteLine("String is not valid JSON.");
            continue;
        }
        
        break;
    }

    List<Vehicle>? listVehicles = JsonSerializer.Deserialize<List<Vehicle>>(json);
    DisplayListVehicles();
}

/// <summary>
/// Filters vehicles by specified type in console.
/// </summary>
void FilterVehicles()
{
    foreach (Vehicle vehicle in listVehicles)
    {
        Console.WriteLine($"\nEnter vehicle type:");
        Console.WriteLine("c (Car)");
        Console.WriteLine("m (Motorcycle)");
        Console.WriteLine("t (Truck)");
        Console.WriteLine("e (Electric Car)");
        Console.WriteLine("0 Return to main menu");

        string? option = Console.ReadLine();

        if (option != null)
        {
            option = option.Trim();
            switch (option)
            {
                case "c":
                    DisplayListVehicles(typeof(Car));
                    break;
                case "m":
                    DisplayListVehicles(typeof(Motorcycle));
                    break;
                case "t":
                    DisplayListVehicles(typeof(Truck));
                    break;
                case "e":
                    DisplayListVehicles(typeof(ElecticCar));
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }
}
#endregion
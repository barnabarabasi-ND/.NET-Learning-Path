using Drone_Fleet_Console.Models;
using Drone_Fleet_Console;
using Drone_Fleet_Console.Models.Interfaces;

public class DroneRegistry
{
    private List<Drone> _droneList = new List<Drone>();
    public void ListDrones()
    {
        if (!IsListEmpty())
        {
            Console.WriteLine("Listing drones...");
            foreach (var drone in _droneList)
            {
                Console.WriteLine($"ID: {drone.Id}, Name: {drone.Name}, Type: {drone.Type}, Battery: {drone.BatteryPercent}, Is Airborne:{drone.IsAirborne}");
                if (drone is IPhotoCapture photoCapture)
                {
                    Console.WriteLine($"                                          Photo storage: {photoCapture.PhotoCount} ");
                }
                if (drone is INavigable navigable)
                {
                    Console.WriteLine($"                                          Waypoint: {navigable.CurrentWaypoint} ");
                }
                if (drone is ICargoCarrier cargoCarrier)
                {
                    Console.WriteLine($"                                          Current Load (kg): {cargoCarrier.CurrentLoadKg} ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }

    public void AddDrone(Menu droneMenu)
    {
        int droneChoice = 0;
        while (DroneMenuOptionsEnum.BackToMainMenu != (DroneMenuOptionsEnum)droneChoice)
        {
            droneMenu.DisplayMenu();
            droneChoice = droneMenu.ReadChoice();
            DroneMenuOptionsEnum droneMenuSelected = (DroneMenuOptionsEnum)droneChoice;
            switch (droneMenuSelected)
            {
                case DroneMenuOptionsEnum.SurveyDrone:
                    AddNewDrone(new SurveyDrone());
                    break;

                case DroneMenuOptionsEnum.DeliveryDrone:
                    AddNewDrone(new DeliveryDrone());
                    break;

                case DroneMenuOptionsEnum.RacingDrone:
                    AddNewDrone(new RacingDrone());
                    break;

                case DroneMenuOptionsEnum.BackToMainMenu:
                    Console.WriteLine("Returning to main menu...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void AddNewDrone(Drone drone)
    {
        drone.Initialize(Guid.NewGuid(), ReadDroneName());
        _droneList.Add(drone);
    }

    private string ReadDroneName()
    {
        string? name;

        do
        {
            Console.Write("Enter the drone's name: ");
            name = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(name));

        return name;
    }

    internal void PreFlightCheck()
    {
        if (!IsListEmpty())
        {
            foreach (var drone in _droneList)
            {
                drone.RunSelfTest();
            }
            Console.ReadLine();
        }
    }

    internal void TakeOff()
    {
        if (!IsListEmpty())
        {
            if (SelectDroneById(out Drone? drone))
            {
                Console.WriteLine("Drone TakeOff:");
                drone.TakeOff();
            }
            Console.ReadLine();
        }
    }

    internal void Land()
    {
        if (!IsListEmpty())
        {
            if (SelectDroneById(out Drone? drone))
            {
                Console.WriteLine("Drone Landing:");
                drone.Land();
            }
            Console.ReadLine();
        }
    }

    internal void SetWaypoint()
    {
        if (!IsListEmpty())
        {
            if (SelectDroneById(out Drone? drone))
            {
                if (drone is INavigable navigable)
                {
                    Console.Write("Enter the latitide of WayPoint [-90 , 90]:");
                    var lat = int.TryParse(Console.ReadLine(), out int latitude);
                    if ((!lat) || (latitude < -90) || (latitude > 90))
                    {
                        Console.WriteLine("Invalid range.");
                    }
                    else
                    {
                        Console.Write("Enter the longitude of WayPoint [-180 , 180]:");
                        var lon = int.TryParse(Console.ReadLine(), out int longitude);
                        if ((!lon) || (longitude < -180) || (longitude > 180))
                        {
                            Console.WriteLine("Invalid range.");
                        }
                        else
                        {
                            if ((lat) && (lon))
                            {
                                navigable.SetWaypoint(latitude, longitude);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The drone is not navigable.");
                }
            }
            Console.ReadLine();
        }
    }

    internal void CapabilityActions()
    {
        if (!IsListEmpty())
        {
            if (SelectDroneById(out Drone? drone))
            {
                if (drone is IPhotoCapture photoCapture)
                {
                    photoCapture.TakePhoto();
                }
                else
                if (drone is ICargoCarrier cargoCarrier)
                {
                    Console.Write("Enter the load height in kg:");
                    if (int.TryParse(Console.ReadLine(), out int kg) && (kg > 0))
                    {
                        cargoCarrier.Load(kg);
                    }
                    else
                    {
                        Console.WriteLine("Invalid load.");
                    }
                }
                if (drone.Type == DroneTypeEnum.Racing)
                {
                    Console.WriteLine("This is a racing drone.");
                }
            }
            Console.ReadLine();
        }
    }

    internal void ChargeBattery()
    {
        if (!IsListEmpty())
        {
            if (SelectDroneById(out Drone? drone))
            {
                Console.Write("Enter desired battery level[20 , 100]:");
                var bat = int.TryParse(Console.ReadLine(), out int battery);
                if ((!bat) || (battery < drone.BatteryTakeOffLimit) || (battery > 100))
                {
                    Console.WriteLine("Invalid range.");

                }
                else
                {
                    drone.ChargeBattery(battery);
                }
            }
            Console.ReadLine();
        }
    }

    private bool IsListEmpty()
    {
        if (_droneList.Count == 0)
        {
            Console.WriteLine("No item found.");
            Console.ReadLine();
            return true;
        }
        return false;
    }

    private bool SelectDroneById(out Drone? drone)
    {
        Console.Write("Enter the drone id:");
        if (!Guid.TryParse(Console.ReadLine(), out Guid droneId))
        {
            Console.WriteLine("Drone not found.");
            drone = null;
            return false;
        }

        drone = _droneList.FirstOrDefault(d => d.Id == droneId);

        return drone != null;
    }
}

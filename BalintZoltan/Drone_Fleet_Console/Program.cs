using Drone_Fleet_Console.Models;
using Drone_Fleet_Console;

var mainMenu = new Menu();
var droneMenu = new Menu();
var droneRegistry = new DroneRegistry();
var mainChoice = 0;

mainMenu.AddCaption("=== DRONE FLEET ===");
mainMenu.AddOption("List drones");
mainMenu.AddOption("Add drone");
mainMenu.AddOption("Pre-flight check (all)");
mainMenu.AddOption("Take off");
mainMenu.AddOption("Land");
mainMenu.AddOption("Set waypoint");
mainMenu.AddOption("Capability actions");
mainMenu.AddOption("Charge battery");
mainMenu.AddOption("Exit");

droneMenu.AddCaption("ADD DRONE");
droneMenu.AddOption("Survey Drone");
droneMenu.AddOption("Delivery Drone");
droneMenu.AddOption("Racing Drone");
droneMenu.AddOption("Back to main menu");

while (MainMenuOptionsEnum.Exit != (MainMenuOptionsEnum)mainChoice)
{
    mainMenu.DisplayMenu();
    mainChoice = mainMenu.ReadChoice();
    MainMenuOptionsEnum mainMenuSelected = (MainMenuOptionsEnum)mainChoice;
    switch (mainMenuSelected)
    {
        case MainMenuOptionsEnum.ListDrone:
            droneRegistry.ListDrones();
            break;

        case MainMenuOptionsEnum.AddDrone:
            droneRegistry.AddDrone(droneMenu);
            break;

        case MainMenuOptionsEnum.PreFlightCheck:
            droneRegistry.PreFlightCheck();
            break;

        case MainMenuOptionsEnum.TakeOff:
            droneRegistry.TakeOff();
            break;

        case MainMenuOptionsEnum.Land:
            droneRegistry.Land();
            break;

        case MainMenuOptionsEnum.SetWaypoint:
            droneRegistry.SetWaypoint();
            break;

        case MainMenuOptionsEnum.CapabilityActions:
            droneRegistry.CapabilityActions();
            break;

        case MainMenuOptionsEnum.ChargeBattery:
            droneRegistry.ChargeBattery();
            break;

        case MainMenuOptionsEnum.Exit:
            Console.WriteLine("Exiting...");
            break;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            Console.ReadLine();
            break;
    }
}

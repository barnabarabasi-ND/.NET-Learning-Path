namespace Drone_Fleet_Console;

enum MainMenuOptionsEnum
{
    ListDrone = 1,
    AddDrone,
    PreFlightCheck,
    TakeOff,
    Land,
    SetWaypoint,
    CapabilityActions,
    ChargeBattery,
    Exit
}

enum DroneMenuOptionsEnum
{
    SurveyDrone = 1,
    DeliveryDrone,
    RacingDrone,
    BackToMainMenu
}
public enum DroneTypeEnum
{
    Survey,
    Delivery,
    Racing
}
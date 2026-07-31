namespace Smart_Home_Console_Remote.Services;

using Smart_Home_Console_Remote.Models;
using Smart_Home_Console_Remote.Models.Interfaces;

enum MainMenuOptions
{
    ListDevices = 1,
    AddDevice,
    TogglePower,
    DeviceActions,
    SelfTestAll,
    Exit
}
enum DeviceMenuOptions
{
    LightBulb = 1,
    Thermostat,
    SmartPlug,
    BackToMainMenu
}

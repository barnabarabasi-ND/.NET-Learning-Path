namespace Drone_Fleet_Console.Models.Interfaces;

interface INavigable
{
    (double lat, double lon)? CurrentWaypoint { get; }
    void SetWaypoint(double lat, double lon);
}
namespace Drone_Fleet_Console.Models.Interfaces;

interface ICargoCarrier
{
    double CapacityKg { get; }
    double CurrentLoadKg { get; }
    bool Load(double kg); void UnloadAll();
}

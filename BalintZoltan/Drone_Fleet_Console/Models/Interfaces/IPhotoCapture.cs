namespace Drone_Fleet_Console.Models.Interfaces;

interface IPhotoCapture
{
    int PhotoCount { get; }
    void TakePhoto();
}

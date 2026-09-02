namespace Model.Notifications;

using Interface.INotification;
public class FakeNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Fake: Only for testing: {message}");
    }
}


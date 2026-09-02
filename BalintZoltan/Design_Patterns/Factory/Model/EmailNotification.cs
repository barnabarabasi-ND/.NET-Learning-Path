namespace Model.Notifications;

using Interface.INotification;

public class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Email: {message}");
    }
}


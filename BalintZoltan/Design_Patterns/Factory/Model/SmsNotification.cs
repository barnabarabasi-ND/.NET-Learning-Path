namespace Model.Notifications;

using Interface.INotification;
public class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS: {message}");
    }
}


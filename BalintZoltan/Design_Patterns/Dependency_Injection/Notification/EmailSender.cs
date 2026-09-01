namespace Dependency_Injection.Notification;

using Interface.Sender;
using Modell.Order;
public class EmailSender : INotificationSender
{
    public void Send(Order order, string message)
    {
        Console.WriteLine($"Email sent to {order.CustomerEmail}: {message}");
    }
}

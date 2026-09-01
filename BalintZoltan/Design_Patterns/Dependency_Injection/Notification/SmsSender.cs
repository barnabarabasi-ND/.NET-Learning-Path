namespace Dependency_Injection.Notification;

using Interface.Sender;
using Modell.Order;

public class SmsSender : INotificationSender
{
    public void Send(Order order, string message)
    {
        ArgumentNullException.ThrowIfNull(order);
        Console.WriteLine($"SMS sent to {order.CustomerPhone}: {message}");
    }
}

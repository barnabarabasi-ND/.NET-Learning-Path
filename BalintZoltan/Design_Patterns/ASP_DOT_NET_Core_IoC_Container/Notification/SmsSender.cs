namespace Dependency_Injection.Notification;
using Interface.Sender;
using Model.Order;
public class SmsSender : INotificationSender
{
    public void Send(Order order, string message)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Console.WriteLine($"SMS sent to {order.CustomerPhone}: {message}");
    }
}


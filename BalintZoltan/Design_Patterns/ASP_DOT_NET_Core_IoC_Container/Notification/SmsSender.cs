namespace ASP_DOT_NET_Core_IoC_Container.Notification;
using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;
public class SmsSender : INotificationSender
{
    public void Send(Order order, string message)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Console.WriteLine($"SMS sent to {order.CustomerPhone}: {message}");
    }
}


namespace ASP_DOT_NET_Core_IoC_Container.Notification;
using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;
public class EmailSender : INotificationSender
{
    public void Send(Order order, string message)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        // The following line simulate the EMAIL part.
        Console.WriteLine($"Email sent to {order.CustomerEmail}: {message}");
    }
}


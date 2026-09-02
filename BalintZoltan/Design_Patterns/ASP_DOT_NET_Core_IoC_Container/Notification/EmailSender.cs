namespace Dependency_Injection.Notification;
using Interface.Sender;
using Model.Order;
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


namespace Modell.Service;

using Modell.NotificationFactory;
public class OrderService
{
    private readonly NotificationFactory _factory;
    public OrderService(NotificationFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }
    public void CompleteOrder(string notificationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(notificationType);
        var notification = _factory.Create(notificationType);         // Loose Coupling

            notification.Send("Order completed.");                        // Loose Coupling
    }
}


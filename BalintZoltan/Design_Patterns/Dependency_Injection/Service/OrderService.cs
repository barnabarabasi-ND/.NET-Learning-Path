namespace Dependency_Injection.Service;

using Interface.Sender;
using Interface.Repository;
using Modell.Order;
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly INotificationSender _notification;

    public OrderService(                                            // DI
        IOrderRepository repository,
        INotificationSender notification)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(notification);
        _repository = repository;
        _notification = notification;
    }

    public void PlaceOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _repository.Save(order);
        _notification.Send(order, "Your order has been placed.");
    }
}
namespace ASP_DOT_NET_Core_IoC_Container.Services;
using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly INotificationSender _notification;

    public OrderService(IOrderRepository repository, INotificationSender notification)
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


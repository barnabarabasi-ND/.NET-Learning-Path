namespace ASP_DOT_NET_Core_IoC_Container.Repositories;

using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;
public class FakeOrderRepository : IOrderRepository
{
    public Order? SavedOrder { get; private set; }

    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        SavedOrder = order;
        Console.WriteLine($"Order #{order.Id} saved to FAKE database.");
    }
}


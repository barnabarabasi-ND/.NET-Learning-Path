namespace Dependency_Injection.Repositories;
using Interface.Repository;
using Model.Order;
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


namespace Dependency_Injection.Repositories;

using Interface.Repository;
using Model.Order;
public class SqlOrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        Console.WriteLine($"Order #{order.Id} saved to SQL database.");
    }
}

namespace Dependency_Injection.Repositories;
using Interface.Repository;
using Model.Order;
public class SqlOrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        // The following line simulate the SQL part.
        Console.WriteLine($"Order #{order.Id} saved to SQL database.");
    }
}


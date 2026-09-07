namespace ASP_DOT_NET_Core_IoC_Container.Repositories;
using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;
public class SqlOrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        // The following line simulate the SQL part.
        Console.WriteLine($"Order #{order.Id} saved to SQL database.");
    }
}


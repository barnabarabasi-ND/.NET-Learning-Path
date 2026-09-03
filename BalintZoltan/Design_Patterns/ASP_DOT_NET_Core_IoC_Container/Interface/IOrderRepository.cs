namespace ASP_DOT_NET_Core_IoC_Container.Interfaces;

using ASP_DOT_NET_Core_IoC_Container.Models;
public interface IOrderRepository
{
    void Save(Order order);
}


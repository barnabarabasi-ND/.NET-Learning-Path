namespace Interface.Repository;

using Modell.Order;
public interface IOrderRepository
{
    void Save(Order order);
}

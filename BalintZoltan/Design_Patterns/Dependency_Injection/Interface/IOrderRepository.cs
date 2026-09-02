namespace Interface.Repository;

using Model.Order;
public interface IOrderRepository
{
    void Save(Order order);
}

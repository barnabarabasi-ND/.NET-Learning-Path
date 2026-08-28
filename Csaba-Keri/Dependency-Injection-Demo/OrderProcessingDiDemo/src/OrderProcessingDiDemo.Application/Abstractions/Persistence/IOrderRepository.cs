using OrderProcessingDiDemo.Domain.Orders;

namespace OrderProcessingDiDemo.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

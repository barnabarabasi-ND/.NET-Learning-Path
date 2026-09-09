namespace OrderProcessingDiDemo.Application.Orders;

public interface IOrderService
{
    Task<OrderResult> CreateAsync(CreateOrderCommand command, CancellationToken cancellationToken);

    Task<OrderResult?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

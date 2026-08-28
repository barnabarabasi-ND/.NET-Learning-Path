using Microsoft.EntityFrameworkCore;
using OrderProcessingDiDemo.Application.Abstractions.Persistence;
using OrderProcessingDiDemo.Domain.Orders;

namespace OrderProcessingDiDemo.Infrastructure.Persistence;

public class OrderRepository(OrderDbContext dbContext) : IOrderRepository
{
    private readonly OrderDbContext _dbContext = dbContext;

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Orders
            .Include(order => order.Lines)
            .SingleOrDefaultAsync(order =>
                order.Id == id,
                cancellationToken
            );
    }
}

using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.Application.Abstractions.Persistence;

public interface IProductQueryRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize, int? categoryId, bool? isActive, string? keyword, CancellationToken cancellationToken);

    Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> CheckProductExistsAsync(string productName, int categoryId, int? excludeProductId, CancellationToken cancellationToken);
}

using MiniStoreDemo.Models;

namespace MiniStoreDemo.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);

    Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken);

    Task<int> AddProductAsync(Product product, CancellationToken cancellationToken);

    Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken);

    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken);
}
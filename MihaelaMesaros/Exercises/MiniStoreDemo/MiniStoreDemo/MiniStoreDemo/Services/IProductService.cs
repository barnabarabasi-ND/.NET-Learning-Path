using MiniStoreDemo.DTOs;
using System.Collections.Generic;
using System.Threading;

namespace MiniStoreDemo.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken cancellationToken);

    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken);

    Task<int> AddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);

    Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken);

    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken);
}

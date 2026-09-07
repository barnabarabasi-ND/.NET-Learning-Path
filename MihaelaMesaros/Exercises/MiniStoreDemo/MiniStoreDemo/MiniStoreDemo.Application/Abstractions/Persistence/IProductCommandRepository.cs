using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.Application.Abstractions.Persistence;

public interface IProductCommandRepository
{
    Task<int> AddProductAsync(Product product, CancellationToken cancellationToken);

    Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken);

    Task<bool> PatchProductAsync(Product product, PatchProductDto patchProductDto, CancellationToken cancellationToken);

    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken);
}


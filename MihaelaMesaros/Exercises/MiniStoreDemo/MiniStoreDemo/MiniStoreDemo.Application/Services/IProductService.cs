using MiniStoreDemo.Application.Common;
using MiniStoreDemo.Application.DTOs;
using System.Collections.Generic;
using System.Threading;

namespace MiniStoreDemo.Application.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(int pageNumber, int pageSize, int? categoryId, bool? isActive, string? keyword, CancellationToken cancellationToken);

    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken);


    Task<Result<ProductDto>> AddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);

    Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken);

    Task<Result<ProductDto>> PatchProductAsync(int id, PatchProductDto patchProductDto, CancellationToken cancellationToken);

    Task<Result> DeleteProductAsync(int id, CancellationToken cancellationToken);
}

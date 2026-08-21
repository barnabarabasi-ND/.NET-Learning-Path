using MiniStoreDemo.DTOs;
using MiniStoreDemo.Repositories;
using MiniStoreDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MiniStoreDemo.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProductsAsync(cancellationToken);

        return products.Select(MapProductToDto);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetProductByIdAsync(id, cancellationToken);

        return product is null ? null : MapProductToDto(product);
    }

    public async Task<int> AddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            ProductName = createProductDto.ProductName,
            ProductDescription = createProductDto.ProductDescription,
            ProductPrice = createProductDto.ProductPrice,
            CategoryId = createProductDto.CategoryId,
            IsActive = createProductDto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        return await productRepository.AddProductAsync(product, cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            ProductId = updateProductDto.ProductId,
            ProductName = updateProductDto.ProductName,
            ProductDescription = updateProductDto.ProductDescription,
            ProductPrice = updateProductDto.ProductPrice,
            CategoryId = updateProductDto.CategoryId,
            IsActive = updateProductDto.IsActive,
            ModifiedAt = DateTime.UtcNow
        };
        return await productRepository.UpdateProductAsync(product, cancellationToken);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        return await productRepository.DeleteProductAsync(id, cancellationToken);
    }

    private static ProductDto MapProductToDto(Product product)
    {
        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            ProductDescription = product.ProductDescription,
            ProductPrice = product.ProductPrice,
            CategoryId = product.CategoryId,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}

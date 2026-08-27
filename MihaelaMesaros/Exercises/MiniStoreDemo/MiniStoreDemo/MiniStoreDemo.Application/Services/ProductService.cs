using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Application.Common;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductQueryRepository _queryRepository;
    private readonly IProductCommandRepository _commandRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductQueryRepository queryRepository, IProductCommandRepository commandRepository, ICategoryRepository categoryRepository)
    {
        ArgumentNullException.ThrowIfNull(queryRepository);
        ArgumentNullException.ThrowIfNull(commandRepository);
        ArgumentNullException.ThrowIfNull(categoryRepository);

        _queryRepository = queryRepository;
        _commandRepository = commandRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(int pageNumber, int pageSize, int? categoryId, bool? isActive, string? keyword, CancellationToken cancellationToken)
    {
        var products = await _queryRepository.GetProductsAsync(pageNumber, pageSize, categoryId, isActive, keyword, cancellationToken);

        return products.Select(MapProductToDto);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _queryRepository.GetProductByIdAsync(id, cancellationToken);

        return product is null ? null : MapProductToDto(product);
    }

    public async Task<Result<ProductDto>> AddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        //check if category exists in db
        var categoryExists = await _categoryRepository.CheckCategoryExistsAsync(createProductDto.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result<ProductDto>.Failure(new Error("Category.NotFound", $"Category with ID {createProductDto.CategoryId} does not exist.", ErrorType.Validation));
        }

        //check if exists another product with same name and category
        var productExists = await _queryRepository.CheckProductExistsAsync(createProductDto.ProductName, createProductDto.CategoryId, null, cancellationToken);

        if (productExists)
        {
            return Result<ProductDto>.Failure(new Error("Product.AlreadyExists", $"Product '{createProductDto.ProductName}' already exists in this category.", ErrorType.Conflict));
        }

        var product = new Product
        {
            ProductName = createProductDto.ProductName,
            ProductDescription = createProductDto.ProductDescription,
            ProductPrice = createProductDto.ProductPrice,
            CategoryId = createProductDto.CategoryId,
            IsActive = createProductDto.IsActive
        };

        var createdProductId = await _commandRepository.AddProductAsync(product, cancellationToken);

        if (createdProductId <= 0)
        {
            throw new InvalidOperationException("Product creation did not return a valid product ID.");
        }

        var createdProduct = await _queryRepository.GetProductByIdAsync(createdProductId, cancellationToken);

        return createdProduct is null
            ? throw new InvalidOperationException($"Product {createdProductId} was created but could not be retrieved.")
            : Result<ProductDto>.Success(MapProductToDto(createdProduct));
    }

    public async Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var existingProduct = await _queryRepository.GetProductByIdAsync(id, cancellationToken);

        if (existingProduct is null)
        {
            return Result<ProductDto>.Failure(new Error("Product.NotFound", $"Product with ID {id} does not exist.", ErrorType.NotFound));
        }

        var categoryExists = await _categoryRepository.CheckCategoryExistsAsync(updateProductDto.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result<ProductDto>.Failure(new Error("Category.NotFound", $"Category with ID {updateProductDto.CategoryId} does not exist.", ErrorType.Validation));
        }

        var productExists = await _queryRepository.CheckProductExistsAsync(updateProductDto.ProductName, updateProductDto.CategoryId, id, cancellationToken);

        if (productExists)
        {
            return Result<ProductDto>.Failure(new Error("Product.AlreadyExists", $"Product '{updateProductDto.ProductName}' already exists in this category.", ErrorType.Conflict));
        }

        existingProduct.ProductName = updateProductDto.ProductName;
        existingProduct.ProductDescription = updateProductDto.ProductDescription;
        existingProduct.ProductPrice = updateProductDto.ProductPrice;
        existingProduct.CategoryId = updateProductDto.CategoryId;
        existingProduct.IsActive = updateProductDto.IsActive;
        existingProduct.ModifiedAt = DateTime.UtcNow;

        var updated = await _commandRepository.UpdateProductAsync(existingProduct, cancellationToken);

        if (!updated)
        {
            throw new InvalidOperationException($"Product {id} could not be updated.");
        }

        var updatedProduct = await _queryRepository.GetProductByIdAsync(id, cancellationToken);

        return updatedProduct is null
            ? throw new InvalidOperationException($"Product {id} was updated but could not be retrieved.")
            : Result<ProductDto>.Success(MapProductToDto(updatedProduct));
    }

    public async Task<Result<ProductDto>> PatchProductAsync(int id, PatchProductDto patchProductDto, CancellationToken cancellationToken)
    {
        var existingProduct = await _queryRepository.GetProductByIdAsync(id, cancellationToken);

        if (existingProduct is null)
        {
            return Result<ProductDto>.Failure(new Error("Product.NotFound", $"Product with ID {id} does not exist.", ErrorType.NotFound));
        }

        var productName = patchProductDto.ProductName ?? existingProduct.ProductName;

        var categoryId = patchProductDto.CategoryId ?? existingProduct.CategoryId;

        if (patchProductDto.CategoryId.HasValue)
        {
            var categoryExists = await _categoryRepository.CheckCategoryExistsAsync(patchProductDto.CategoryId.Value, cancellationToken);

            if (!categoryExists)
            {
                return Result<ProductDto>.Failure(new Error("Category.NotFound", $"Category with ID {patchProductDto.CategoryId} does not exist.", ErrorType.Validation));
            }
        }

        if (patchProductDto.ProductName is not null || patchProductDto.CategoryId.HasValue)
        {
            var productExists = await _queryRepository.CheckProductExistsAsync(productName, categoryId, id, cancellationToken);

            if (productExists)
            {
                return Result<ProductDto>.Failure(new Error("Product.AlreadyExists", $"Product '{productName}' already exists in this category.", ErrorType.Conflict));
            }
        }


        if (patchProductDto.ProductName is not null)
            existingProduct.ProductName = patchProductDto.ProductName;

        if (patchProductDto.ProductDescription is not null)
            existingProduct.ProductDescription = patchProductDto.ProductDescription;

        if (patchProductDto.ProductPrice.HasValue)
            existingProduct.ProductPrice = patchProductDto.ProductPrice.Value;

        if (patchProductDto.CategoryId.HasValue)
            existingProduct.CategoryId = patchProductDto.CategoryId.Value;

        if (patchProductDto.IsActive.HasValue)
            existingProduct.IsActive = patchProductDto.IsActive.Value;
        
        existingProduct.ModifiedAt = DateTime.UtcNow;

        var updated = await _commandRepository.PatchProductAsync(existingProduct, patchProductDto, cancellationToken);

        if (!updated)
        {
            throw new InvalidOperationException($"Product {id} could not be updated.");
        }

        var updatedProduct = await _queryRepository.GetProductByIdAsync(id, cancellationToken);

        return updatedProduct is null
            ? throw new InvalidOperationException($"Product {id} was updated but could not be retrieved.")
            : Result<ProductDto>.Success(MapProductToDto(updatedProduct));
    }

    public async Task<Result> DeleteProductAsync(
    int id,
    CancellationToken cancellationToken)
    {
        var deleted = await _commandRepository.DeleteProductAsync(id, cancellationToken);

        if (!deleted)
        {
            return Result.Failure(new Error("Product.NotFound", $"Product with ID {id} does not exist.", ErrorType.NotFound));
        }

        return Result.Success();
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
            CreatedAt = product.CreatedAt,
            ModifiedAt = product.ModifiedAt
        };
    }
}

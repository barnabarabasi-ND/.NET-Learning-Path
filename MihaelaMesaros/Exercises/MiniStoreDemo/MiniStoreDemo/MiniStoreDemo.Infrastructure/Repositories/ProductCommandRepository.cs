using Microsoft.EntityFrameworkCore;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Domain.Entities;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Infrastructure.Persistence;

namespace MiniStoreDemo.Infrastructure.Repositories;

public sealed class ProductCommandRepository : MiniStoreDemo.Application.Abstractions.Persistence.IProductCommandRepository
{
    private readonly AppDbContext _dbContext;

    public ProductCommandRepository(AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<int> AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return product.ProductId;
    }

    public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Attach(product);

        var entry = _dbContext.Entry(product);

        entry.Property(p => p.ProductName).IsModified = true;
        entry.Property(p => p.ProductDescription).IsModified = true;
        entry.Property(p => p.ProductPrice).IsModified = true;
        entry.Property(p => p.CategoryId).IsModified = true;
        entry.Property(p => p.IsActive).IsModified = true;
        entry.Property(p => p.ModifiedAt).IsModified = true;

        var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);

        return affectedRows > 0;
    }

    public async Task<bool> PatchProductAsync(Product product, PatchProductDto patchProductDto, CancellationToken cancellationToken)
    {
        _dbContext.Products.Attach(product);

        var entry = _dbContext.Entry(product);

        if (patchProductDto.ProductName is not null)
            entry.Property(p => p.ProductName).IsModified = true;

        if (patchProductDto.ProductDescription is not null)
            entry.Property(p => p.ProductDescription).IsModified = true;

        if (patchProductDto.ProductPrice.HasValue)
            entry.Property(p => p.ProductPrice).IsModified = true;

        if (patchProductDto.CategoryId.HasValue)
            entry.Property(p => p.CategoryId).IsModified = true;

        if (patchProductDto.IsActive.HasValue)
            entry.Property(p => p.IsActive).IsModified = true;

        entry.Property(p => p.ModifiedAt).IsModified = true;

        var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);

        if (product is null)
            return false;

        _dbContext.Products.Remove(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}

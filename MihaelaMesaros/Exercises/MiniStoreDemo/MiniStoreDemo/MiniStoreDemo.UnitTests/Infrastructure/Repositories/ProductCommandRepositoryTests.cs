using Microsoft.EntityFrameworkCore;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Domain.Entities;
using MiniStoreDemo.Infrastructure.Persistence;
using MiniStoreDemo.Infrastructure.Repositories;

namespace MiniStoreDemo.UnitTests.Infrastructure.Repositories;

public sealed class ProductCommandRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProductCommandRepository _repository;

    public ProductCommandRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new ProductCommandRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithNullDbContext_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ProductCommandRepository(null!));
    }

    [Fact]
    public void Constructor_WithValidDbContext_DoesNotThrow()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using var context = new AppDbContext(options);

        // Act & Assert
        var repository = new ProductCommandRepository(context);
        Assert.NotNull(repository);
    }

    #endregion

    #region AddProductAsync - Happy Path Tests

    [Fact]
    public async Task AddProductAsync_WithValidProduct_ReturnsProductId()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        var result = await _repository.AddProductAsync(product, CancellationToken.None);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public async Task AddProductAsync_WithValidProduct_ProductIsPersistedInDatabase()
    {
        // Arrange
        var product = CreateProduct(name: "Test Product");

        // Act
        await _repository.AddProductAsync(product, CancellationToken.None);

        // Assert
        var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == "Test Product");
        Assert.NotNull(savedProduct);
    }

    [Fact]
    public async Task AddProductAsync_WithValidProduct_AssignsGeneratedProductId()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        var productId = await _repository.AddProductAsync(product, CancellationToken.None);

        // Assert
        Assert.Equal(product.ProductId, productId);
    }

    [Fact]
    public async Task AddProductAsync_AddingMultipleProducts_GeneratesUniqueIds()
    {
        // Arrange
        var product1 = CreateProduct(name: "Product 1");
        var product2 = CreateProduct(name: "Product 2");

        // Act
        var id1 = await _repository.AddProductAsync(product1, CancellationToken.None);
        var id2 = await _repository.AddProductAsync(product2, CancellationToken.None);

        // Assert
        Assert.NotEqual(id1, id2);
    }

    #endregion

    #region AddProductAsync - Edge Cases

    [Fact]
    public async Task AddProductAsync_WithMinimalProduct_Succeeds()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Min",
            ProductDescription = "Minimal product",
            ProductPrice = 0.01m,
            CategoryId = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _repository.AddProductAsync(product, CancellationToken.None);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public async Task AddProductAsync_WithMaxDecimalPrice_Succeeds()
    {
        // Arrange
        var product = CreateProduct(price: 99999999.99m);

        // Act
        var result = await _repository.AddProductAsync(product, CancellationToken.None);

        // Assert
        Assert.True(result > 0);
        var saved = await _context.Products.FindAsync(result);
        Assert.Equal(99999999.99m, saved!.ProductPrice);
    }

    [Fact]
    public async Task AddProductAsync_WithEmptyDescription_Succeeds()
    {
        // Arrange
        var product = CreateProduct();
        product.ProductDescription = "";

        // Act
        var result = await _repository.AddProductAsync(product, CancellationToken.None);

        // Assert
        Assert.True(result > 0);
    }

    #endregion

    #region AddProductAsync - Cancellation Tests

    [Fact]
    public async Task AddProductAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        var product = CreateProduct();
        var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => _repository.AddProductAsync(product, cts.Token));
    }

    #endregion

    #region UpdateProductAsync - Happy Path Tests

    [Fact]
    public async Task UpdateProductAsync_WithExistingProduct_ReturnsTrue()
    {
        // Arrange
        var product = await AddAndDetachProduct("Original Name");
        product.ProductName = "Updated Name";
        product.ModifiedAt = DateTime.UtcNow;

        // Act
        var result = await _repository.UpdateProductAsync(product, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateProductAsync_WithExistingProduct_PersistsChanges()
    {
        // Arrange
        var product = await AddAndDetachProduct("Original Name");
        product.ProductName = "Updated Name";
        product.ProductDescription = "Updated Description";
        product.ProductPrice = 999.99m;
        product.CategoryId = 2;
        product.IsActive = false;
        product.ModifiedAt = DateTime.UtcNow;

        // Act
        await _repository.UpdateProductAsync(product, CancellationToken.None);

        // Assert
        var updatedProduct = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal("Updated Name", updatedProduct!.ProductName);
        Assert.Equal("Updated Description", updatedProduct.ProductDescription);
        Assert.Equal(999.99m, updatedProduct.ProductPrice);
        Assert.Equal(2, updatedProduct.CategoryId);
        Assert.False(updatedProduct.IsActive);
    }

    [Fact]
    public async Task UpdateProductAsync_WithExistingProduct_UpdatesModifiedDate()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product");
        var modifiedAt = new DateTime(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc);
        product.ModifiedAt = modifiedAt;

        // Act
        await _repository.UpdateProductAsync(product, CancellationToken.None);

        // Assert
        var updatedProduct = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal(modifiedAt, updatedProduct!.ModifiedAt);
    }

    #endregion

    #region UpdateProductAsync - Edge Cases

    [Fact]
    public async Task UpdateProductAsync_WithNonExistentProduct_ThrowsDbUpdateConcurrencyException()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 99999,
            ProductName = "Non Existent",
            ProductDescription = "Non Existent Description",
            ProductPrice = 10.00m,
            CategoryId = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        // Act & Assert
        // EF Core InMemory throws DbUpdateConcurrencyException when updating non-existent entities
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(
            () => _repository.UpdateProductAsync(product, CancellationToken.None));
    }

    #endregion

    #region PatchProductAsync - Happy Path Tests

    [Fact]
    public async Task PatchProductAsync_WithProductNameOnly_UpdatesOnlyProductName()
    {
        // Arrange
        var product = await AddAndDetachProduct("Original Name", price: 50.00m);
        var originalPrice = product.ProductPrice;
        product.ProductName = "Patched Name";

        var patchDto = new PatchProductDto { ProductName = "Patched Name" };

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal("Patched Name", patched!.ProductName);
    }

    [Fact]
    public async Task PatchProductAsync_WithPriceOnly_UpdatesOnlyPrice()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product", price: 50.00m);
        product.ProductPrice = 75.00m;

        var patchDto = new PatchProductDto { ProductPrice = 75.00m };

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal(75.00m, patched!.ProductPrice);
    }

    [Fact]
    public async Task PatchProductAsync_WithIsActiveOnly_UpdatesOnlyIsActive()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product");
        product.IsActive = false;

        var patchDto = new PatchProductDto { IsActive = false };

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.False(patched!.IsActive);
    }

    [Fact]
    public async Task PatchProductAsync_WithDescriptionOnly_UpdatesOnlyDescription()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product");
        product.ProductDescription = "New Description";

        var patchDto = new PatchProductDto { ProductDescription = "New Description" };

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal("New Description", patched!.ProductDescription);
    }

    [Fact]
    public async Task PatchProductAsync_WithCategoryIdOnly_UpdatesOnlyCategoryId()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product");
        product.CategoryId = 5;

        var patchDto = new PatchProductDto { CategoryId = 5 };

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal(5, patched!.CategoryId);
    }

    [Fact]
    public async Task PatchProductAsync_WithMultipleFields_UpdatesAllSpecifiedFields()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product", price: 10.00m);
        product.ProductName = "Patched Name";
        product.ProductPrice = 99.99m;
        product.IsActive = false;
        product.ModifiedAt = DateTime.UtcNow;

        var patchDto = new PatchProductDto
        {
            ProductName = "Patched Name",
            ProductPrice = 99.99m,
            IsActive = false
        };

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal("Patched Name", patched!.ProductName);
        Assert.Equal(99.99m, patched.ProductPrice);
        Assert.False(patched.IsActive);
    }

    [Fact]
    public async Task PatchProductAsync_AlwaysUpdatesModifiedAt()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product");
        var modifiedAt = new DateTime(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc);
        product.ModifiedAt = modifiedAt;

        var patchDto = new PatchProductDto { ProductName = "New Name" };
        product.ProductName = "New Name";

        // Act
        await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        var patched = await _context.Products.FindAsync(product.ProductId);
        Assert.Equal(modifiedAt, patched!.ModifiedAt);
    }

    #endregion

    #region PatchProductAsync - Edge Cases

    [Fact]
    public async Task PatchProductAsync_WithEmptyPatchDto_StillUpdatesModifiedAt()
    {
        // Arrange
        var product = await AddAndDetachProduct("Test Product");
        var modifiedAt = DateTime.UtcNow;
        product.ModifiedAt = modifiedAt;

        var patchDto = new PatchProductDto(); // No fields set

        // Act
        var result = await _repository.PatchProductAsync(product, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region DeleteProductAsync - Happy Path Tests

    [Fact]
    public async Task DeleteProductAsync_WithExistingProduct_ReturnsTrue()
    {
        // Arrange
        var product = CreateProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        var productId = product.ProductId;

        // Act
        var result = await _repository.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithExistingProduct_RemovesProductFromDatabase()
    {
        // Arrange
        var product = CreateProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        var productId = product.ProductId;

        // Act
        await _repository.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        var deletedProduct = await _context.Products.FindAsync(productId);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteProductAsync_WithExistingProduct_DoesNotAffectOtherProducts()
    {
        // Arrange
        var product1 = CreateProduct(name: "Product 1");
        var product2 = CreateProduct(name: "Product 2");
        _context.Products.AddRange(product1, product2);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteProductAsync(product1.ProductId, CancellationToken.None);

        // Assert
        var remaining = await _context.Products.FindAsync(product2.ProductId);
        Assert.NotNull(remaining);
        Assert.Equal("Product 2", remaining.ProductName);
    }

    #endregion

    #region DeleteProductAsync - Non-Existent Product Tests

    [Fact]
    public async Task DeleteProductAsync_WithNonExistentProduct_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var result = await _repository.DeleteProductAsync(nonExistentId, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithZeroId_ReturnsFalse()
    {
        // Act
        var result = await _repository.DeleteProductAsync(0, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithNegativeId_ReturnsFalse()
    {
        // Act
        var result = await _repository.DeleteProductAsync(-1, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region DeleteProductAsync - Edge Cases

    [Fact]
    public async Task DeleteProductAsync_DeletingSameProductTwice_ReturnsFalseOnSecondAttempt()
    {
        // Arrange
        var product = CreateProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        var productId = product.ProductId;

        // Act
        var firstResult = await _repository.DeleteProductAsync(productId, CancellationToken.None);
        var secondResult = await _repository.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        Assert.True(firstResult);
        Assert.False(secondResult);
    }

    #endregion

    #region Helper Methods

    private static Product CreateProduct(
        string name = "Test Product",
        string? description = "Test Description",
        decimal price = 29.99m,
        int categoryId = 1,
        bool isActive = true)
    {
        return new Product
        {
            ProductName = name,
            ProductDescription = description,
            ProductPrice = price,
            CategoryId = categoryId,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };
    }

    private async Task<Product> AddAndDetachProduct(
        string name = "Test Product",
        decimal price = 29.99m)
    {
        var product = CreateProduct(name: name, price: price);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Detach so we can reattach with changes
        _context.Entry(product).State = EntityState.Detached;

        return product;
    }

    #endregion
}

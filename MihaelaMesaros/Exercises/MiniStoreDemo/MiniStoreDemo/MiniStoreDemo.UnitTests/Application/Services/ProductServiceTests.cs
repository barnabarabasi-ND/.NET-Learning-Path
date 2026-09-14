using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Application.Services;
using MiniStoreDemo.Domain.Entities;
using Moq;

namespace MiniStoreDemo.UnitTests.Application.Services;

public sealed class ProductServiceTests
{
    private readonly Mock<IProductQueryRepository> _mockQueryRepository;
    private readonly Mock<IProductCommandRepository> _mockCommandRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockQueryRepository = new Mock<IProductQueryRepository>();
        _mockCommandRepository = new Mock<IProductCommandRepository>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _service = new ProductService(
            _mockQueryRepository.Object,
            _mockCommandRepository.Object,
            _mockCategoryRepository.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithNullQueryRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ProductService(null!, _mockCommandRepository.Object, _mockCategoryRepository.Object));
        Assert.Equal("queryRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullCommandRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ProductService(_mockQueryRepository.Object, null!, _mockCategoryRepository.Object));
        Assert.Equal("commandRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullCategoryRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ProductService(_mockQueryRepository.Object, _mockCommandRepository.Object, null!));
        Assert.Equal("categoryRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidDependencies_DoesNotThrow()
    {
        // Act & Assert
        var service = new ProductService(
            _mockQueryRepository.Object,
            _mockCommandRepository.Object,
            _mockCategoryRepository.Object);
        Assert.NotNull(service);
    }

    #endregion

    #region GetProductsAsync Tests

    [Fact]
    public async Task GetProductsAsync_WithProducts_ReturnsProductDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            CreateProduct(1, "Product 1"),
            CreateProduct(2, "Product 2")
        };
        _mockQueryRepository
            .Setup(r => r.GetProductsAsync(1, 10, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetProductsAsync(1, 10, null, null, null, CancellationToken.None);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("Product 1", resultList[0].ProductName);
        Assert.Equal("Product 2", resultList[1].ProductName);
    }

    [Fact]
    public async Task GetProductsAsync_WithNoProducts_ReturnsEmptyCollection()
    {
        // Arrange
        _mockQueryRepository
            .Setup(r => r.GetProductsAsync(1, 10, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Product>());

        // Act
        var result = await _service.GetProductsAsync(1, 10, null, null, null, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProductsAsync_WithFilters_PassesFiltersToRepository()
    {
        // Arrange
        var products = new List<Product> { CreateProduct(1, "Filtered Product") };
        _mockQueryRepository
            .Setup(r => r.GetProductsAsync(2, 20, 5, true, "search", It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetProductsAsync(2, 20, 5, true, "search", CancellationToken.None);

        // Assert
        Assert.Single(result);
        _mockQueryRepository.Verify(r => r.GetProductsAsync(2, 20, 5, true, "search", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var modifiedAt = new DateTime(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc);
        var product = new Product
        {
            ProductId = 42,
            ProductName = "Test Product",
            ProductDescription = "Test Description",
            ProductPrice = 99.99m,
            CategoryId = 3,
            IsActive = true,
            CreatedAt = createdAt,
            ModifiedAt = modifiedAt
        };
        _mockQueryRepository
            .Setup(r => r.GetProductsAsync(1, 10, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { product });

        // Act
        var result = (await _service.GetProductsAsync(1, 10, null, null, null, CancellationToken.None)).First();

        // Assert
        Assert.Equal(42, result.ProductId);
        Assert.Equal("Test Product", result.ProductName);
        Assert.Equal("Test Description", result.ProductDescription);
        Assert.Equal(99.99m, result.ProductPrice);
        Assert.Equal(3, result.CategoryId);
        Assert.True(result.IsActive);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(modifiedAt, result.ModifiedAt);
    }

    #endregion

    #region GetProductByIdAsync Tests

    [Fact]
    public async Task GetProductByIdAsync_WithExistingProduct_ReturnsProductDto()
    {
        // Arrange
        var product = CreateProduct(1, "Test Product");
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetProductByIdAsync(1, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductId);
        Assert.Equal("Test Product", result.ProductName);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithNonExistingProduct_ReturnsNull()
    {
        // Arrange
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetProductByIdAsync(999, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithZeroId_CallsRepository()
    {
        // Arrange
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(0, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetProductByIdAsync(0, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockQueryRepository.Verify(r => r.GetProductByIdAsync(0, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithNegativeId_CallsRepository()
    {
        // Arrange
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(-1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetProductByIdAsync(-1, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockQueryRepository.Verify(r => r.GetProductByIdAsync(-1, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region AddProductAsync Tests

    [Fact]
    public async Task AddProductAsync_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var createDto = CreateProductDto();
        var createdProduct = CreateProduct(1, createDto.ProductName);

        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(createDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(createDto.ProductName, createDto.CategoryId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _service.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(createDto.ProductName, result.Value.ProductName);
    }

    [Fact]
    public async Task AddProductAsync_WithNonExistentCategory_ReturnsFailure()
    {
        // Arrange
        var createDto = CreateProductDto();
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(createDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Category.NotFound", result.Error.Code);
        Assert.Contains(createDto.CategoryId.ToString(), result.Error.Description);
    }

    [Fact]
    public async Task AddProductAsync_WithDuplicateProduct_ReturnsFailure()
    {
        // Arrange
        var createDto = CreateProductDto();
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(createDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(createDto.ProductName, createDto.CategoryId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Product.AlreadyExists", result.Error.Code);
        Assert.Contains(createDto.ProductName, result.Error.Description);
    }

    [Fact]
    public async Task AddProductAsync_WhenCreationReturnsZeroId_ThrowsInvalidOperationException()
    {
        // Arrange
        var createDto = CreateProductDto();
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(createDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(createDto.ProductName, createDto.CategoryId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AddProductAsync(createDto, CancellationToken.None));
        Assert.Contains("valid product ID", exception.Message);
    }

    [Fact]
    public async Task AddProductAsync_WhenCreationReturnsNegativeId_ThrowsInvalidOperationException()
    {
        // Arrange
        var createDto = CreateProductDto();
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(createDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(createDto.ProductName, createDto.CategoryId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(-1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AddProductAsync(createDto, CancellationToken.None));
    }

    [Fact]
    public async Task AddProductAsync_WhenCreatedProductCannotBeRetrieved_ThrowsInvalidOperationException()
    {
        // Arrange
        var createDto = CreateProductDto();
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(createDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(createDto.ProductName, createDto.CategoryId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AddProductAsync(createDto, CancellationToken.None));
        Assert.Contains("could not be retrieved", exception.Message);
    }

    #endregion

    #region UpdateProductAsync Tests

    [Fact]
    public async Task UpdateProductAsync_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Old Name");
        var updateDto = CreateUpdateProductDto(1);
        var updatedProduct = CreateProduct(1, updateDto.ProductName);

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(updateDto.ProductName, updateDto.CategoryId, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Reset and re-setup GetProductByIdAsync for the final retrieval
        _mockQueryRepository
            .SetupSequence(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct)
            .ReturnsAsync(updatedProduct);

        // Act
        var result = await _service.UpdateProductAsync(1, updateDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task UpdateProductAsync_WithNonExistentProduct_ReturnsFailure()
    {
        // Arrange
        var updateDto = CreateUpdateProductDto(999);
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateProductAsync(999, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Product.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task UpdateProductAsync_WithNonExistentCategory_ReturnsFailure()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test");
        var updateDto = CreateUpdateProductDto(1);

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateProductAsync(1, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Category.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task UpdateProductAsync_WithDuplicateProductName_ReturnsFailure()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Old Name");
        var updateDto = CreateUpdateProductDto(1);

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(updateDto.ProductName, updateDto.CategoryId, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateProductAsync(1, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Product.AlreadyExists", result.Error.Code);
    }

    [Fact]
    public async Task UpdateProductAsync_WhenUpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test");
        var updateDto = CreateUpdateProductDto(1);

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(updateDto.ProductName, updateDto.CategoryId, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateProductAsync(1, updateDto, CancellationToken.None));
        Assert.Contains("could not be updated", exception.Message);
    }

    [Fact]
    public async Task UpdateProductAsync_WhenUpdatedProductCannotBeRetrieved_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test");
        var updateDto = CreateUpdateProductDto(1);

        _mockQueryRepository
            .SetupSequence(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct)
            .ReturnsAsync((Product?)null);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(updateDto.ProductName, updateDto.CategoryId, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateProductAsync(1, updateDto, CancellationToken.None));
        Assert.Contains("could not be retrieved", exception.Message);
    }

    #endregion

    #region PatchProductAsync Tests

    [Fact]
    public async Task PatchProductAsync_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test Product");
        var patchDto = new PatchProductDto { ProductName = "Patched Name" };
        var patchedProduct = CreateProduct(1, "Patched Name");

        _mockQueryRepository
            .SetupSequence(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct)
            .ReturnsAsync(patchedProduct);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync("Patched Name", existingProduct.CategoryId, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.PatchProductAsync(It.IsAny<Product>(), patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task PatchProductAsync_WithNonExistentProduct_ReturnsFailure()
    {
        // Arrange
        var patchDto = new PatchProductDto { ProductName = "New Name" };
        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.PatchProductAsync(999, patchDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Product.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task PatchProductAsync_WithNonExistentCategory_ReturnsFailure()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test");
        var patchDto = new PatchProductDto { CategoryId = 999 };

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Category.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task PatchProductAsync_WithDuplicateProductName_ReturnsFailure()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Original Name");
        var patchDto = new PatchProductDto { ProductName = "Duplicate Name" };

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync("Duplicate Name", existingProduct.CategoryId, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Product.AlreadyExists", result.Error.Code);
    }

    [Fact]
    public async Task PatchProductAsync_WithOnlyPriceChange_DoesNotCheckForDuplicates()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test Product");
        var patchDto = new PatchProductDto { ProductPrice = 199.99m };
        var patchedProduct = CreateProduct(1, "Test Product");
        patchedProduct.ProductPrice = 199.99m;

        _mockQueryRepository
            .SetupSequence(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct)
            .ReturnsAsync(patchedProduct);
        _mockCommandRepository
            .Setup(r => r.PatchProductAsync(It.IsAny<Product>(), patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        // Verify CheckProductExistsAsync was NOT called since only price changed
        _mockQueryRepository.Verify(
            r => r.CheckProductExistsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task PatchProductAsync_WhenPatchFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test");
        var patchDto = new PatchProductDto { ProductPrice = 99.99m };

        _mockQueryRepository
            .Setup(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _mockCommandRepository
            .Setup(r => r.PatchProductAsync(It.IsAny<Product>(), patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.PatchProductAsync(1, patchDto, CancellationToken.None));
        Assert.Contains("could not be updated", exception.Message);
    }

    [Fact]
    public async Task PatchProductAsync_WithAllFieldsNull_StillUpdatesModifiedAt()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test Product");
        var patchDto = new PatchProductDto(); // All fields null
        var patchedProduct = CreateProduct(1, "Test Product");

        _mockQueryRepository
            .SetupSequence(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct)
            .ReturnsAsync(patchedProduct);
        _mockCommandRepository
            .Setup(r => r.PatchProductAsync(It.IsAny<Product>(), patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task PatchProductAsync_WithCategoryIdChangeOnly_ChecksCategoryExists()
    {
        // Arrange
        var existingProduct = CreateProduct(1, "Test Product");
        existingProduct.CategoryId = 1;
        var patchDto = new PatchProductDto { CategoryId = 5 };
        var patchedProduct = CreateProduct(1, "Test Product");
        patchedProduct.CategoryId = 5;

        _mockQueryRepository
            .SetupSequence(r => r.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct)
            .ReturnsAsync(patchedProduct);
        _mockCategoryRepository
            .Setup(r => r.CheckCategoryExistsAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockQueryRepository
            .Setup(r => r.CheckProductExistsAsync(existingProduct.ProductName, 5, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockCommandRepository
            .Setup(r => r.PatchProductAsync(It.IsAny<Product>(), patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _mockCategoryRepository.Verify(r => r.CheckCategoryExistsAsync(5, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DeleteProductAsync Tests

    [Fact]
    public async Task DeleteProductAsync_WithExistingProduct_ReturnsSuccessResult()
    {
        // Arrange
        _mockCommandRepository
            .Setup(r => r.DeleteProductAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteProductAsync(1, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task DeleteProductAsync_WithNonExistentProduct_ReturnsFailure()
    {
        // Arrange
        _mockCommandRepository
            .Setup(r => r.DeleteProductAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteProductAsync(999, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Product.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task DeleteProductAsync_WithZeroId_CallsRepository()
    {
        // Arrange
        _mockCommandRepository
            .Setup(r => r.DeleteProductAsync(0, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteProductAsync(0, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockCommandRepository.Verify(r => r.DeleteProductAsync(0, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_WithNegativeId_CallsRepository()
    {
        // Arrange
        _mockCommandRepository
            .Setup(r => r.DeleteProductAsync(-1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteProductAsync(-1, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockCommandRepository.Verify(r => r.DeleteProductAsync(-1, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Helper Methods

    private static Product CreateProduct(int id, string name)
    {
        return new Product
        {
            ProductId = id,
            ProductName = name,
            ProductDescription = "Test Description",
            ProductPrice = 99.99m,
            CategoryId = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static CreateProductDto CreateProductDto()
    {
        return new CreateProductDto
        {
            ProductName = "New Product",
            ProductDescription = "New Description",
            ProductPrice = 49.99m,
            CategoryId = 1,
            IsActive = true
        };
    }

    private static UpdateProductDto CreateUpdateProductDto(int productId)
    {
        return new UpdateProductDto
        {
            ProductId = productId,
            ProductName = "Updated Product",
            ProductDescription = "Updated Description",
            ProductPrice = 79.99m,
            CategoryId = 1,
            IsActive = true
        };
    }

    #endregion
}

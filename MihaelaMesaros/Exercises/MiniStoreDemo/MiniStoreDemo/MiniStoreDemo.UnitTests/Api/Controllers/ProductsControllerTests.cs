using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Api.Controllers;
using MiniStoreDemo.Application.Common;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Application.Services;
using Moq;

namespace MiniStoreDemo.UnitTests.Api.Controllers;

public sealed class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    #region GetProductsAsync - Success Cases

    [Fact]
    public async Task GetProductsAsync_WithDefaultParameters_ReturnsOkResult()
    {
        // Arrange
        var queryParams = new ProductQueryParameters();
        var products = new List<ProductDto>
        {
            new() { ProductId = 1, ProductName = "Product 1", ProductPrice = 10.00m }
        };
        _mockProductService.Setup(s => s.GetProductsAsync(
            queryParams.PageNumber, queryParams.PageSize, queryParams.CategoryId,
            queryParams.IsActive, queryParams.Keyword, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task GetProductsAsync_WithEmptyResult_ReturnsOkWithEmptyCollection()
    {
        // Arrange
        var queryParams = new ProductQueryParameters();
        List<ProductDto> products = [];
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<IEnumerable<ProductDto>>(okResult.Value, exactMatch: false);
        Assert.Empty(returnedProducts);
    }

    [Fact]
    public async Task GetProductsAsync_WithMultipleProducts_ReturnsAll()
    {
        // Arrange
        var queryParams = new ProductQueryParameters();
        var products = Enumerable.Range(1, 10).Select(i => new ProductDto
        {
            ProductId = i,
            ProductName = $"Product {i}",
            ProductPrice = i * 10.00m
        }).ToList();
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<IEnumerable<ProductDto>>(okResult.Value, exactMatch: false);
        Assert.Equal(10, returnedProducts.Count());
    }

    #endregion

    #region GetProductsAsync - Parameter Passing

    [Fact]
    public async Task GetProductsAsync_PassesPageNumberToService()
    {
        // Arrange
        var queryParams = new ProductQueryParameters { PageNumber = 5 };
        _mockProductService.Setup(s => s.GetProductsAsync(
            5, It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            5, It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_PassesPageSizeToService()
    {
        // Arrange
        var queryParams = new ProductQueryParameters { PageSize = 50 };
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), 50, It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            It.IsAny<int>(), 50, It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_PassesCategoryIdToService()
    {
        // Arrange
        var queryParams = new ProductQueryParameters { CategoryId = 3 };
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), 3,
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), 3,
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_PassesIsActiveToService()
    {
        // Arrange
        var queryParams = new ProductQueryParameters { IsActive = true };
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            true, It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            true, It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_PassesKeywordToService()
    {
        // Arrange
        var queryParams = new ProductQueryParameters { Keyword = "searchterm" };
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), "searchterm", It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), "searchterm", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_PassesNullCategoryIdWhenNotProvided()
    {
        // Arrange
        var queryParams = new ProductQueryParameters();
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), null,
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, CancellationToken.None);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), null,
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetProductByIdAsync - Success Cases

    [Fact]
    public async Task GetProductByIdAsync_WithExistingProduct_ReturnsOkResult()
    {
        // Arrange
        var product = new ProductDto { ProductId = 1, ProductName = "Test Product", ProductPrice = 25.00m };
        _mockProductService.Setup(s => s.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductByIdAsync(1, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithExistingProduct_ReturnsCorrectProduct()
    {
        // Arrange
        var product = new ProductDto
        {
            ProductId = 42,
            ProductName = "Special Product",
            ProductDescription = "Description",
            ProductPrice = 99.99m,
            CategoryId = 5,
            IsActive = true
        };
        _mockProductService.Setup(s => s.GetProductByIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductByIdAsync(42, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(42, returnedProduct.ProductId);
        Assert.Equal("Special Product", returnedProduct.ProductName);
    }

    #endregion

    #region GetProductByIdAsync - Not Found Cases

    [Fact]
    public async Task GetProductByIdAsync_WithNonExistentProduct_ReturnsNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetProductByIdAsync(999, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithZeroId_ReturnsNotFoundWhenServiceReturnsNull()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductByIdAsync(0, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetProductByIdAsync(0, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithNegativeId_ReturnsNotFoundWhenServiceReturnsNull()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductByIdAsync(-1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetProductByIdAsync(-1, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    #endregion

    #region AddProductAsync - Success Cases

    [Fact]
    public async Task AddProductAsync_WithValidProduct_ReturnsCreatedAtRoute()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "New Product",
            ProductDescription = "Description",
            ProductPrice = 50.00m,
            CategoryId = 1
        };
        var createdProduct = new ProductDto
        {
            ProductId = 1,
            ProductName = "New Product",
            ProductDescription = "Description",
            ProductPrice = 50.00m,
            CategoryId = 1
        };
        _mockProductService.Setup(s => s.AddProductAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(createdProduct));

        // Act
        var result = await _controller.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        Assert.Equal("GetProductById", createdResult.RouteName);
        Assert.Equal(1, createdResult.RouteValues!["id"]);
    }

    [Fact]
    public async Task AddProductAsync_WithValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "New Product",
            ProductDescription = "Description",
            ProductPrice = 75.00m,
            CategoryId = 2
        };
        var createdProduct = new ProductDto
        {
            ProductId = 5,
            ProductName = "New Product",
            ProductDescription = "Description",
            ProductPrice = 75.00m,
            CategoryId = 2
        };
        _mockProductService.Setup(s => s.AddProductAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(createdProduct));

        // Act
        var result = await _controller.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var returnedProduct = Assert.IsType<ProductDto>(createdResult.Value);
        Assert.Equal(5, returnedProduct.ProductId);
        Assert.Equal("New Product", returnedProduct.ProductName);
    }

    #endregion

    #region AddProductAsync - Failure Cases

    [Fact]
    public async Task AddProductAsync_WithNonExistentCategory_ReturnsBadRequestForValidationError()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "New Product",
            ProductDescription = "Description",
            ProductPrice = 50.00m,
            CategoryId = 999
        };
        var error = new Error("Category.NotFound", "Category does not exist", ErrorType.Validation);
        _mockProductService.Setup(s => s.AddProductAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddProductAsync_WithDuplicateName_ReturnsConflict()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "Existing Product",
            ProductDescription = "Description",
            ProductPrice = 50.00m,
            CategoryId = 1
        };
        var error = new Error("Product.Duplicate", "Product with this name already exists", ErrorType.Conflict);
        _mockProductService.Setup(s => s.AddProductAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddProductAsync_WithNotFoundError_ReturnsNotFound()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Description",
            ProductPrice = 50.00m,
            CategoryId = 999
        };
        var error = new Error("Category.NotFound", "Category not found", ErrorType.NotFound);
        _mockProductService.Setup(s => s.AddProductAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.AddProductAsync(createDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    #endregion

    #region UpdateProductAsync - Success Cases

    [Fact]
    public async Task UpdateProductAsync_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            ProductName = "Updated Product",
            ProductDescription = "Updated Description",
            ProductPrice = 100.00m,
            CategoryId = 1
        };
        var updatedProduct = new ProductDto
        {
            ProductId = 1,
            ProductName = "Updated Product",
            ProductDescription = "Updated Description",
            ProductPrice = 100.00m,
            CategoryId = 1
        };
        _mockProductService.Setup(s => s.UpdateProductAsync(1, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(updatedProduct));

        // Act
        var result = await _controller.UpdateProductAsync(1, updateDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(updatedProduct, okResult.Value);
    }

    #endregion

    #region UpdateProductAsync - Failure Cases

    [Fact]
    public async Task UpdateProductAsync_WithNonExistentProduct_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            ProductName = "Updated",
            ProductDescription = "Desc",
            ProductPrice = 50.00m,
            CategoryId = 1
        };
        var error = new Error("Product.NotFound", "Product not found", ErrorType.NotFound);
        _mockProductService.Setup(s => s.UpdateProductAsync(999, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.UpdateProductAsync(999, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateProductAsync_WithDuplicateName_ReturnsConflict()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            ProductName = "Existing Name",
            ProductDescription = "Desc",
            ProductPrice = 50.00m,
            CategoryId = 1
        };
        var error = new Error("Product.Duplicate", "Product name already exists", ErrorType.Conflict);
        _mockProductService.Setup(s => s.UpdateProductAsync(1, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.UpdateProductAsync(1, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateProductAsync_WithNonExistentCategory_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Desc",
            ProductPrice = 50.00m,
            CategoryId = 999
        };
        var error = new Error("Category.NotFound", "Category does not exist", ErrorType.Validation);
        _mockProductService.Setup(s => s.UpdateProductAsync(1, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.UpdateProductAsync(1, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region PatchProductAsync - Success Cases

    [Fact]
    public async Task PatchProductAsync_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var patchDto = new PatchProductDto { ProductPrice = 150.00m };
        var patchedProduct = new ProductDto
        {
            ProductId = 1,
            ProductName = "Product",
            ProductPrice = 150.00m
        };
        _mockProductService.Setup(s => s.PatchProductAsync(1, patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(patchedProduct));

        // Act
        var result = await _controller.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(patchedProduct, okResult.Value);
    }

    [Fact]
    public async Task PatchProductAsync_WithOnlyNameChange_ReturnsUpdatedProduct()
    {
        // Arrange
        var patchDto = new PatchProductDto { ProductName = "New Name" };
        var patchedProduct = new ProductDto
        {
            ProductId = 1,
            ProductName = "New Name",
            ProductPrice = 50.00m
        };
        _mockProductService.Setup(s => s.PatchProductAsync(1, patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(patchedProduct));

        // Act
        var result = await _controller.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal("New Name", returnedProduct.ProductName);
    }

    #endregion

    #region PatchProductAsync - Failure Cases

    [Fact]
    public async Task PatchProductAsync_WithNonExistentProduct_ReturnsNotFound()
    {
        // Arrange
        var patchDto = new PatchProductDto { ProductPrice = 75.00m };
        var error = new Error("Product.NotFound", "Product not found", ErrorType.NotFound);
        _mockProductService.Setup(s => s.PatchProductAsync(999, patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.PatchProductAsync(999, patchDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PatchProductAsync_WithDuplicateName_ReturnsConflict()
    {
        // Arrange
        var patchDto = new PatchProductDto { ProductName = "Existing Name" };
        var error = new Error("Product.Duplicate", "Product name already exists", ErrorType.Conflict);
        _mockProductService.Setup(s => s.PatchProductAsync(1, patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task PatchProductAsync_WithInvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var patchDto = new PatchProductDto { CategoryId = 999 };
        var error = new Error("Category.NotFound", "Category does not exist", ErrorType.Validation);
        _mockProductService.Setup(s => s.PatchProductAsync(1, patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure(error));

        // Act
        var result = await _controller.PatchProductAsync(1, patchDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region DeleteProductAsync - Success Cases

    [Fact]
    public async Task DeleteProductAsync_WithExistingProduct_ReturnsNoContent()
    {
        // Arrange
        _mockProductService.Setup(s => s.DeleteProductAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _controller.DeleteProductAsync(1, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    #endregion

    #region DeleteProductAsync - Failure Cases

    [Fact]
    public async Task DeleteProductAsync_WithNonExistentProduct_ReturnsNotFound()
    {
        // Arrange
        var error = new Error("Product.NotFound", "Product not found", ErrorType.NotFound);
        _mockProductService.Setup(s => s.DeleteProductAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.DeleteProductAsync(999, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithZeroId_ReturnsNotFoundWhenServiceFails()
    {
        // Arrange
        var error = new Error("Product.NotFound", "Product not found", ErrorType.NotFound);
        _mockProductService.Setup(s => s.DeleteProductAsync(0, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.DeleteProductAsync(0, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithNegativeId_ReturnsNotFoundWhenServiceFails()
    {
        // Arrange
        var error = new Error("Product.NotFound", "Product not found", ErrorType.NotFound);
        _mockProductService.Setup(s => s.DeleteProductAsync(-1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.DeleteProductAsync(-1, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    #endregion

    #region Cancellation Token Tests

    [Fact]
    public async Task GetProductsAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var queryParams = new ProductQueryParameters();
        var token = new CancellationToken();
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), token))
            .ReturnsAsync([]);

        // Act
        await _controller.GetProductsAsync(queryParams, token);

        // Assert
        _mockProductService.Verify(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), token), Times.Once);
    }

    [Fact]
    public async Task GetProductByIdAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var token = new CancellationToken();
        _mockProductService.Setup(s => s.GetProductByIdAsync(1, token))
            .ReturnsAsync(new ProductDto());

        // Act
        await _controller.GetProductByIdAsync(1, token);

        // Assert
        _mockProductService.Verify(s => s.GetProductByIdAsync(1, token), Times.Once);
    }

    [Fact]
    public async Task AddProductAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Desc",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var token = new CancellationToken();
        _mockProductService.Setup(s => s.AddProductAsync(createDto, token))
            .ReturnsAsync(Result<ProductDto>.Success(new ProductDto { ProductId = 1 }));

        // Act
        await _controller.AddProductAsync(createDto, token);

        // Assert
        _mockProductService.Verify(s => s.AddProductAsync(createDto, token), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Desc",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var token = new CancellationToken();
        _mockProductService.Setup(s => s.UpdateProductAsync(1, updateDto, token))
            .ReturnsAsync(Result<ProductDto>.Success(new ProductDto()));

        // Act
        await _controller.UpdateProductAsync(1, updateDto, token);

        // Assert
        _mockProductService.Verify(s => s.UpdateProductAsync(1, updateDto, token), Times.Once);
    }

    [Fact]
    public async Task PatchProductAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var patchDto = new PatchProductDto();
        var token = new CancellationToken();
        _mockProductService.Setup(s => s.PatchProductAsync(1, patchDto, token))
            .ReturnsAsync(Result<ProductDto>.Success(new ProductDto()));

        // Act
        await _controller.PatchProductAsync(1, patchDto, token);

        // Assert
        _mockProductService.Verify(s => s.PatchProductAsync(1, patchDto, token), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var token = new CancellationToken();
        _mockProductService.Setup(s => s.DeleteProductAsync(1, token))
            .ReturnsAsync(Result.Success());

        // Act
        await _controller.DeleteProductAsync(1, token);

        // Assert
        _mockProductService.Verify(s => s.DeleteProductAsync(1, token), Times.Once);
    }

    #endregion

    #region Service Exception Tests

    [Fact]
    public async Task GetProductsAsync_WhenServiceThrows_PropagatesException()
    {
        // Arrange
        var queryParams = new ProductQueryParameters();
        _mockProductService.Setup(s => s.GetProductsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(),
            It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _controller.GetProductsAsync(queryParams, CancellationToken.None));
    }

    [Fact]
    public async Task AddProductAsync_WhenServiceThrowsInvalidOperation_PropagatesException()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Desc",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        _mockProductService.Setup(s => s.AddProductAsync(createDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Creation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.AddProductAsync(createDto, CancellationToken.None));
    }

    #endregion
}

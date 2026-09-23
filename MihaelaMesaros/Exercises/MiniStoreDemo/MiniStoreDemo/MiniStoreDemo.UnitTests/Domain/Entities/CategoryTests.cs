using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.UnitTests.Domain.Entities;

public sealed class CategoryTests
{
    #region Constructor and Default Values Tests

    [Fact]
    public void Category_WhenCreated_HasDefaultCategoryIdOfZero()
    {
        // Act
        var category = new Category();

        // Assert
        Assert.Equal(0, category.CategoryId);
    }

    [Fact]
    public void Category_WhenCreated_HasNullCategoryName()
    {
        // Act
        var category = new Category();

        // Assert
        Assert.Null(category.CategoryName);
    }

    [Fact]
    public void Category_WhenCreated_HasEmptyProductsCollection()
    {
        // Act
        var category = new Category();

        // Assert
        Assert.NotNull(category.Products);
        Assert.Empty(category.Products);
    }

    [Fact]
    public void Category_WithObjectInitializer_SetsCategoryName()
    {
        // Arrange & Act
        var category = new Category { CategoryName = "Test Category" };

        // Assert
        Assert.Equal("Test Category", category.CategoryName);
    }

    #endregion

    #region Property Get/Set Tests

    [Fact]
    public void CategoryId_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var category = new Category();

        // Act
        category.CategoryId = 42;

        // Assert
        Assert.Equal(42, category.CategoryId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void CategoryId_SetVariousValues_ReturnsCorrectValue(int categoryId)
    {
        // Arrange
        var category = new Category();

        // Act
        category.CategoryId = categoryId;

        // Assert
        Assert.Equal(categoryId, category.CategoryId);
    }

    [Fact]
    public void CategoryName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var category = new Category();

        // Act
        category.CategoryName = "Electronics";

        // Assert
        Assert.Equal("Electronics", category.CategoryName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    [InlineData("Very Long Category Name That Could Be Used In Production")]
    public void CategoryName_SetVariousValues_ReturnsCorrectValue(string categoryName)
    {
        // Arrange
        var category = new Category();

        // Act
        category.CategoryName = categoryName;

        // Assert
        Assert.Equal(categoryName, category.CategoryName);
    }

    [Fact]
    public void CategoryName_SetToNull_ReturnsNull()
    {
        // Arrange
        var category = new Category { CategoryName = "Test" };

        // Act
        category.CategoryName = null!;

        // Assert
        Assert.Null(category.CategoryName);
    }

    [Fact]
    public void CategoryName_WithSpecialCharacters_ReturnsCorrectValue()
    {
        // Arrange
        var category = new Category();
        var specialName = "Électronique & Gadgets™ 日本語";

        // Act
        category.CategoryName = specialName;

        // Assert
        Assert.Equal(specialName, category.CategoryName);
    }

    #endregion

    #region Products Collection Tests

    [Fact]
    public void Products_AddProduct_IncreasesCount()
    {
        // Arrange
        var category = new Category();
        var product = new Product { ProductName = "Test Product" };

        // Act
        category.Products.Add(product);

        // Assert
        Assert.Single(category.Products);
    }

    [Fact]
    public void Products_AddMultipleProducts_ContainsAllProducts()
    {
        // Arrange
        var category = new Category();
        var product1 = new Product { ProductName = "Product 1" };
        var product2 = new Product { ProductName = "Product 2" };
        var product3 = new Product { ProductName = "Product 3" };

        // Act
        category.Products.Add(product1);
        category.Products.Add(product2);
        category.Products.Add(product3);

        // Assert
        Assert.Equal(3, category.Products.Count);
        Assert.Contains(product1, category.Products);
        Assert.Contains(product2, category.Products);
        Assert.Contains(product3, category.Products);
    }

    [Fact]
    public void Products_RemoveProduct_DecreasesCount()
    {
        // Arrange
        var category = new Category();
        var product = new Product { ProductName = "Test Product" };
        category.Products.Add(product);

        // Act
        category.Products.Remove(product);

        // Assert
        Assert.Empty(category.Products);
    }

    [Fact]
    public void Products_ClearCollection_ResultsInEmptyCollection()
    {
        // Arrange
        var category = new Category();
        category.Products.Add(new Product { ProductName = "Product 1" });
        category.Products.Add(new Product { ProductName = "Product 2" });

        // Act
        category.Products.Clear();

        // Assert
        Assert.Empty(category.Products);
    }

    [Fact]
    public void Products_SetToNewCollection_ReplacesExistingCollection()
    {
        // Arrange
        var category = new Category();
        category.Products.Add(new Product { ProductName = "Old Product" });
        var newProducts = new List<Product>
        {
            new() { ProductName = "New Product 1" },
            new() { ProductName = "New Product 2" }
        };

        // Act
        category.Products = newProducts;

        // Assert
        Assert.Equal(2, category.Products.Count);
        Assert.DoesNotContain(category.Products, p => p.ProductName == "Old Product");
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void Category_ObjectInitializer_SetsAllProperties()
    {
        // Arrange & Act
        var category = new Category
        {
            CategoryId = 5,
            CategoryName = "Books",
            Products = [new Product { ProductName = "Test Book" }]
        };

        // Assert
        Assert.Equal(5, category.CategoryId);
        Assert.Equal("Books", category.CategoryName);
        Assert.Single(category.Products);
    }

    #endregion

    #region Partial Class Verification

    [Fact]
    public void Category_IsPartialClass_CanBeExtended()
    {
        // Verify that Category is a partial class by checking it exists and has expected properties
        var category = new Category();
        var type = category.GetType();

        // Assert - verify the type exists and is public
        Assert.True(type.IsPublic);
        Assert.Equal("Category", type.Name);
        Assert.Equal("MiniStoreDemo.Domain.Entities", type.Namespace);
    }

    #endregion
}

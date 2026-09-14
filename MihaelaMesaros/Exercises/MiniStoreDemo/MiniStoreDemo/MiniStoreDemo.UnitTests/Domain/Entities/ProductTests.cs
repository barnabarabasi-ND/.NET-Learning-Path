using System.ComponentModel.DataAnnotations;
using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.UnitTests.Domain.Entities;

public sealed class ProductTests
{
    #region Constructor and Default Values Tests

    [Fact]
    public void Product_WhenCreated_HasDefaultProductIdOfZero()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Equal(0, product.ProductId);
    }

    [Fact]
    public void Product_WhenCreated_HasNullProductName()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Null(product.ProductName);
    }

    [Fact]
    public void Product_WhenCreated_HasNullProductDescription()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Null(product.ProductDescription);
    }

    [Fact]
    public void Product_WhenCreated_HasDefaultProductPriceOfZero()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Equal(0m, product.ProductPrice);
    }

    [Fact]
    public void Product_WhenCreated_HasDefaultCategoryIdOfZero()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Equal(0, product.CategoryId);
    }

    [Fact]
    public void Product_WhenCreated_HasDefaultIsActiveOfFalse()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.False(product.IsActive);
    }

    [Fact]
    public void Product_WhenCreated_HasDefaultCreatedAtOfMinValue()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Equal(default(DateTime), product.CreatedAt);
    }

    [Fact]
    public void Product_WhenCreated_HasNullModifiedAt()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Null(product.ModifiedAt);
    }

    [Fact]
    public void Product_WhenCreated_HasNullCategory()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Null(product.Category);
    }

    #endregion

    #region ProductId Property Tests

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void ProductId_SetVariousValues_ReturnsCorrectValue(int productId)
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductId = productId;

        // Assert
        Assert.Equal(productId, product.ProductId);
    }

    #endregion

    #region ProductName Property Tests

    [Fact]
    public void ProductName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductName = "Test Product";

        // Assert
        Assert.Equal("Test Product", product.ProductName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    [InlineData("Product with Special Characters @#$%^&*()")]
    public void ProductName_SetVariousValues_ReturnsCorrectValue(string productName)
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductName = productName;

        // Assert
        Assert.Equal(productName, product.ProductName);
    }

    [Fact]
    public void ProductName_ExactlyMaxLength_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();
        var maxLengthName = new string('x', 200);

        // Act
        product.ProductName = maxLengthName;

        // Assert
        Assert.Equal(maxLengthName, product.ProductName);
        Assert.Equal(200, product.ProductName.Length);
    }

    [Fact]
    public void ProductName_ExceedsMaxLength_StillAcceptsValue()
    {
        // Arrange - The property allows any length at runtime; MaxLength is for validation
        var product = new Product();
        var longName = new string('x', 500);

        // Act
        product.ProductName = longName;

        // Assert - Property accepts any length; validation happens separately
        Assert.Equal(longName, product.ProductName);
    }

    [Fact]
    public void ProductName_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(Product).GetProperty(nameof(Product.ProductName));

        // Act
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

        // Assert
        Assert.NotNull(requiredAttribute);
    }

    [Fact]
    public void ProductName_HasMaxLengthAttribute_WithValueOf200()
    {
        // Arrange
        var property = typeof(Product).GetProperty(nameof(Product.ProductName));

        // Act
        var maxLengthAttribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false)
            .FirstOrDefault() as MaxLengthAttribute;

        // Assert
        Assert.NotNull(maxLengthAttribute);
        Assert.Equal(200, maxLengthAttribute.Length);
    }

    #endregion

    #region ProductDescription Property Tests

    [Fact]
    public void ProductDescription_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductDescription = "A detailed description";

        // Assert
        Assert.Equal("A detailed description", product.ProductDescription);
    }

    [Fact]
    public void ProductDescription_SetToNull_ReturnsNull()
    {
        // Arrange
        var product = new Product { ProductDescription = "Initial Description" };

        // Act
        product.ProductDescription = null;

        // Assert
        Assert.Null(product.ProductDescription);
    }

    [Fact]
    public void ProductDescription_ExactlyMaxLength_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();
        var maxLengthDescription = new string('x', 1000);

        // Act
        product.ProductDescription = maxLengthDescription;

        // Assert
        Assert.Equal(maxLengthDescription, product.ProductDescription);
        Assert.Equal(1000, product.ProductDescription.Length);
    }

    [Fact]
    public void ProductDescription_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(Product).GetProperty(nameof(Product.ProductDescription));

        // Act
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

        // Assert
        Assert.NotNull(requiredAttribute);
    }

    [Fact]
    public void ProductDescription_HasMaxLengthAttribute_WithValueOf1000()
    {
        // Arrange
        var property = typeof(Product).GetProperty(nameof(Product.ProductDescription));

        // Act
        var maxLengthAttribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false)
            .FirstOrDefault() as MaxLengthAttribute;

        // Assert
        Assert.NotNull(maxLengthAttribute);
        Assert.Equal(1000, maxLengthAttribute.Length);
    }

    #endregion

    #region ProductPrice Property Tests

    [Theory]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(99.99)]
    [InlineData(1000000.00)]
    public void ProductPrice_SetValidValues_ReturnsCorrectValue(decimal price)
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductPrice = price;

        // Assert
        Assert.Equal(price, product.ProductPrice);
    }

    [Fact]
    public void ProductPrice_SetToZero_ReturnsZero()
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductPrice = 0m;

        // Assert
        Assert.Equal(0m, product.ProductPrice);
    }

    [Fact]
    public void ProductPrice_SetToNegative_ReturnsNegative()
    {
        // Arrange - Property accepts any value; validation happens separately
        var product = new Product();

        // Act
        product.ProductPrice = -10.00m;

        // Assert
        Assert.Equal(-10.00m, product.ProductPrice);
    }

    [Fact]
    public void ProductPrice_SetToDecimalMaxValue_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductPrice = decimal.MaxValue;

        // Assert
        Assert.Equal(decimal.MaxValue, product.ProductPrice);
    }

    [Fact]
    public void ProductPrice_SetToDecimalMinValue_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductPrice = decimal.MinValue;

        // Assert
        Assert.Equal(decimal.MinValue, product.ProductPrice);
    }

    [Fact]
    public void ProductPrice_WithManyDecimalPlaces_PreservesPrecision()
    {
        // Arrange
        var product = new Product();
        var precisePrice = 123.456789012345678901234567890m;

        // Act
        product.ProductPrice = precisePrice;

        // Assert
        Assert.Equal(precisePrice, product.ProductPrice);
    }

    [Fact]
    public void ProductPrice_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(Product).GetProperty(nameof(Product.ProductPrice));

        // Act
        var rangeAttribute = property?.GetCustomAttributes(typeof(RangeAttribute), false)
            .FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.NotNull(rangeAttribute);
        Assert.Equal(0.01, rangeAttribute.Minimum);
        Assert.Equal(double.MaxValue, rangeAttribute.Maximum);
    }

    #endregion

    #region CategoryId Property Tests

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void CategoryId_SetValidValues_ReturnsCorrectValue(int categoryId)
    {
        // Arrange
        var product = new Product();

        // Act
        product.CategoryId = categoryId;

        // Assert
        Assert.Equal(categoryId, product.CategoryId);
    }

    [Fact]
    public void CategoryId_SetToZero_ReturnsZero()
    {
        // Arrange - Property accepts any value; validation happens separately
        var product = new Product();

        // Act
        product.CategoryId = 0;

        // Assert
        Assert.Equal(0, product.CategoryId);
    }

    [Fact]
    public void CategoryId_SetToNegative_ReturnsNegative()
    {
        // Arrange - Property accepts any value; validation happens separately
        var product = new Product();

        // Act
        product.CategoryId = -1;

        // Assert
        Assert.Equal(-1, product.CategoryId);
    }

    [Fact]
    public void CategoryId_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(Product).GetProperty(nameof(Product.CategoryId));

        // Act
        var rangeAttribute = property?.GetCustomAttributes(typeof(RangeAttribute), false)
            .FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.NotNull(rangeAttribute);
        Assert.Equal(1, rangeAttribute.Minimum);
        Assert.Equal(int.MaxValue, rangeAttribute.Maximum);
    }

    #endregion

    #region IsActive Property Tests

    [Fact]
    public void IsActive_SetToTrue_ReturnsTrue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.IsActive = true;

        // Assert
        Assert.True(product.IsActive);
    }

    [Fact]
    public void IsActive_SetToFalse_ReturnsFalse()
    {
        // Arrange
        var product = new Product { IsActive = true };

        // Act
        product.IsActive = false;

        // Assert
        Assert.False(product.IsActive);
    }

    [Fact]
    public void IsActive_Toggle_ChangesValue()
    {
        // Arrange
        var product = new Product();

        // Act & Assert
        product.IsActive = true;
        Assert.True(product.IsActive);

        product.IsActive = false;
        Assert.False(product.IsActive);

        product.IsActive = true;
        Assert.True(product.IsActive);
    }

    #endregion

    #region CreatedAt Property Tests

    [Fact]
    public void CreatedAt_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();
        var createdAt = new DateTime(2026, 6, 15, 12, 30, 45, DateTimeKind.Utc);

        // Act
        product.CreatedAt = createdAt;

        // Assert
        Assert.Equal(createdAt, product.CreatedAt);
    }

    [Fact]
    public void CreatedAt_SetToMinValue_ReturnsMinValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.CreatedAt = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, product.CreatedAt);
    }

    [Fact]
    public void CreatedAt_SetToMaxValue_ReturnsMaxValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.CreatedAt = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, product.CreatedAt);
    }

    [Fact]
    public void CreatedAt_PreservesDateTimeKind()
    {
        // Arrange
        var product = new Product();
        var utcTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        product.CreatedAt = utcTime;

        // Assert
        Assert.Equal(DateTimeKind.Utc, product.CreatedAt.Kind);
    }

    #endregion

    #region ModifiedAt Property Tests

    [Fact]
    public void ModifiedAt_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();
        var modifiedAt = new DateTime(2026, 6, 15, 14, 30, 0, DateTimeKind.Utc);

        // Act
        product.ModifiedAt = modifiedAt;

        // Assert
        Assert.Equal(modifiedAt, product.ModifiedAt);
    }

    [Fact]
    public void ModifiedAt_SetToNull_ReturnsNull()
    {
        // Arrange
        var product = new Product { ModifiedAt = DateTime.UtcNow };

        // Act
        product.ModifiedAt = null;

        // Assert
        Assert.Null(product.ModifiedAt);
    }

    [Fact]
    public void ModifiedAt_HasValue_ReturnsTrue()
    {
        // Arrange
        var product = new Product { ModifiedAt = DateTime.UtcNow };

        // Assert
        Assert.True(product.ModifiedAt.HasValue);
    }

    [Fact]
    public void ModifiedAt_IsNull_HasValueReturnsFalse()
    {
        // Arrange
        var product = new Product();

        // Assert
        Assert.False(product.ModifiedAt.HasValue);
    }

    #endregion

    #region Category Navigation Property Tests

    [Fact]
    public void Category_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var product = new Product();
        var category = new Category { CategoryId = 1, CategoryName = "Electronics" };

        // Act
        product.Category = category;

        // Assert
        Assert.Same(category, product.Category);
        Assert.Equal(1, product.Category.CategoryId);
        Assert.Equal("Electronics", product.Category.CategoryName);
    }

    [Fact]
    public void Category_SetToNull_ReturnsNull()
    {
        // Arrange
        var product = new Product
        {
            Category = new Category { CategoryName = "Test" }
        };

        // Act
        product.Category = null!;

        // Assert
        Assert.Null(product.Category);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void Product_ObjectInitializer_SetsAllProperties()
    {
        // Arrange
        var category = new Category { CategoryId = 1, CategoryName = "Test Category" };
        var createdAt = DateTime.UtcNow;
        var modifiedAt = DateTime.UtcNow.AddHours(1);

        // Act
        var product = new Product
        {
            ProductId = 42,
            ProductName = "Test Product",
            ProductDescription = "Test Description",
            ProductPrice = 99.99m,
            CategoryId = 1,
            IsActive = true,
            CreatedAt = createdAt,
            ModifiedAt = modifiedAt,
            Category = category
        };

        // Assert
        Assert.Equal(42, product.ProductId);
        Assert.Equal("Test Product", product.ProductName);
        Assert.Equal("Test Description", product.ProductDescription);
        Assert.Equal(99.99m, product.ProductPrice);
        Assert.Equal(1, product.CategoryId);
        Assert.True(product.IsActive);
        Assert.Equal(createdAt, product.CreatedAt);
        Assert.Equal(modifiedAt, product.ModifiedAt);
        Assert.Same(category, product.Category);
    }

    #endregion

    #region Validation Tests Using DataAnnotations

    [Fact]
    public void Product_WithValidData_PassesValidation()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Valid Product",
            ProductDescription = "Valid Description",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Product_WithNullProductName_FailsValidation()
    {
        // Arrange
        var product = new Product
        {
            ProductName = null!,
            ProductDescription = "Description",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Product.ProductName)));
    }

    [Fact]
    public void Product_WithProductNameExceedingMaxLength_FailsValidation()
    {
        // Arrange
        var product = new Product
        {
            ProductName = new string('x', 201), // Exceeds 200 max length
            ProductDescription = "Description",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Product.ProductName)));
    }

    [Fact]
    public void Product_WithPriceBelowMinimum_FailsValidation()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            ProductDescription = "Description",
            ProductPrice = 0m, // Below 0.01 minimum
            CategoryId = 1
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Product.ProductPrice)));
    }

    [Fact]
    public void Product_WithCategoryIdBelowMinimum_FailsValidation()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            ProductDescription = "Description",
            ProductPrice = 10.00m,
            CategoryId = 0 // Below 1 minimum
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Product.CategoryId)));
    }

    [Fact]
    public void Product_WithDescriptionExceedingMaxLength_FailsValidation()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            ProductDescription = new string('x', 1001), // Exceeds 1000 max length
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Product.ProductDescription)));
    }

    [Fact]
    public void Product_WithNullDescription_FailsValidation()
    {
        // Arrange - ProductDescription has [Required] attribute despite being nullable
        var product = new Product
        {
            ProductName = "Test Product",
            ProductDescription = null,
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(product);

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Product.ProductDescription)));
    }

    #endregion

    #region Partial Class Verification

    [Fact]
    public void Product_IsPartialClass_HasExpectedTypeInfo()
    {
        // Arrange
        var product = new Product();
        var type = product.GetType();

        // Assert
        Assert.True(type.IsPublic);
        Assert.Equal("Product", type.Name);
        Assert.Equal("MiniStoreDemo.Domain.Entities", type.Namespace);
    }

    #endregion
}

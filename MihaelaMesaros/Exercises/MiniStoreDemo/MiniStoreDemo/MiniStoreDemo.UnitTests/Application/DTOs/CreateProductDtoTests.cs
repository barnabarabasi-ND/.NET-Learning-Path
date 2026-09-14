using System.ComponentModel.DataAnnotations;
using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.UnitTests.Application.DTOs;

public sealed class CreateProductDtoTests
{
    #region Default Value Tests

    [Fact]
    public void CreateProductDto_WhenCreated_HasNullProductName()
    {
        // Act
        var dto = new CreateProductDto();

        // Assert
        Assert.Null(dto.ProductName);
    }

    [Fact]
    public void CreateProductDto_WhenCreated_HasNullProductDescription()
    {
        // Act
        var dto = new CreateProductDto();

        // Assert
        Assert.Null(dto.ProductDescription);
    }

    [Fact]
    public void CreateProductDto_WhenCreated_HasDefaultProductPrice()
    {
        // Act
        var dto = new CreateProductDto();

        // Assert
        Assert.Equal(0m, dto.ProductPrice);
    }

    [Fact]
    public void CreateProductDto_WhenCreated_HasDefaultCategoryId()
    {
        // Act
        var dto = new CreateProductDto();

        // Assert
        Assert.Equal(0, dto.CategoryId);
    }

    [Fact]
    public void CreateProductDto_WhenCreated_HasDefaultIsActive()
    {
        // Act
        var dto = new CreateProductDto();

        // Assert
        Assert.False(dto.IsActive);
    }

    #endregion

    #region Property Get/Set Tests

    [Fact]
    public void ProductName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new CreateProductDto();

        // Act
        dto.ProductName = "Test Product";

        // Assert
        Assert.Equal("Test Product", dto.ProductName);
    }

    [Fact]
    public void ProductDescription_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new CreateProductDto();

        // Act
        dto.ProductDescription = "Test Description";

        // Assert
        Assert.Equal("Test Description", dto.ProductDescription);
    }

    [Fact]
    public void ProductPrice_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new CreateProductDto();

        // Act
        dto.ProductPrice = 99.99m;

        // Assert
        Assert.Equal(99.99m, dto.ProductPrice);
    }

    [Fact]
    public void CategoryId_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new CreateProductDto();

        // Act
        dto.CategoryId = 5;

        // Assert
        Assert.Equal(5, dto.CategoryId);
    }

    [Fact]
    public void IsActive_SetAndGet_ReturnsTrue()
    {
        // Arrange
        var dto = new CreateProductDto();

        // Act
        dto.IsActive = true;

        // Assert
        Assert.True(dto.IsActive);
    }

    #endregion

    #region Validation Attribute Tests

    [Fact]
    public void ProductName_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.ProductName));

        // Assert
        Assert.NotNull(property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault());
    }

    [Fact]
    public void ProductName_HasMaxLengthAttribute_200()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.ProductName));
        var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(200, attribute.Length);
    }

    [Fact]
    public void ProductDescription_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.ProductDescription));

        // Assert
        Assert.NotNull(property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault());
    }

    [Fact]
    public void ProductDescription_HasMaxLengthAttribute_1000()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.ProductDescription));
        var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(1000, attribute.Length);
    }

    [Fact]
    public void ProductPrice_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.ProductPrice));

        // Assert
        Assert.NotNull(property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault());
    }

    [Fact]
    public void ProductPrice_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.ProductPrice));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(0.01, attribute.Minimum);
        Assert.Equal(double.MaxValue, attribute.Maximum);
    }

    [Fact]
    public void CategoryId_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.CategoryId));

        // Assert
        Assert.NotNull(property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault());
    }

    [Fact]
    public void CategoryId_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(CreateProductDto).GetProperty(nameof(CreateProductDto.CategoryId));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Minimum);
        Assert.Equal(int.MaxValue, attribute.Maximum);
    }

    #endregion

    #region DataAnnotations Validation Tests

    [Fact]
    public void CreateProductDto_WithValidData_PassesValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = "Valid Product",
            ProductDescription = "Valid Description",
            ProductPrice = 10.00m,
            CategoryId = 1,
            IsActive = true
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateProductDto_WithNullProductName_FailsValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = null!,
            ProductDescription = "Description",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(CreateProductDto.ProductName)));
    }

    [Fact]
    public void CreateProductDto_WithProductNameExceedingMaxLength_FailsValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = new string('x', 201),
            ProductDescription = "Description",
            ProductPrice = 10.00m,
            CategoryId = 1
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void CreateProductDto_WithPriceBelowMinimum_FailsValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Description",
            ProductPrice = 0m,
            CategoryId = 1
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void CreateProductDto_WithCategoryIdZero_FailsValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = "Product",
            ProductDescription = "Description",
            ProductPrice = 10.00m,
            CategoryId = 0
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void CreateProductDto_ObjectInitializer_SetsAllProperties()
    {
        // Act
        var dto = new CreateProductDto
        {
            ProductName = "Test",
            ProductDescription = "Desc",
            ProductPrice = 50.00m,
            CategoryId = 2,
            IsActive = true
        };

        // Assert
        Assert.Equal("Test", dto.ProductName);
        Assert.Equal("Desc", dto.ProductDescription);
        Assert.Equal(50.00m, dto.ProductPrice);
        Assert.Equal(2, dto.CategoryId);
        Assert.True(dto.IsActive);
    }

    #endregion
}

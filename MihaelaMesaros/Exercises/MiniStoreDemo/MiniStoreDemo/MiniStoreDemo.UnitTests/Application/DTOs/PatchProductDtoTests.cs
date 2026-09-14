using System.ComponentModel.DataAnnotations;
using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.UnitTests.Application.DTOs;

public sealed class PatchProductDtoTests
{
    #region Default Value Tests

    [Fact]
    public void PatchProductDto_WhenCreated_HasNullProductName()
    {
        // Act
        var dto = new PatchProductDto();

        // Assert
        Assert.Null(dto.ProductName);
    }

    [Fact]
    public void PatchProductDto_WhenCreated_HasNullProductDescription()
    {
        // Act
        var dto = new PatchProductDto();

        // Assert
        Assert.Null(dto.ProductDescription);
    }

    [Fact]
    public void PatchProductDto_WhenCreated_HasNullProductPrice()
    {
        // Act
        var dto = new PatchProductDto();

        // Assert
        Assert.Null(dto.ProductPrice);
    }

    [Fact]
    public void PatchProductDto_WhenCreated_HasNullCategoryId()
    {
        // Act
        var dto = new PatchProductDto();

        // Assert
        Assert.Null(dto.CategoryId);
    }

    [Fact]
    public void PatchProductDto_WhenCreated_HasNullIsActive()
    {
        // Act
        var dto = new PatchProductDto();

        // Assert
        Assert.Null(dto.IsActive);
    }

    #endregion

    #region Property Get/Set Tests

    [Fact]
    public void ProductName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Act
        dto.ProductName = "Patched Product";

        // Assert
        Assert.Equal("Patched Product", dto.ProductName);
    }

    [Fact]
    public void ProductDescription_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Act
        dto.ProductDescription = "Patched Description";

        // Assert
        Assert.Equal("Patched Description", dto.ProductDescription);
    }

    [Fact]
    public void ProductPrice_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Act
        dto.ProductPrice = 199.99m;

        // Assert
        Assert.Equal(199.99m, dto.ProductPrice);
    }

    [Fact]
    public void CategoryId_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Act
        dto.CategoryId = 5;

        // Assert
        Assert.Equal(5, dto.CategoryId);
    }

    [Fact]
    public void IsActive_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Act
        dto.IsActive = true;

        // Assert
        Assert.True(dto.IsActive);
    }

    #endregion

    #region Nullable Property Tests

    [Fact]
    public void ProductPrice_HasValue_ReturnsTrue()
    {
        // Arrange
        var dto = new PatchProductDto { ProductPrice = 50.00m };

        // Assert
        Assert.True(dto.ProductPrice.HasValue);
    }

    [Fact]
    public void ProductPrice_IsNull_HasValueReturnsFalse()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Assert
        Assert.False(dto.ProductPrice.HasValue);
    }

    [Fact]
    public void CategoryId_HasValue_ReturnsTrue()
    {
        // Arrange
        var dto = new PatchProductDto { CategoryId = 3 };

        // Assert
        Assert.True(dto.CategoryId.HasValue);
    }

    [Fact]
    public void CategoryId_IsNull_HasValueReturnsFalse()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Assert
        Assert.False(dto.CategoryId.HasValue);
    }

    [Fact]
    public void IsActive_HasValue_ReturnsTrue()
    {
        // Arrange
        var dto = new PatchProductDto { IsActive = false };

        // Assert
        Assert.True(dto.IsActive.HasValue);
    }

    [Fact]
    public void IsActive_IsNull_HasValueReturnsFalse()
    {
        // Arrange
        var dto = new PatchProductDto();

        // Assert
        Assert.False(dto.IsActive.HasValue);
    }

    #endregion

    #region Validation Attribute Tests

    [Fact]
    public void ProductPrice_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(PatchProductDto).GetProperty(nameof(PatchProductDto.ProductPrice));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(0.01, attribute.Minimum);
        Assert.Equal(double.MaxValue, attribute.Maximum);
    }

    [Fact]
    public void CategoryId_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(PatchProductDto).GetProperty(nameof(PatchProductDto.CategoryId));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Minimum);
        Assert.Equal(int.MaxValue, attribute.Maximum);
    }

    [Fact]
    public void ProductName_DoesNotHaveRequiredAttribute()
    {
        // Arrange
        var property = typeof(PatchProductDto).GetProperty(nameof(PatchProductDto.ProductName));

        // Assert
        Assert.Null(property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault());
    }

    [Fact]
    public void ProductDescription_DoesNotHaveRequiredAttribute()
    {
        // Arrange
        var property = typeof(PatchProductDto).GetProperty(nameof(PatchProductDto.ProductDescription));

        // Assert
        Assert.Null(property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault());
    }

    #endregion

    #region DataAnnotations Validation Tests

    [Fact]
    public void PatchProductDto_WithAllNullValues_PassesValidation()
    {
        // Arrange
        var dto = new PatchProductDto();
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void PatchProductDto_WithValidPrice_PassesValidation()
    {
        // Arrange
        var dto = new PatchProductDto { ProductPrice = 10.00m };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void PatchProductDto_WithPriceBelowMinimum_FailsValidation()
    {
        // Arrange
        var dto = new PatchProductDto { ProductPrice = 0m };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void PatchProductDto_WithCategoryIdBelowMinimum_FailsValidation()
    {
        // Arrange
        var dto = new PatchProductDto { CategoryId = 0 };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void PatchProductDto_WithNegativeCategoryId_FailsValidation()
    {
        // Arrange
        var dto = new PatchProductDto { CategoryId = -1 };
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
    public void PatchProductDto_ObjectInitializer_SetsAllProperties()
    {
        // Act
        var dto = new PatchProductDto
        {
            ProductName = "Patched",
            ProductDescription = "Patched Desc",
            ProductPrice = 75.00m,
            CategoryId = 2,
            IsActive = false
        };

        // Assert
        Assert.Equal("Patched", dto.ProductName);
        Assert.Equal("Patched Desc", dto.ProductDescription);
        Assert.Equal(75.00m, dto.ProductPrice);
        Assert.Equal(2, dto.CategoryId);
        Assert.False(dto.IsActive);
    }

    [Fact]
    public void PatchProductDto_PartialInitialization_LeavesOthersNull()
    {
        // Act
        var dto = new PatchProductDto
        {
            ProductName = "Only Name"
        };

        // Assert
        Assert.Equal("Only Name", dto.ProductName);
        Assert.Null(dto.ProductDescription);
        Assert.Null(dto.ProductPrice);
        Assert.Null(dto.CategoryId);
        Assert.Null(dto.IsActive);
    }

    #endregion
}

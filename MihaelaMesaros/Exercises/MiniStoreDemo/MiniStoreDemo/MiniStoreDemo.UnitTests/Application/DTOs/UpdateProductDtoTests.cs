using System.ComponentModel.DataAnnotations;
using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.UnitTests.Application.DTOs;

public sealed class UpdateProductDtoTests
{
    #region Default Values

    [Fact]
    public void DefaultProductId_ShouldBeZero()
    {
        // Arrange & Act
        var dto = new UpdateProductDto();

        // Assert
        Assert.Equal(0, dto.ProductId);
    }

    [Fact]
    public void DefaultProductName_ShouldBeNull()
    {
        // Arrange & Act
        var dto = new UpdateProductDto();

        // Assert
        Assert.Null(dto.ProductName);
    }

    [Fact]
    public void DefaultProductDescription_ShouldBeNull()
    {
        // Arrange & Act
        var dto = new UpdateProductDto();

        // Assert
        Assert.Null(dto.ProductDescription);
    }

    [Fact]
    public void DefaultProductPrice_ShouldBeZero()
    {
        // Arrange & Act
        var dto = new UpdateProductDto();

        // Assert
        Assert.Equal(0m, dto.ProductPrice);
    }

    [Fact]
    public void DefaultCategoryId_ShouldBeZero()
    {
        // Arrange & Act
        var dto = new UpdateProductDto();

        // Assert
        Assert.Equal(0, dto.CategoryId);
    }

    [Fact]
    public void DefaultIsActive_ShouldBeFalse()
    {
        // Arrange & Act
        var dto = new UpdateProductDto();

        // Assert
        Assert.False(dto.IsActive);
    }

    #endregion

    #region Property Setters

    [Fact]
    public void ProductId_CanBeSet()
    {
        // Arrange & Act
        var dto = new UpdateProductDto { ProductId = 42 };

        // Assert
        Assert.Equal(42, dto.ProductId);
    }

    [Fact]
    public void ProductName_CanBeSet()
    {
        // Arrange & Act
        var dto = new UpdateProductDto { ProductName = "Test Product" };

        // Assert
        Assert.Equal("Test Product", dto.ProductName);
    }

    [Fact]
    public void ProductDescription_CanBeSet()
    {
        // Arrange & Act
        var dto = new UpdateProductDto { ProductDescription = "Test Description" };

        // Assert
        Assert.Equal("Test Description", dto.ProductDescription);
    }

    [Fact]
    public void ProductPrice_CanBeSet()
    {
        // Arrange & Act
        var dto = new UpdateProductDto { ProductPrice = 99.99m };

        // Assert
        Assert.Equal(99.99m, dto.ProductPrice);
    }

    [Fact]
    public void CategoryId_CanBeSet()
    {
        // Arrange & Act
        var dto = new UpdateProductDto { CategoryId = 5 };

        // Assert
        Assert.Equal(5, dto.CategoryId);
    }

    [Fact]
    public void IsActive_CanBeSet()
    {
        // Arrange & Act
        var dto = new UpdateProductDto { IsActive = true };

        // Assert
        Assert.True(dto.IsActive);
    }

    #endregion

    #region Validation Attributes - ProductId

    [Fact]
    public void ProductId_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductId));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
        Assert.NotNull(attribute);
    }

    #endregion

    #region Validation Attributes - ProductName

    [Fact]
    public void ProductName_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductName));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
        Assert.NotNull(attribute);
    }

    [Fact]
    public void ProductName_HasMaxLengthAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductName));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(200, attribute.Length);
    }

    [Fact]
    public void ProductName_ExactlyMaxLength_IsValid()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductName = new string('a', 200);
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ProductName_ExceedsMaxLength_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductName = new string('a', 201);
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(UpdateProductDto.ProductName)));
    }

    [Fact]
    public void ProductName_Null_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductName = null!;
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(UpdateProductDto.ProductName)));
    }

    #endregion

    #region Validation Attributes - ProductDescription

    [Fact]
    public void ProductDescription_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductDescription));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
        Assert.NotNull(attribute);
    }

    [Fact]
    public void ProductDescription_HasMaxLengthAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductDescription));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(1000, attribute.Length);
    }

    [Fact]
    public void ProductDescription_ExactlyMaxLength_IsValid()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductDescription = new string('a', 1000);
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ProductDescription_ExceedsMaxLength_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductDescription = new string('a', 1001);
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(UpdateProductDto.ProductDescription)));
    }

    #endregion

    #region Validation Attributes - ProductPrice

    [Fact]
    public void ProductPrice_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductPrice));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
        Assert.NotNull(attribute);
    }

    [Fact]
    public void ProductPrice_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductPrice));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(0.01, attribute.Minimum);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(100.00)]
    [InlineData(999999.99)]
    public void ProductPrice_ValidValues_PassValidation(decimal price)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductPrice = price;
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void ProductPrice_InvalidValues_FailValidation(decimal price)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductPrice = price;
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(UpdateProductDto.ProductPrice)));
    }

    #endregion

    #region Validation Attributes - CategoryId

    [Fact]
    public void CategoryId_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.CategoryId));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
        Assert.NotNull(attribute);
    }

    [Fact]
    public void CategoryId_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.CategoryId));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Minimum);
        Assert.Equal(int.MaxValue, attribute.Maximum);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void CategoryId_ValidValues_PassValidation(int categoryId)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.CategoryId = categoryId;
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void CategoryId_InvalidValues_FailValidation(int categoryId)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.CategoryId = categoryId;
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(UpdateProductDto.CategoryId)));
    }

    #endregion

    #region Full Model Validation

    [Fact]
    public void ValidDto_PassesValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void DefaultDto_FailsValidation()
    {
        // Arrange
        var dto = new UpdateProductDto();
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.True(validationResults.Count > 0);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ProductName_EmptyString_FailsRequiredValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductName = "";
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void ProductName_WithSpecialCharacters_IsValid()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductName = "Product™ - Special & \"Quoted\" <Test>";
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ProductName_WithUnicodeCharacters_IsValid()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ProductName = "产品名称 товар 🎁";
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ProductPrice_WithManyDecimalPlaces_CanBeSet()
    {
        // Arrange
        var dto = CreateValidDto();

        // Act
        dto.ProductPrice = 99.999999999999999m;

        // Assert
        Assert.Equal(99.999999999999999m, dto.ProductPrice);
    }

    #endregion

    #region Object Initialization

    [Fact]
    public void ObjectInitializer_SetsAllProperties()
    {
        // Arrange & Act
        var dto = new UpdateProductDto
        {
            ProductId = 42,
            ProductName = "Test Product",
            ProductDescription = "Test Description",
            ProductPrice = 99.99m,
            CategoryId = 5,
            IsActive = true
        };

        // Assert
        Assert.Equal(42, dto.ProductId);
        Assert.Equal("Test Product", dto.ProductName);
        Assert.Equal("Test Description", dto.ProductDescription);
        Assert.Equal(99.99m, dto.ProductPrice);
        Assert.Equal(5, dto.CategoryId);
        Assert.True(dto.IsActive);
    }

    #endregion

    #region Type Information

    [Fact]
    public void Class_HasSixProperties()
    {
        // Assert
        Assert.Equal(6, typeof(UpdateProductDto).GetProperties().Length);
    }

    [Fact]
    public void ProductId_IsIntType()
    {
        // Assert
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductId));
        Assert.Equal(typeof(int), property?.PropertyType);
    }

    [Fact]
    public void ProductPrice_IsDecimalType()
    {
        // Assert
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.ProductPrice));
        Assert.Equal(typeof(decimal), property?.PropertyType);
    }

    [Fact]
    public void IsActive_IsBoolType()
    {
        // Assert
        var property = typeof(UpdateProductDto).GetProperty(nameof(UpdateProductDto.IsActive));
        Assert.Equal(typeof(bool), property?.PropertyType);
    }

    #endregion

    #region Helper Methods

    private static UpdateProductDto CreateValidDto()
    {
        return new UpdateProductDto
        {
            ProductId = 1,
            ProductName = "Valid Product",
            ProductDescription = "Valid Description",
            ProductPrice = 10.00m,
            CategoryId = 1,
            IsActive = true
        };
    }

    #endregion
}

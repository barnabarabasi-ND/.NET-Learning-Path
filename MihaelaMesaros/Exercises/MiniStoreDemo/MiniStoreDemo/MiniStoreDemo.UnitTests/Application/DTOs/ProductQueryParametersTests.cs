using System.ComponentModel.DataAnnotations;
using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.UnitTests.Application.DTOs;

public sealed class ProductQueryParametersTests
{
    #region Default Values

    [Fact]
    public void DefaultPageNumber_ShouldBeOne()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters();

        // Assert
        Assert.Equal(1, parameters.PageNumber);
    }

    [Fact]
    public void DefaultPageSize_ShouldBeTen()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters();

        // Assert
        Assert.Equal(10, parameters.PageSize);
    }

    [Fact]
    public void DefaultCategoryId_ShouldBeNull()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters();

        // Assert
        Assert.Null(parameters.CategoryId);
    }

    [Fact]
    public void DefaultIsActive_ShouldBeNull()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters();

        // Assert
        Assert.Null(parameters.IsActive);
    }

    [Fact]
    public void DefaultKeyword_ShouldBeNull()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters();

        // Assert
        Assert.Null(parameters.Keyword);
    }

    #endregion

    #region Property Setters

    [Fact]
    public void PageNumber_CanBeSet()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters() { PageNumber = 5};

        // Assert
        Assert.Equal(5, parameters.PageNumber);
    }

    [Fact]
    public void PageSize_CanBeSet()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters() { PageSize = 50 };

        // Assert
        Assert.Equal(50, parameters.PageSize);
    }

    [Fact]
    public void CategoryId_CanBeSet()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters() { CategoryId = 3 };

        // Assert
        Assert.Equal(3, parameters.CategoryId);
    }

    [Fact]
    public void IsActive_CanBeSetToTrue()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { IsActive = true };

        // Assert
        Assert.True(parameters.IsActive);
    }

    [Fact]
    public void IsActive_CanBeSetToFalse()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { IsActive = false };

        // Assert
        Assert.False(parameters.IsActive);
    }

    [Fact]
    public void Keyword_CanBeSet()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { Keyword = "search term" };

        // Assert
        Assert.Equal("search term", parameters.Keyword);
    }

    #endregion

    #region Validation Attributes - PageNumber

    [Fact]
    public void PageNumber_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageNumber));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;
        Assert.NotNull(attribute);
    }

    [Fact]
    public void PageNumber_RangeAttributeMinimumIsOne()
    {
        // Arrange
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageNumber));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.Equal(1, attribute?.Minimum);
    }

    [Fact]
    public void PageNumber_RangeAttributeMaximumIsIntMaxValue()
    {
        // Arrange
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageNumber));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.Equal(int.MaxValue, attribute?.Maximum);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void PageNumber_ValidValues_PassValidation(int pageNumber)
    {
        // Arrange
        var parameters = new ProductQueryParameters { PageNumber = pageNumber };
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.DoesNotContain(validationResults, r => r.MemberNames.Contains(nameof(ProductQueryParameters.PageNumber)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void PageNumber_InvalidValues_FailValidation(int pageNumber)
    {
        // Arrange
        var parameters = new ProductQueryParameters { PageNumber = pageNumber };
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(ProductQueryParameters.PageNumber)));
    }

    #endregion

    #region Validation Attributes - PageSize

    [Fact]
    public void PageSize_HasRangeAttribute()
    {
        // Arrange
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageSize));

        // Assert
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;
        Assert.NotNull(attribute);
    }

    [Fact]
    public void PageSize_RangeAttributeMinimumIsOne()
    {
        // Arrange
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageSize));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.Equal(1, attribute?.Minimum);
    }

    [Fact]
    public void PageSize_RangeAttributeMaximumIs100()
    {
        // Arrange
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageSize));
        var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

        // Assert
        Assert.Equal(100, attribute?.Maximum);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]
    public void PageSize_ValidValues_PassValidation(int pageSize)
    {
        // Arrange
        var parameters = new ProductQueryParameters { PageSize = pageSize };
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.DoesNotContain(validationResults, r => r.MemberNames.Contains(nameof(ProductQueryParameters.PageSize)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(1000)]
    public void PageSize_InvalidValues_FailValidation(int pageSize)
    {
        // Arrange
        var parameters = new ProductQueryParameters { PageSize = pageSize };
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(ProductQueryParameters.PageSize)));
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Keyword_CanBeEmptyString()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { Keyword = "" };

        // Assert
        Assert.Equal("", parameters.Keyword);
    }

    [Fact]
    public void Keyword_CanContainSpecialCharacters()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { Keyword = "!@#$%^&*()" };

        // Assert
        Assert.Equal("!@#$%^&*()", parameters.Keyword);
    }

    [Fact]
    public void Keyword_CanContainUnicodeCharacters()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { Keyword = "产品搜索" };

        // Assert
        Assert.Equal("产品搜索", parameters.Keyword);
    }

    [Fact]
    public void Keyword_CanContainWhitespace()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { Keyword = "  search term  " };

        // Assert
        Assert.Equal("  search term  ", parameters.Keyword);
    }

    [Fact]
    public void Keyword_CanBeVeryLong()
    {
        // Arrange
        var longKeyword = new string('a', 10000);

        // Act
        var parameters = new ProductQueryParameters { Keyword = longKeyword };

        // Assert
        Assert.Equal(longKeyword, parameters.Keyword);
    }

    [Fact]
    public void CategoryId_CanBeSetToZero()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { CategoryId = 0 };

        // Assert
        Assert.Equal(0, parameters.CategoryId);
    }

    [Fact]
    public void CategoryId_CanBeSetToNegative()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { CategoryId = -1 };

        // Assert
        Assert.Equal(-1, parameters.CategoryId);
    }

    [Fact]
    public void CategoryId_CanBeSetToLargeValue()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters { CategoryId = int.MaxValue };

        // Assert
        Assert.Equal(int.MaxValue, parameters.CategoryId);
    }

    #endregion

    #region Object Initialization

    [Fact]
    public void ObjectInitializer_SetsAllProperties()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters
        {
            PageNumber = 3,
            PageSize = 25,
            CategoryId = 5,
            IsActive = true,
            Keyword = "test"
        };

        // Assert
        Assert.Equal(3, parameters.PageNumber);
        Assert.Equal(25, parameters.PageSize);
        Assert.Equal(5, parameters.CategoryId);
        Assert.True(parameters.IsActive);
        Assert.Equal("test", parameters.Keyword);
    }

    [Fact]
    public void PartialObjectInitializer_UsesDefaults()
    {
        // Arrange & Act
        var parameters = new ProductQueryParameters
        {
            Keyword = "search"
        };

        // Assert
        Assert.Equal(1, parameters.PageNumber);
        Assert.Equal(10, parameters.PageSize);
        Assert.Null(parameters.CategoryId);
        Assert.Null(parameters.IsActive);
        Assert.Equal("search", parameters.Keyword);
    }

    #endregion

    #region Full Model Validation

    [Fact]
    public void DefaultParameters_AreValid()
    {
        // Arrange
        var parameters = new ProductQueryParameters();
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void AllValidValues_AreValid()
    {
        // Arrange
        var parameters = new ProductQueryParameters
        {
            PageNumber = 5,
            PageSize = 50,
            CategoryId = 10,
            IsActive = true,
            Keyword = "product"
        };
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void BothInvalidValues_FailsWithMultipleErrors()
    {
        // Arrange
        var parameters = new ProductQueryParameters
        {
            PageNumber = 0,
            PageSize = 200
        };
        var validationContext = new ValidationContext(parameters);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(parameters, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Equal(2, validationResults.Count);
    }

    #endregion

    #region Type Information

    [Fact]
    public void Class_IsNotSealed()
    {
        // Assert
        Assert.False(typeof(ProductQueryParameters).IsSealed);
    }

    [Fact]
    public void Class_HasFiveProperties()
    {
        // Assert
        Assert.Equal(5, typeof(ProductQueryParameters).GetProperties().Length);
    }

    [Fact]
    public void PageNumber_IsIntType()
    {
        // Assert
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageNumber));
        Assert.Equal(typeof(int), property?.PropertyType);
    }

    [Fact]
    public void PageSize_IsIntType()
    {
        // Assert
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.PageSize));
        Assert.Equal(typeof(int), property?.PropertyType);
    }

    [Fact]
    public void CategoryId_IsNullableIntType()
    {
        // Assert
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.CategoryId));
        Assert.Equal(typeof(int?), property?.PropertyType);
    }

    [Fact]
    public void IsActive_IsNullableBoolType()
    {
        // Assert
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.IsActive));
        Assert.Equal(typeof(bool?), property?.PropertyType);
    }

    [Fact]
    public void Keyword_IsNullableStringType()
    {
        // Assert
        var property = typeof(ProductQueryParameters).GetProperty(nameof(ProductQueryParameters.Keyword));
        Assert.Equal(typeof(string), property?.PropertyType);
    }

    #endregion
}

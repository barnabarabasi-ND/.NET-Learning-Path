using MiniStoreDemo.Application.Common;

namespace MiniStoreDemo.UnitTests.Application.Common;

public sealed class ErrorTypeTests
{
    #region Enum Value Tests

    [Fact]
    public void ErrorType_HasValidationValue()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(ErrorType), ErrorType.Validation));
    }

    [Fact]
    public void ErrorType_HasNotFoundValue()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(ErrorType), ErrorType.NotFound));
    }

    [Fact]
    public void ErrorType_HasConflictValue()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(ErrorType), ErrorType.Conflict));
    }

    [Fact]
    public void ErrorType_HasExactlyThreeValues()
    {
        // Arrange
        var values = Enum.GetValues<ErrorType>();

        // Assert
        Assert.Equal(3, values.Length);
    }

    #endregion

    #region Enum Casting Tests

    [Theory]
    [InlineData(0, ErrorType.Validation)]
    [InlineData(1, ErrorType.NotFound)]
    [InlineData(2, ErrorType.Conflict)]
    public void ErrorType_CastFromInt_ReturnsCorrectValue(int intValue, ErrorType expected)
    {
        // Act
        var result = (ErrorType)intValue;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ErrorType.Validation, 0)]
    [InlineData(ErrorType.NotFound, 1)]
    [InlineData(ErrorType.Conflict, 2)]
    public void ErrorType_CastToInt_ReturnsCorrectValue(ErrorType errorType, int expected)
    {
        // Act
        var result = (int)errorType;

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Enum String Conversion Tests

    [Fact]
    public void ErrorType_Validation_ToStringReturnsCorrectName()
    {
        // Assert
        Assert.Equal("Validation", ErrorType.Validation.ToString());
    }

    [Fact]
    public void ErrorType_NotFound_ToStringReturnsCorrectName()
    {
        // Assert
        Assert.Equal("NotFound", ErrorType.NotFound.ToString());
    }

    [Fact]
    public void ErrorType_Conflict_ToStringReturnsCorrectName()
    {
        // Assert
        Assert.Equal("Conflict", ErrorType.Conflict.ToString());
    }

    [Theory]
    [InlineData("Validation", ErrorType.Validation)]
    [InlineData("NotFound", ErrorType.NotFound)]
    [InlineData("Conflict", ErrorType.Conflict)]
    public void ErrorType_ParseFromString_ReturnsCorrectValue(string stringValue, ErrorType expected)
    {
        // Act
        var result = Enum.Parse<ErrorType>(stringValue);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ErrorType_TryParseValidString_ReturnsTrue()
    {
        // Act
        var success = Enum.TryParse<ErrorType>("Validation", out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(ErrorType.Validation, result);
    }

    [Fact]
    public void ErrorType_TryParseInvalidString_ReturnsFalse()
    {
        // Act
        var success = Enum.TryParse<ErrorType>("InvalidValue", out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void ErrorType_ParseCaseInsensitive_ReturnsCorrectValue()
    {
        // Act
        var result = Enum.Parse<ErrorType>("validation", ignoreCase: true);

        // Assert
        Assert.Equal(ErrorType.Validation, result);
    }

    #endregion

    #region Enum Comparison Tests

    [Fact]
    public void ErrorType_SameValues_AreEqual()
    {
        // Arrange
        var type1 = ErrorType.Validation;
        var type2 = ErrorType.Validation;

        // Assert
        Assert.Equal(type1, type2);
    }

    [Fact]
    public void ErrorType_DifferentValues_AreNotEqual()
    {
        // Arrange
        var type1 = ErrorType.Validation;
        var type2 = ErrorType.NotFound;

        // Assert
        Assert.NotEqual(type1, type2);
    }

    #endregion
}

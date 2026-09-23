using MiniStoreDemo.Application.Common;

namespace MiniStoreDemo.UnitTests.Application.Common;

public sealed class ResultTests
{
    #region Result (Non-Generic) - Success Tests

    [Fact]
    public void Success_ReturnsResultWithIsSuccessTrue()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Success_ReturnsResultWithNullError()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.Null(result.Error);
    }

    #endregion

    #region Result (Non-Generic) - Failure Tests

    [Fact]
    public void Failure_ReturnsResultWithIsSuccessFalse()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.Validation);

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Failure_ReturnsResultWithProvidedError()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.Validation);

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal("TestCode", result.Error.Code);
        Assert.Equal("Test description", result.Error.Description);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Theory]
    [InlineData(ErrorType.Validation)]
    [InlineData(ErrorType.NotFound)]
    [InlineData(ErrorType.Conflict)]
    public void Failure_WithDifferentErrorTypes_ReturnsCorrectErrorType(ErrorType errorType)
    {
        // Arrange
        var error = new Error("Code", "Description", errorType);

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.Equal(errorType, result.Error!.Type);
    }

    #endregion

    #region Result<T> (Generic) - Success Tests

    [Fact]
    public void GenericSuccess_WithValue_ReturnsResultWithIsSuccessTrue()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GenericSuccess_WithValue_ReturnsResultWithProvidedValue()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void GenericSuccess_WithValue_ReturnsResultWithNullError()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.Null(result.Error);
    }

    [Fact]
    public void GenericSuccess_WithNullValue_AcceptsNull()
    {
        // Act
        var result = Result<string?>.Success(null);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public void GenericSuccess_WithIntValue_ReturnsCorrectValue()
    {
        // Act
        var result = Result<int>.Success(42);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void GenericSuccess_WithComplexObject_ReturnsCorrectReference()
    {
        // Arrange
        var obj = new { Name = "Test", Value = 123 };

        // Act
        var result = Result<object>.Success(obj);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Same(obj, result.Value);
    }

    #endregion

    #region Result<T> (Generic) - Failure Tests

    [Fact]
    public void GenericFailure_ReturnsResultWithIsSuccessFalse()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.Validation);

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void GenericFailure_ReturnsResultWithProvidedError()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.NotFound);

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal("TestCode", result.Error.Code);
        Assert.Equal("Test description", result.Error.Description);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public void GenericFailure_ReturnsResultWithDefaultValue()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.Validation);

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.Null(result.Value);
    }

    [Fact]
    public void GenericFailure_WithValueType_ReturnsDefaultValue()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.Validation);

        // Act
        var result = Result<int>.Failure(error);

        // Assert
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void GenericFailure_WithBoolType_ReturnsFalse()
    {
        // Arrange
        var error = new Error("TestCode", "Test description", ErrorType.Validation);

        // Act
        var result = Result<bool>.Failure(error);

        // Assert
        Assert.False(result.Value);
    }

    #endregion

    #region Result Inheritance Tests

    [Fact]
    public void GenericResult_InheritsFromResult()
    {
        // Arrange
        var result = Result<string>.Success("test");

        // Assert
        Assert.IsAssignableFrom<Result>(result);
    }

    [Fact]
    public void GenericResult_CanBeAssignedToBaseResult()
    {
        // Arrange
        Result<string> genericResult = Result<string>.Success("test");

        // Act
        Result baseResult = genericResult;

        // Assert
        Assert.True(baseResult.IsSuccess);
        Assert.Null(baseResult.Error);
    }

    #endregion
}

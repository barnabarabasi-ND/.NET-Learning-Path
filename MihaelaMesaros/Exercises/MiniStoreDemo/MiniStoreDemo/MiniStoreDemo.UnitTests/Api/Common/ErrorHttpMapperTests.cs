using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Api.Common;
using MiniStoreDemo.Application.Common;

namespace MiniStoreDemo.UnitTests.Api.Common;

public sealed class ErrorHttpMapperTests
{
    #region ToProblemResult - Validation Error

    [Fact]
    public void ToProblemResult_WithValidationError_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var error = new Error("VAL001", "Validation failed", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void ToProblemResult_WithValidationError_Returns400StatusCode()
    {
        // Arrange
        var error = new Error("VAL001", "Validation failed", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Fact]
    public void ToProblemResult_WithValidationError_ReturnsProblemDetailsWithCorrectTitle()
    {
        // Arrange
        var error = new Error("VAL001", "Validation failed", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Validation failed", problemDetails.Title);
    }

    [Fact]
    public void ToProblemResult_WithValidationError_ReturnsProblemDetailsWithErrorDescription()
    {
        // Arrange
        var error = new Error("VAL001", "Field X is required", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Field X is required", problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithValidationError_ReturnsProblemDetailsWithErrorCode()
    {
        // Arrange
        var error = new Error("VAL001", "Validation failed", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.True(problemDetails.Extensions.ContainsKey("code"));
        Assert.Equal("VAL001", problemDetails.Extensions["code"]);
    }

    #endregion

    #region ToProblemResult - NotFound Error

    [Fact]
    public void ToProblemResult_WithNotFoundError_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var error = new Error("NF001", "Resource not found", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ToProblemResult_WithNotFoundError_Returns404StatusCode()
    {
        // Arrange
        var error = new Error("NF001", "Resource not found", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Fact]
    public void ToProblemResult_WithNotFoundError_ReturnsProblemDetailsWithCorrectTitle()
    {
        // Arrange
        var error = new Error("NF001", "Resource not found", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Resource not found", problemDetails.Title);
    }

    [Fact]
    public void ToProblemResult_WithNotFoundError_ReturnsProblemDetailsWithErrorDescription()
    {
        // Arrange
        var error = new Error("NF001", "Product with ID 123 was not found", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Product with ID 123 was not found", problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithNotFoundError_ReturnsProblemDetailsWithErrorCode()
    {
        // Arrange
        var error = new Error("NF001", "Resource not found", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.True(problemDetails.Extensions.ContainsKey("code"));
        Assert.Equal("NF001", problemDetails.Extensions["code"]);
    }

    #endregion

    #region ToProblemResult - Conflict Error

    [Fact]
    public void ToProblemResult_WithConflictError_ReturnsConflictObjectResult()
    {
        // Arrange
        var error = new Error("CF001", "Resource conflict", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public void ToProblemResult_WithConflictError_Returns409StatusCode()
    {
        // Arrange
        var error = new Error("CF001", "Resource conflict", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
    }

    [Fact]
    public void ToProblemResult_WithConflictError_ReturnsProblemDetailsWithCorrectTitle()
    {
        // Arrange
        var error = new Error("CF001", "Resource conflict", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Resource conflict", problemDetails.Title);
    }

    [Fact]
    public void ToProblemResult_WithConflictError_ReturnsProblemDetailsWithErrorDescription()
    {
        // Arrange
        var error = new Error("CF001", "A product with this name already exists", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("A product with this name already exists", problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithConflictError_ReturnsProblemDetailsWithErrorCode()
    {
        // Arrange
        var error = new Error("CF001", "Resource conflict", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.True(problemDetails.Extensions.ContainsKey("code"));
        Assert.Equal("CF001", problemDetails.Extensions["code"]);
    }

    #endregion

    #region ToProblemResult - Unsupported Error Type

    [Fact]
    public void ToProblemResult_WithUnsupportedErrorType_ThrowsInvalidOperationException()
    {
        // Arrange
        var error = new Error("UNK001", "Unknown error", (ErrorType)999);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ErrorHttpMapper.ToProblemResult(error));
        Assert.Contains("Unsupported error type", exception.Message);
    }

    [Fact]
    public void ToProblemResult_WithUnsupportedErrorType_ExceptionMessageContainsErrorType()
    {
        // Arrange
        var error = new Error("UNK001", "Unknown error", (ErrorType)999);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ErrorHttpMapper.ToProblemResult(error));
        Assert.Contains("999", exception.Message);
    }

    #endregion

    #region ToProblemResult - Edge Cases

    [Fact]
    public void ToProblemResult_WithEmptyErrorCode_StillCreatesValidResponse()
    {
        // Arrange
        var error = new Error("", "Empty code error", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("", problemDetails.Extensions["code"]);
    }

    [Fact]
    public void ToProblemResult_WithEmptyDescription_StillCreatesValidResponse()
    {
        // Arrange
        var error = new Error("ERR001", "", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("", problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithSpecialCharactersInDescription_PreservesCharacters()
    {
        // Arrange
        var error = new Error("ERR001", "Error: <script>alert('xss')</script> & other \"chars\"", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Error: <script>alert('xss')</script> & other \"chars\"", problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithUnicodeCharactersInDescription_PreservesCharacters()
    {
        // Arrange
        var error = new Error("ERR001", "错误信息 • émoji 🚨", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("错误信息 • émoji 🚨", problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithVeryLongDescription_PreservesFullDescription()
    {
        // Arrange
        var longDescription = new string('x', 10000);
        var error = new Error("ERR001", longDescription, ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal(longDescription, problemDetails.Detail);
    }

    [Fact]
    public void ToProblemResult_WithNewlinesInDescription_PreservesNewlines()
    {
        // Arrange
        var error = new Error("ERR001", "Line 1\nLine 2\r\nLine 3", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal("Line 1\nLine 2\r\nLine 3", problemDetails.Detail);
    }

    #endregion

    #region ProblemDetails Structure Tests

    [Fact]
    public void ToProblemResult_ValidationError_HasCorrectStatusInProblemDetails()
    {
        // Arrange
        var error = new Error("VAL001", "Validation error", ErrorType.Validation);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
    }

    [Fact]
    public void ToProblemResult_NotFoundError_HasCorrectStatusInProblemDetails()
    {
        // Arrange
        var error = new Error("NF001", "Not found error", ErrorType.NotFound);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
    }

    [Fact]
    public void ToProblemResult_ConflictError_HasCorrectStatusInProblemDetails()
    {
        // Arrange
        var error = new Error("CF001", "Conflict error", ErrorType.Conflict);

        // Act
        var result = ErrorHttpMapper.ToProblemResult(error);

        // Assert
        var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal(StatusCodes.Status409Conflict, problemDetails.Status);
    }

    #endregion
}

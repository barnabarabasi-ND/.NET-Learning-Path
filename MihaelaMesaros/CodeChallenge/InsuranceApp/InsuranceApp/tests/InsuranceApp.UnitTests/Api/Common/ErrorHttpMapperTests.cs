using InsuranceApp.Api.Common;
using InsuranceApp.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.UnitTests.Api.Common;

public sealed class ErrorHttpMapperTests
{
    [Fact]
    public void ToProblemResult_ValidationError_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        var error = new Error(
            "Test.Validation",
            "Validation error.",
            ErrorType.Validation);

        // Act
        var result = error.ToProblemResult();

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);

        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Validation failed", problemDetails.Title);
        Assert.Equal(error.Description, problemDetails.Detail);
        Assert.Equal(error.Code, problemDetails.Extensions["code"]);
    }

    [Fact]
    public void ToProblemResult_NotFoundError_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        var error = new Error(
            "Test.NotFound",
            "Resource does not exist.",
            ErrorType.NotFound);

        // Act
        var result = error.ToProblemResult();

        // Assert
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);

        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        Assert.Equal("Resource not found", problemDetails.Title);
        Assert.Equal(error.Description, problemDetails.Detail);
        Assert.Equal(error.Code, problemDetails.Extensions["code"]);
    }

    [Fact]
    public void ToProblemResult_ConflictError_ReturnsConflictProblemDetails()
    {
        // Arrange
        var error = new Error(
            "Test.Conflict",
            "Resource conflict.",
            ErrorType.Conflict);

        // Act
        var result = error.ToProblemResult();

        // Assert
        var objectResult = Assert.IsType<ConflictObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);

        Assert.Equal(StatusCodes.Status409Conflict, objectResult.StatusCode);
        Assert.Equal(StatusCodes.Status409Conflict, problemDetails.Status);
        Assert.Equal("Resource conflict", problemDetails.Title);
        Assert.Equal(error.Description, problemDetails.Detail);
        Assert.Equal(error.Code, problemDetails.Extensions["code"]);
    }
}
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniStoreDemo.Api.ExceptionHandling;
using Moq;
using System.Text.Json;

namespace MiniStoreDemo.UnitTests.Api.ExceptionHandling;

public sealed class GlobalExceptionHandlerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly Mock<ILogger<GlobalExceptionHandler>> _mockLogger;
    private readonly GlobalExceptionHandler _handler;

    public GlobalExceptionHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GlobalExceptionHandler>>();
        _handler = new GlobalExceptionHandler(_mockLogger.Object);
    }

    #region TryHandleAsync - Core Behavior

    [Fact]
    public async Task TryHandleAsync_WithAnyException_ReturnsTrue()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TryHandleAsync_SetsStatusCodeTo500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WritesProblemDetailsToResponse()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        Assert.Contains("Internal Server Error", responseBody);
    }

    [Fact]
    public async Task TryHandleAsync_ProblemDetailsHasCorrectStatus()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body, JsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemDetails.Status);
    }

    [Fact]
    public async Task TryHandleAsync_ProblemDetailsHasCorrectTitle()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body, JsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal("Internal Server Error", problemDetails.Title);
    }

    [Fact]
    public async Task TryHandleAsync_ProblemDetailsHasGenericDetail()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Sensitive database connection string here");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body, JsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal("An unexpected error occurred.", problemDetails.Detail);
        Assert.DoesNotContain("Sensitive", problemDetails.Detail);
    }

    #endregion

    #region TryHandleAsync - Logging

    [Fact]
    public async Task TryHandleAsync_LogsException()
    {
        // Arrange
        var context = CreateHttpContext("/api/test", "GET");
        var exception = new Exception("Test exception");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task TryHandleAsync_LogsRequestMethodAndPath()
    {
        // Arrange
        var context = CreateHttpContext("/api/products", "POST");
        var exception = new Exception("Test exception");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, type) =>
                    state.ToString()!.Contains("POST") && state.ToString()!.Contains("/api/products")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region TryHandleAsync - Different Exception Types

    [Fact]
    public async Task TryHandleAsync_WithArgumentNullException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();

#pragma warning disable S3928 // Instantiate argument exceptions correctly - this is a test creating a sample exception
        var exception = new ArgumentNullException("paramName");
#pragma warning restore S3928

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithInvalidOperationException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("Invalid operation");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithNullReferenceException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new NullReferenceException("Object reference not set");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithAggregateException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var innerExceptions = new[] { new Exception("Inner 1"), new Exception("Inner 2") };
        var exception = new AggregateException("Multiple errors", innerExceptions);

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithTimeoutException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new TimeoutException("Operation timed out");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithCustomException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new CustomTestException("Custom error");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    #endregion

    #region TryHandleAsync - Edge Cases

    [Fact]
    public async Task TryHandleAsync_WithExceptionWithInnerException_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var innerException = new InvalidOperationException("Inner");
        var exception = new Exception("Outer", innerException);

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithDeeplyNestedInnerExceptions_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var inner1 = new Exception("Level 1");
        var inner2 = new Exception("Level 2", inner1);
        var inner3 = new Exception("Level 3", inner2);
        var exception = new Exception("Outer", inner3);

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithEmptyExceptionMessage_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithExceptionContainingSpecialCharacters_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("<script>alert('xss')</script> & \"quotes\"");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithExceptionContainingUnicode_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("错误信息 🔥");

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithVeryLongExceptionMessage_Returns500()
    {
        // Arrange
        var context = CreateHttpContext();
        var longMessage = new string('x', 100000);
        var exception = new Exception(longMessage);

        // Act
        await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    #endregion

    #region TryHandleAsync - Request Context Variations

    [Fact]
    public async Task TryHandleAsync_WithGetRequest_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("/api/products", "GET");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WithPostRequest_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("/api/products", "POST");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TryHandleAsync_WithPutRequest_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("/api/products/1", "PUT");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TryHandleAsync_WithDeleteRequest_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("/api/products/1", "DELETE");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TryHandleAsync_WithPatchRequest_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("/api/products/1", "PATCH");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TryHandleAsync_WithEmptyPath_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("", "GET");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TryHandleAsync_WithQueryStringPath_HandlesCorrectly()
    {
        // Arrange
        var context = CreateHttpContext("/api/products?page=1&size=10", "GET");
        var exception = new Exception("Error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region TryHandleAsync - Cancellation Token

    [Fact]
    public async Task TryHandleAsync_WithCancellationToken_PassesToResponse()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Error");
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _handler.TryHandleAsync(context, exception, cts.Token);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region Helper Methods

    private static DefaultHttpContext CreateHttpContext(string path = "/api/test", string method = "GET")
    {
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.Method = method;
        context.Response.Body = new MemoryStream();
        return context;
    }

    public class CustomTestException(string message) : Exception(message);

    #endregion
}

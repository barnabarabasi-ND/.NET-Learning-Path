using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Api.Controllers;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Application.Services;
using Moq;

namespace MiniStoreDemo.UnitTests.Api.Controllers;

public sealed class AuthorizeControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthorizeController _controller;

    public AuthorizeControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthorizeController(_mockAuthService.Object);
    }

    #region LoginAsync - Success Cases

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password123" };
        var loginResponse = new LoginResponseDto
        {
            AccessToken = "valid-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(loginResponse, okResult.Value);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponseDto()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password123" };
        var expectedToken = "jwt-token-here";
        var expectedExpiry = DateTime.UtcNow.AddHours(2);
        var loginResponse = new LoginResponseDto
        {
            AccessToken = expectedToken,
            ExpiresAt = expectedExpiry
        };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal(expectedToken, returnedResponse.AccessToken);
        Assert.Equal(expectedExpiry, returnedResponse.ExpiresAt);
    }

    [Fact]
    public async Task LoginAsync_CallsAuthServiceWithCorrectParameters()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password123" };
        var cancellationToken = new CancellationToken();
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, cancellationToken))
            .ReturnsAsync(new LoginResponseDto { AccessToken = "token", ExpiresAt = DateTime.UtcNow });

        // Act
        await _controller.LoginAsync(loginDto, cancellationToken);

        // Assert
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, cancellationToken), Times.Once);
    }

    #endregion

    #region LoginAsync - Failure Cases

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsUnauthorizedResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "invaliduser", Password = "wrongpassword" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_WhenServiceReturnsNull_ReturnsUnauthorizedResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_ReturnsUnauthorizedResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "nonexistent", Password = "anypassword" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    #endregion

    #region LoginAsync - Edge Cases

    [Fact]
    public async Task LoginAsync_WithEmptyUsername_CallsServiceAndReturnsResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "", Password = "password123" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedResult>(result.Result);
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyPassword_CallsServiceAndReturnsResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_WithWhitespaceUsername_CallsService()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "   ", Password = "password123" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithSpecialCharactersInCredentials_CallsService()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user@domain.com", Password = "P@$$w0rd!#$%" };
        var loginResponse = new LoginResponseDto { AccessToken = "token", ExpiresAt = DateTime.UtcNow };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_WithUnicodeCharactersInCredentials_CallsService()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "用户名", Password = "密码123" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithVeryLongUsername_CallsService()
    {
        // Arrange
        var longUsername = new string('a', 1000);
        var loginDto = new LoginDto { Username = longUsername, Password = "password" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithVeryLongPassword_CallsService()
    {
        // Arrange
        var longPassword = new string('x', 1000);
        var loginDto = new LoginDto { Username = "testuser", Password = longPassword };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        await _controller.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Cancellation Token Tests

    [Fact]
    public async Task LoginAsync_PassesCancellationTokenToService()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        using var cts = new CancellationTokenSource();
        var token = cts.Token;
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, token))
            .ReturnsAsync(new LoginResponseDto { AccessToken = "token", ExpiresAt = DateTime.UtcNow });

        // Act
        await _controller.LoginAsync(loginDto, token);

        // Assert
        _mockAuthService.Verify(s => s.LoginAsync(loginDto, token), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithCancelledToken_PropagatesCancellation()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, cts.Token))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _controller.LoginAsync(loginDto, cts.Token));
    }

    #endregion

    #region Service Exception Tests

    [Fact]
    public async Task LoginAsync_WhenServiceThrows_PropagatesException()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.LoginAsync(loginDto, CancellationToken.None));
    }

    [Fact]
    public async Task LoginAsync_WhenServiceThrowsInvalidOperationException_PropagatesException()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.LoginAsync(loginDto, CancellationToken.None));
    }

    #endregion
}

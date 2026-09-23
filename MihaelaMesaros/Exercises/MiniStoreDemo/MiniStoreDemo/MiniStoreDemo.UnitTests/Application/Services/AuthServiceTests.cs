using Microsoft.AspNetCore.Identity;
using MiniStoreDemo.Application.Abstractions.Authentication;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Application.Services;
using MiniStoreDemo.Domain.Entities;
using Moq;

namespace MiniStoreDemo.UnitTests.Application.Services;

public sealed class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        _service = new AuthService(
            _mockUserRepository.Object,
            _mockTokenService.Object,
            _mockPasswordHasher.Object);
    }

    #region LoginAsync - Happy Path Tests

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponseDto()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password123" };
        var user = CreateActiveUser("testuser");
        var expectedResponse = new LoginResponseDto
        {
            AccessToken = "jwt_token",
            ExpiresAt = DateTime.UtcNow.AddMinutes(30)
        };

        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "password123"))
            .Returns(PasswordVerificationResult.Success);
        _mockTokenService
            .Setup(t => t.GenerateToken(user))
            .Returns(expectedResponse);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.AccessToken, result.AccessToken);
        Assert.Equal(expectedResponse.ExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_TrimsUsername()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "  testuser  ", Password = "password123" };
        var user = CreateActiveUser("testuser");
        var expectedResponse = new LoginResponseDto { AccessToken = "token", ExpiresAt = DateTime.UtcNow };

        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "password123"))
            .Returns(PasswordVerificationResult.Success);
        _mockTokenService
            .Setup(t => t.GenerateToken(user))
            .Returns(expectedResponse);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _mockUserRepository.Verify(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithRehashNeeded_StillReturnsToken()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password123" };
        var user = CreateActiveUser("testuser");
        var expectedResponse = new LoginResponseDto { AccessToken = "token", ExpiresAt = DateTime.UtcNow };

        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "password123"))
            .Returns(PasswordVerificationResult.SuccessRehashNeeded);
        _mockTokenService
            .Setup(t => t.GenerateToken(user))
            .Returns(expectedResponse);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region LoginAsync - User Not Found Tests

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_DoesNotVerifyPassword()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockPasswordHasher.Verify(
            h => h.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_DoesNotGenerateToken()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region LoginAsync - Inactive User Tests

    [Fact]
    public async Task LoginAsync_WithInactiveUser_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "inactiveuser", Password = "password123" };
        var user = CreateInactiveUser("inactiveuser");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("inactiveuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_DoesNotVerifyPassword()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "inactiveuser", Password = "password123" };
        var user = CreateInactiveUser("inactiveuser");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("inactiveuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockPasswordHasher.Verify(
            h => h.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_DoesNotGenerateToken()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "inactiveuser", Password = "password123" };
        var user = CreateInactiveUser("inactiveuser");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("inactiveuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region LoginAsync - Invalid Password Tests

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
        var user = CreateActiveUser("testuser");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "wrongpassword"))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_DoesNotGenerateToken()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
        var user = CreateActiveUser("testuser");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "wrongpassword"))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region LoginAsync - Edge Cases

    [Fact]
    public async Task LoginAsync_WithEmptyUsername_CallsRepositoryWithEmptyString()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(r => r.GetByUsernameAsync("", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithWhitespaceOnlyUsername_TrimsToEmpty()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "   ", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(r => r.GetByUsernameAsync("", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyPassword_VerifiesEmptyPassword()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "" };
        var user = CreateActiveUser("testuser");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, ""))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockPasswordHasher.Verify(h => h.VerifyHashedPassword(user, user.PasswordHash, ""), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithCaseSensitiveUsername_PassesOriginalCase()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "TestUser", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("TestUser", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(r => r.GetByUsernameAsync("TestUser", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithSpecialCharactersInUsername_PassesThrough()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user@email.com!#$", Password = "password123" };
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("user@email.com!#$", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(r => r.GetByUsernameAsync("user@email.com!#$", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithUnicodeCharacters_PassesThrough()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "用户名", Password = "密码" };
        var user = CreateActiveUser("用户名");
        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("用户名", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "密码"))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var result = await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(r => r.GetByUsernameAsync("用户名", It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region LoginAsync - Verification Order Tests

    [Fact]
    public async Task LoginAsync_VerifiesUserExistsBeforeCheckingPassword()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password123" };
        var callOrder = new List<string>();

        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("GetByUsername"))
            .ReturnsAsync((User?)null);

        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback(() => callOrder.Add("VerifyPassword"))
            .Returns(PasswordVerificationResult.Success);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Single(callOrder);
        Assert.Equal("GetByUsername", callOrder[0]);
    }

    [Fact]
    public async Task LoginAsync_ChecksUserActiveBeforeVerifyingPassword()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "inactiveuser", Password = "password123" };
        var user = CreateInactiveUser("inactiveuser");
        var callOrder = new List<string>();

        _mockUserRepository
            .Setup(r => r.GetByUsernameAsync("inactiveuser", It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("GetByUsername"))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback(() => callOrder.Add("VerifyPassword"))
            .Returns(PasswordVerificationResult.Success);

        // Act
        await _service.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        Assert.Single(callOrder);
        Assert.Equal("GetByUsername", callOrder[0]);
    }

    #endregion

    #region Helper Methods

    private static User CreateActiveUser(string username)
    {
        return new User
        {
            UserId = 1,
            Username = username,
            PasswordHash = "hashed_password",
            Role = "User",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static User CreateInactiveUser(string username)
    {
        return new User
        {
            UserId = 1,
            Username = username,
            PasswordHash = "hashed_password",
            Role = "User",
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion
}

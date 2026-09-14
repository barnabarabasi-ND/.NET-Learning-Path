using Microsoft.Extensions.Configuration;
using MiniStoreDemo.Domain.Entities;
using MiniStoreDemo.Infrastructure.Authentication;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniStoreDemo.UnitTests.Infrastructure.Authentication;

public sealed class JwtTokenServiceTests
{
    private const string ValidIssuer = "test-issuer";
    private const string ValidAudience = "test-audience";
    private const string ValidKey = "12345678901234567890123456789012"; // 32 chars for HMAC256

    #region GenerateToken - Happy Path Tests

    [Fact]
    public void GenerateToken_WithValidUser_ReturnsNonNullResponse()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
    }

    [Fact]
    public void GenerateToken_WithValidUser_ReturnsValidJwtToken()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(result.AccessToken));
    }

    [Fact]
    public void GenerateToken_WithValidUser_SetsCorrectIssuer()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        Assert.Equal(ValidIssuer, token.Issuer);
    }

    [Fact]
    public void GenerateToken_WithValidUser_SetsCorrectAudience()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        Assert.Contains(ValidAudience, token.Audiences);
    }

    [Fact]
    public void GenerateToken_WithValidUser_IncludesUserIdClaim()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(userId: 42);

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.NotNull(claim);
        Assert.Equal("42", claim.Value);
    }

    [Fact]
    public void GenerateToken_WithValidUser_IncludesUsernameClaim()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(username: "testuser");

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        Assert.NotNull(claim);
        Assert.Equal("testuser", claim.Value);
    }

    [Fact]
    public void GenerateToken_WithValidUser_IncludesRoleClaim()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(role: "Admin");

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        Assert.NotNull(claim);
        Assert.Equal("Admin", claim.Value);
    }

    [Fact]
    public void GenerateToken_WithValidUser_SetsExpirationTo30Minutes()
    {
        // Arrange
        var fixedTime = new DateTimeOffset(2026, 6, 15, 10, 0, 0, TimeSpan.Zero);
        var expectedExpiration = fixedTime.UtcDateTime.AddMinutes(30);
        var service = CreateService(fixedTime);
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        Assert.Equal(expectedExpiration, result.ExpiresAt);
    }

    [Fact]
    public void GenerateToken_WithValidUser_TokenExpiresAtMatchesResponse()
    {
        // Arrange
        var fixedTime = new DateTimeOffset(2026, 6, 15, 10, 0, 0, TimeSpan.Zero);
        var service = CreateService(fixedTime);
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        Assert.Equal(result.ExpiresAt, token.ValidTo);
    }

    #endregion

    #region GenerateToken - Different User Roles

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Manager")]
    [InlineData("Guest")]
    public void GenerateToken_WithDifferentRoles_IncludesCorrectRoleClaim(string role)
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(role: role);

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        Assert.NotNull(claim);
        Assert.Equal(role, claim.Value);
    }

    #endregion

    #region GenerateToken - Edge Cases

    [Fact]
    public void GenerateToken_WithEmptyUsername_StillGeneratesToken()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(username: "");

        // Act
        var result = service.GenerateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
    }

    [Fact]
    public void GenerateToken_WithSpecialCharactersInUsername_GeneratesValidToken()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(username: "user@domain.com!#$%");

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(result.AccessToken));
        var token = handler.ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        Assert.Equal("user@domain.com!#$%", claim?.Value);
    }

    [Fact]
    public void GenerateToken_WithLongUsername_GeneratesValidToken()
    {
        // Arrange
        var service = CreateService();
        var longUsername = new string('a', 500);
        var user = CreateUser(username: longUsername);

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(result.AccessToken));
    }

    [Fact]
    public void GenerateToken_WithZeroUserId_IncludesZeroInClaim()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(userId: 0);

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.Equal("0", claim?.Value);
    }

    [Fact]
    public void GenerateToken_WithNegativeUserId_IncludesNegativeInClaim()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(userId: -1);

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.Equal("-1", claim?.Value);
    }

    [Fact]
    public void GenerateToken_WithMaxIntUserId_IncludesMaxIntInClaim()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser(userId: int.MaxValue);

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.Equal(int.MaxValue.ToString(), claim?.Value);
    }

    #endregion

    #region GenerateToken - Configuration Error Cases

    [Fact]
    public void GenerateToken_WhenIssuerNotConfigured_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Audience"] = ValidAudience,
                ["Jwt:Key"] = ValidKey
            })
            .Build();
        var service = new JwtTokenService(config, TimeProvider.System);
        var user = CreateUser();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => service.GenerateToken(user));
        Assert.Contains("Issuer", ex.Message);
    }

    [Fact]
    public void GenerateToken_WhenAudienceNotConfigured_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = ValidIssuer,
                ["Jwt:Key"] = ValidKey
            })
            .Build();
        var service = new JwtTokenService(config, TimeProvider.System);
        var user = CreateUser();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => service.GenerateToken(user));
        Assert.Contains("Audience", ex.Message);
    }

    [Fact]
    public void GenerateToken_WhenKeyNotConfigured_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = ValidIssuer,
                ["Jwt:Audience"] = ValidAudience
            })
            .Build();
        var service = new JwtTokenService(config, TimeProvider.System);
        var user = CreateUser();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => service.GenerateToken(user));
        Assert.Contains("Key", ex.Message);
    }

    [Fact]
    public void GenerateToken_WhenAllConfigMissing_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder().Build();
        var service = new JwtTokenService(config, TimeProvider.System);
        var user = CreateUser();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.GenerateToken(user));
    }

    #endregion

    #region GenerateToken - Time Provider Tests

    [Fact]
    public void GenerateToken_UsesInjectedTimeProvider()
    {
        // Arrange
        var specificTime = new DateTimeOffset(2030, 12, 25, 0, 0, 0, TimeSpan.Zero);
        var service = CreateService(specificTime);
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var expectedExpiration = specificTime.UtcDateTime.AddMinutes(30);
        Assert.Equal(expectedExpiration, result.ExpiresAt);
    }

    [Fact]
    public void GenerateToken_AtMidnight_CalculatesCorrectExpiration()
    {
        // Arrange
        var midnight = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var service = CreateService(midnight);
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var expected = new DateTime(2026, 1, 1, 0, 30, 0, DateTimeKind.Utc);
        Assert.Equal(expected, result.ExpiresAt);
    }

    [Fact]
    public void GenerateToken_AtEndOfDay_CalculatesCorrectExpiration()
    {
        // Arrange
        var endOfDay = new DateTimeOffset(2026, 1, 1, 23, 45, 0, TimeSpan.Zero);
        var service = CreateService(endOfDay);
        var user = CreateUser();

        // Act
        var result = service.GenerateToken(user);

        // Assert
        var expected = new DateTime(2026, 1, 2, 0, 15, 0, DateTimeKind.Utc);
        Assert.Equal(expected, result.ExpiresAt);
    }

    #endregion

    #region Helper Methods

    private static JwtTokenService CreateService(DateTimeOffset? fixedTime = null)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = ValidIssuer,
                ["Jwt:Audience"] = ValidAudience,
                ["Jwt:Key"] = ValidKey
            })
            .Build();

        var timeProvider = fixedTime.HasValue
            ? new FixedTimeProvider(fixedTime.Value)
            : TimeProvider.System;

        return new JwtTokenService(config, timeProvider);
    }

    private static User CreateUser(int userId = 1, string username = "testuser", string role = "User")
    {
        return new User
        {
            UserId = userId,
            Username = username,
            Role = role,
            PasswordHash = "hash",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    #endregion
}

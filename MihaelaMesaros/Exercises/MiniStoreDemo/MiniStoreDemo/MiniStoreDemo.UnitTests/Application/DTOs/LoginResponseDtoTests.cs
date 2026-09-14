using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.UnitTests.Application.DTOs;

public sealed class LoginResponseDtoTests
{
    #region Default Value Tests

    [Fact]
    public void LoginResponseDto_WhenCreated_HasEmptyAccessToken()
    {
        // Act
        var dto = new LoginResponseDto();

        // Assert
        Assert.Equal(string.Empty, dto.AccessToken);
    }

    [Fact]
    public void LoginResponseDto_WhenCreated_HasDefaultExpiresAt()
    {
        // Act
        var dto = new LoginResponseDto();

        // Assert
        Assert.Equal(default(DateTime), dto.ExpiresAt);
    }

    #endregion

    #region Property Get/Set Tests

    [Fact]
    public void AccessToken_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new LoginResponseDto();
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIn0.dozjgNryP4J3jVmNHl0w5N_XgL0n3I9PlFUP0THsR8U";

        // Act
        dto.AccessToken = token;

        // Assert
        Assert.Equal(token, dto.AccessToken);
    }

    [Fact]
    public void ExpiresAt_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new LoginResponseDto();
        var expiresAt = new DateTime(2026, 6, 15, 12, 30, 0, DateTimeKind.Utc);

        // Act
        dto.ExpiresAt = expiresAt;

        // Assert
        Assert.Equal(expiresAt, dto.ExpiresAt);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void LoginResponseDto_ObjectInitializer_SetsAllProperties()
    {
        // Arrange
        var token = "jwt_token_here";
        var expiresAt = DateTime.UtcNow.AddMinutes(30);

        // Act
        var dto = new LoginResponseDto
        {
            AccessToken = token,
            ExpiresAt = expiresAt
        };

        // Assert
        Assert.Equal(token, dto.AccessToken);
        Assert.Equal(expiresAt, dto.ExpiresAt);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void AccessToken_SetToNull_AcceptsNull()
    {
        // Arrange
        var dto = new LoginResponseDto { AccessToken = "initial token" };

        // Act
        dto.AccessToken = null!;

        // Assert
        Assert.Null(dto.AccessToken);
    }

    [Fact]
    public void AccessToken_SetToEmptyString_AcceptsEmpty()
    {
        // Arrange
        var dto = new LoginResponseDto { AccessToken = "initial token" };

        // Act
        dto.AccessToken = "";

        // Assert
        Assert.Equal("", dto.AccessToken);
    }

    [Fact]
    public void AccessToken_WithVeryLongToken_AcceptsValue()
    {
        // Arrange
        var dto = new LoginResponseDto();
        var longToken = new string('x', 10000);

        // Act
        dto.AccessToken = longToken;

        // Assert
        Assert.Equal(longToken, dto.AccessToken);
    }

    [Fact]
    public void ExpiresAt_SetToMinValue_AcceptsValue()
    {
        // Arrange
        var dto = new LoginResponseDto();

        // Act
        dto.ExpiresAt = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, dto.ExpiresAt);
    }

    [Fact]
    public void ExpiresAt_SetToMaxValue_AcceptsValue()
    {
        // Arrange
        var dto = new LoginResponseDto();

        // Act
        dto.ExpiresAt = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, dto.ExpiresAt);
    }

    [Fact]
    public void ExpiresAt_PreservesDateTimeKind()
    {
        // Arrange
        var dto = new LoginResponseDto();
        var utcTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        dto.ExpiresAt = utcTime;

        // Assert
        Assert.Equal(DateTimeKind.Utc, dto.ExpiresAt.Kind);
    }

    #endregion
}

using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.UnitTests.Domain.Entities;

public sealed class UserTests
{
    #region Constructor and Default Values Tests

    [Fact]
    public void User_WhenCreated_HasDefaultUserIdOfZero()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.UserId);
    }

    [Fact]
    public void User_WhenCreated_HasNullUsername()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Null(user.Username);
    }

    [Fact]
    public void User_WhenCreated_HasNullPasswordHash()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Null(user.PasswordHash);
    }

    [Fact]
    public void User_WhenCreated_HasNullRole()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Null(user.Role);
    }

    [Fact]
    public void User_WhenCreated_HasDefaultIsActiveOfFalse()
    {
        // Act
        var user = new User();

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void User_WhenCreated_HasDefaultCreatedAtOfMinValue()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Equal(default(DateTime), user.CreatedAt);
    }

    #endregion

    #region UserId Property Tests

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void UserId_SetVariousValues_ReturnsCorrectValue(int userId)
    {
        // Arrange
        var user = new User();

        // Act
        user.UserId = userId;

        // Assert
        Assert.Equal(userId, user.UserId);
    }

    [Fact]
    public void UserId_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.UserId = 42;

        // Assert
        Assert.Equal(42, user.UserId);
    }

    #endregion

    #region Username Property Tests

    [Fact]
    public void Username_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.Username = "testuser";

        // Assert
        Assert.Equal("testuser", user.Username);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("a")]
    [InlineData("user123")]
    [InlineData("user@example.com")]
    [InlineData("User_Name-123")]
    public void Username_SetVariousValues_ReturnsCorrectValue(string username)
    {
        // Arrange
        var user = new User();

        // Act
        user.Username = username;

        // Assert
        Assert.Equal(username, user.Username);
    }

    [Fact]
    public void Username_SetToNull_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "test" };

        // Act
        user.Username = null!;

        // Assert
        Assert.Null(user.Username);
    }

    [Fact]
    public void Username_WithSpecialCharacters_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var specialUsername = "用户名@example.com!#$%";

        // Act
        user.Username = specialUsername;

        // Assert
        Assert.Equal(specialUsername, user.Username);
    }

    [Fact]
    public void Username_VeryLongValue_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var longUsername = new string('a', 1000);

        // Act
        user.Username = longUsername;

        // Assert
        Assert.Equal(longUsername, user.Username);
        Assert.Equal(1000, user.Username.Length);
    }

    #endregion

    #region PasswordHash Property Tests

    [Fact]
    public void PasswordHash_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var hash = "hashed_password_value_abc123";

        // Act
        user.PasswordHash = hash;

        // Assert
        Assert.Equal(hash, user.PasswordHash);
    }

    [Fact]
    public void PasswordHash_SetToNull_ReturnsNull()
    {
        // Arrange
        var user = new User { PasswordHash = "initial_hash" };

        // Act
        user.PasswordHash = null!;

        // Assert
        Assert.Null(user.PasswordHash);
    }

    [Theory]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZRGdjGj/n3.s7e2vZ4K5K5K5K5K5K")] // BCrypt-like
    [InlineData("AQAAAAEAACcQAAAAEDD6hZ2MKkYB7Z3UQ4RYz1234567890abcdef")] // ASP.NET Identity-like
    public void PasswordHash_SetVariousFormats_ReturnsCorrectValue(string hash)
    {
        // Arrange
        var user = new User();

        // Act
        user.PasswordHash = hash;

        // Assert
        Assert.Equal(hash, user.PasswordHash);
    }

    [Fact]
    public void PasswordHash_VeryLongValue_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var longHash = new string('x', 5000);

        // Act
        user.PasswordHash = longHash;

        // Assert
        Assert.Equal(longHash, user.PasswordHash);
    }

    #endregion

    #region Role Property Tests

    [Fact]
    public void Role_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.Role = "Admin";

        // Assert
        Assert.Equal("Admin", user.Role);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Manager")]
    [InlineData("Guest")]
    [InlineData("SuperAdmin")]
    [InlineData("ReadOnly")]
    public void Role_SetCommonRoles_ReturnsCorrectValue(string role)
    {
        // Arrange
        var user = new User();

        // Act
        user.Role = role;

        // Assert
        Assert.Equal(role, user.Role);
    }

    [Fact]
    public void Role_SetToNull_ReturnsNull()
    {
        // Arrange
        var user = new User { Role = "Admin" };

        // Act
        user.Role = null!;

        // Assert
        Assert.Null(user.Role);
    }

    [Fact]
    public void Role_SetToEmpty_ReturnsEmpty()
    {
        // Arrange
        var user = new User();

        // Act
        user.Role = "";

        // Assert
        Assert.Equal("", user.Role);
    }

    [Fact]
    public void Role_CaseSensitive_PreservesCase()
    {
        // Arrange
        var user = new User();

        // Act
        user.Role = "ADMIN";

        // Assert
        Assert.Equal("ADMIN", user.Role);
        Assert.NotEqual("admin", user.Role);
        Assert.NotEqual("Admin", user.Role);
    }

    #endregion

    #region IsActive Property Tests

    [Fact]
    public void IsActive_SetToTrue_ReturnsTrue()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void IsActive_SetToFalse_ReturnsFalse()
    {
        // Arrange
        var user = new User { IsActive = true };

        // Act
        user.IsActive = false;

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void IsActive_Toggle_ChangesValue()
    {
        // Arrange
        var user = new User();

        // Act & Assert
        user.IsActive = true;
        Assert.True(user.IsActive);

        user.IsActive = false;
        Assert.False(user.IsActive);

        user.IsActive = true;
        Assert.True(user.IsActive);
    }

    #endregion

    #region CreatedAt Property Tests

    [Fact]
    public void CreatedAt_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var createdAt = new DateTime(2026, 6, 15, 12, 30, 45, DateTimeKind.Utc);

        // Act
        user.CreatedAt = createdAt;

        // Assert
        Assert.Equal(createdAt, user.CreatedAt);
    }

    [Fact]
    public void CreatedAt_SetToMinValue_ReturnsMinValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.CreatedAt = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, user.CreatedAt);
    }

    [Fact]
    public void CreatedAt_SetToMaxValue_ReturnsMaxValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.CreatedAt = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, user.CreatedAt);
    }

    [Fact]
    public void CreatedAt_PreservesDateTimeKind()
    {
        // Arrange
        var user = new User();
        var utcTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var localTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Local);
        var unspecifiedTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

        // Act & Assert - UTC
        user.CreatedAt = utcTime;
        Assert.Equal(DateTimeKind.Utc, user.CreatedAt.Kind);

        // Act & Assert - Local
        user.CreatedAt = localTime;
        Assert.Equal(DateTimeKind.Local, user.CreatedAt.Kind);

        // Act & Assert - Unspecified
        user.CreatedAt = unspecifiedTime;
        Assert.Equal(DateTimeKind.Unspecified, user.CreatedAt.Kind);
    }

    [Fact]
    public void CreatedAt_PreservesMilliseconds()
    {
        // Arrange
        var user = new User();
        var timeWithMs = new DateTime(2026, 6, 15, 12, 30, 45, 123, DateTimeKind.Utc);

        // Act
        user.CreatedAt = timeWithMs;

        // Assert
        Assert.Equal(123, user.CreatedAt.Millisecond);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void User_ObjectInitializer_SetsAllProperties()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;

        // Act
        var user = new User
        {
            UserId = 1,
            Username = "admin",
            PasswordHash = "hashed_password",
            Role = "Admin",
            IsActive = true,
            CreatedAt = createdAt
        };

        // Assert
        Assert.Equal(1, user.UserId);
        Assert.Equal("admin", user.Username);
        Assert.Equal("hashed_password", user.PasswordHash);
        Assert.Equal("Admin", user.Role);
        Assert.True(user.IsActive);
        Assert.Equal(createdAt, user.CreatedAt);
    }

    [Fact]
    public void User_PartialInitialization_LeavesOtherPropertiesDefault()
    {
        // Act
        var user = new User
        {
            Username = "testuser",
            Role = "User"
        };

        // Assert
        Assert.Equal(0, user.UserId);
        Assert.Equal("testuser", user.Username);
        Assert.Null(user.PasswordHash);
        Assert.Equal("User", user.Role);
        Assert.False(user.IsActive);
        Assert.Equal(default(DateTime), user.CreatedAt);
    }

    #endregion

    #region Type Information Tests

    [Fact]
    public void User_HasExpectedTypeInfo()
    {
        // Arrange
        var user = new User();
        var type = user.GetType();

        // Assert
        Assert.True(type.IsPublic);
        Assert.Equal("User", type.Name);
        Assert.Equal("MiniStoreDemo.Domain.Entities", type.Namespace);
    }

    [Fact]
    public void User_IsNotPartialClass()
    {
        // Arrange - User is not declared as partial (unlike Category and Product)
        var type = typeof(User);

        // Assert - Just verify it's a regular class
        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
        Assert.False(type.IsSealed);
    }

    [Fact]
    public void User_HasExpectedProperties()
    {
        // Arrange
        var type = typeof(User);

        // Assert
        Assert.NotNull(type.GetProperty(nameof(User.UserId)));
        Assert.NotNull(type.GetProperty(nameof(User.Username)));
        Assert.NotNull(type.GetProperty(nameof(User.PasswordHash)));
        Assert.NotNull(type.GetProperty(nameof(User.Role)));
        Assert.NotNull(type.GetProperty(nameof(User.IsActive)));
        Assert.NotNull(type.GetProperty(nameof(User.CreatedAt)));
    }

    [Fact]
    public void User_PropertyCount_IsSix()
    {
        // Arrange
        var type = typeof(User);
        var properties = type.GetProperties();

        // Assert
        Assert.Equal(6, properties.Length);
    }

    #endregion

    #region Equality and Reference Tests

    [Fact]
    public void User_TwoInstancesWithSameValues_AreNotEqual()
    {
        // Arrange - Default reference equality
        var user1 = new User { UserId = 1, Username = "test" };
        var user2 = new User { UserId = 1, Username = "test" };

        // Assert
        Assert.NotSame(user1, user2);
        Assert.False(ReferenceEquals(user1, user2));
    }

    [Fact]
    public void User_SameReference_IsEqual()
    {
        // Arrange
        var user1 = new User { UserId = 1, Username = "test" };
        var user2 = user1;

        // Assert
        Assert.Same(user1, user2);
        Assert.True(ReferenceEquals(user1, user2));
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void User_AllPropertiesNull_DoesNotThrow()
    {
        // Arrange & Act
        var user = new User
        {
            Username = null!,
            PasswordHash = null!,
            Role = null!
        };

        // Assert
        Assert.Null(user.Username);
        Assert.Null(user.PasswordHash);
        Assert.Null(user.Role);
    }

    [Fact]
    public void User_PropertiesWithWhitespace_PreservesWhitespace()
    {
        // Arrange
        var user = new User
        {
            Username = "   ",
            PasswordHash = "\t\n",
            Role = " Admin "
        };

        // Assert
        Assert.Equal("   ", user.Username);
        Assert.Equal("\t\n", user.PasswordHash);
        Assert.Equal(" Admin ", user.Role);
    }

    [Fact]
    public void User_UnicodeCharacters_HandledCorrectly()
    {
        // Arrange
        var user = new User
        {
            Username = "用户名",
            PasswordHash = "密码哈希值",
            Role = "管理员"
        };

        // Assert
        Assert.Equal("用户名", user.Username);
        Assert.Equal("密码哈希值", user.PasswordHash);
        Assert.Equal("管理员", user.Role);
    }

    #endregion
}

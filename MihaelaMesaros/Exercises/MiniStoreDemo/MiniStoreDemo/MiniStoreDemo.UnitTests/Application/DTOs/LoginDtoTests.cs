using System.ComponentModel.DataAnnotations;
using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.UnitTests.Application.DTOs;

public sealed class LoginDtoTests
{
    #region Default Value Tests

    [Fact]
    public void LoginDto_WhenCreated_HasEmptyUsername()
    {
        // Act
        var dto = new LoginDto();

        // Assert
        Assert.Equal(string.Empty, dto.Username);
    }

    [Fact]
    public void LoginDto_WhenCreated_HasEmptyPassword()
    {
        // Act
        var dto = new LoginDto();

        // Assert
        Assert.Equal(string.Empty, dto.Password);
    }

    #endregion

    #region Property Get/Set Tests

    [Fact]
    public void Username_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new LoginDto();

        // Act
        dto.Username = "testuser";

        // Assert
        Assert.Equal("testuser", dto.Username);
    }

    [Fact]
    public void Password_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new LoginDto();

        // Act
        dto.Password = "password123";

        // Assert
        Assert.Equal("password123", dto.Password);
    }

    #endregion

    #region Validation Attribute Tests

    [Fact]
    public void Username_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(LoginDto).GetProperty(nameof(LoginDto.Username));
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

        // Assert
        Assert.NotNull(attribute);
    }

    [Fact]
    public void Username_HasRequiredAttribute_WithCustomErrorMessage()
    {
        // Arrange
        var property = typeof(LoginDto).GetProperty(nameof(LoginDto.Username));
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal("Username is required.", attribute.ErrorMessage);
    }

    [Fact]
    public void Username_HasMaxLengthAttribute_100()
    {
        // Arrange
        var property = typeof(LoginDto).GetProperty(nameof(LoginDto.Username));
        var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(100, attribute.Length);
    }

    [Fact]
    public void Password_HasRequiredAttribute()
    {
        // Arrange
        var property = typeof(LoginDto).GetProperty(nameof(LoginDto.Password));
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

        // Assert
        Assert.NotNull(attribute);
    }

    [Fact]
    public void Password_HasRequiredAttribute_WithCustomErrorMessage()
    {
        // Arrange
        var property = typeof(LoginDto).GetProperty(nameof(LoginDto.Password));
        var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal("Password is required.", attribute.ErrorMessage);
    }

    [Fact]
    public void Password_HasMaxLengthAttribute_1000()
    {
        // Arrange
        var property = typeof(LoginDto).GetProperty(nameof(LoginDto.Password));
        var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(1000, attribute.Length);
    }

    #endregion

    #region DataAnnotations Validation Tests

    [Fact]
    public void LoginDto_WithValidData_PassesValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "testuser",
            Password = "password123"
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void LoginDto_WithEmptyUsername_FailsValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "",
            Password = "password123"
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(LoginDto.Username)));
    }

    [Fact]
    public void LoginDto_WithEmptyPassword_FailsValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "testuser",
            Password = ""
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(LoginDto.Password)));
    }

    [Fact]
    public void LoginDto_WithUsernameExceedingMaxLength_FailsValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = new string('x', 101),
            Password = "password123"
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void LoginDto_WithPasswordExceedingMaxLength_FailsValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "testuser",
            Password = new string('x', 1001)
        };
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void LoginDto_ObjectInitializer_SetsAllProperties()
    {
        // Act
        var dto = new LoginDto
        {
            Username = "admin",
            Password = "secret"
        };

        // Assert
        Assert.Equal("admin", dto.Username);
        Assert.Equal("secret", dto.Password);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Username_WithSpecialCharacters_AcceptsValue()
    {
        // Arrange
        var dto = new LoginDto();

        // Act
        dto.Username = "user@email.com!#$";

        // Assert
        Assert.Equal("user@email.com!#$", dto.Username);
    }

    [Fact]
    public void Password_WithSpecialCharacters_AcceptsValue()
    {
        // Arrange
        var dto = new LoginDto();

        // Act
        dto.Password = "P@ss!w0rd#$%^&*()";

        // Assert
        Assert.Equal("P@ss!w0rd#$%^&*()", dto.Password);
    }

    [Fact]
    public void Username_WithUnicodeCharacters_AcceptsValue()
    {
        // Arrange
        var dto = new LoginDto();

        // Act
        dto.Username = "用户名";

        // Assert
        Assert.Equal("用户名", dto.Username);
    }

    #endregion
}

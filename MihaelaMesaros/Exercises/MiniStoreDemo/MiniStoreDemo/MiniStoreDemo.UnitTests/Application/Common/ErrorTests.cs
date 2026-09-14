using MiniStoreDemo.Application.Common;

namespace MiniStoreDemo.UnitTests.Application.Common;

public sealed class ErrorTests
{
    #region Error Record Tests

    [Fact]
    public void Error_WhenCreated_HasCorrectCode()
    {
        // Act
        var error = new Error("TestCode", "Test Description", ErrorType.Validation);

        // Assert
        Assert.Equal("TestCode", error.Code);
    }

    [Fact]
    public void Error_WhenCreated_HasCorrectDescription()
    {
        // Act
        var error = new Error("TestCode", "Test Description", ErrorType.Validation);

        // Assert
        Assert.Equal("Test Description", error.Description);
    }

    [Fact]
    public void Error_WhenCreated_HasCorrectType()
    {
        // Act
        var error = new Error("TestCode", "Test Description", ErrorType.NotFound);

        // Assert
        Assert.Equal(ErrorType.NotFound, error.Type);
    }

    [Theory]
    [InlineData(ErrorType.Validation)]
    [InlineData(ErrorType.NotFound)]
    [InlineData(ErrorType.Conflict)]
    public void Error_WithDifferentTypes_ReturnsCorrectType(ErrorType errorType)
    {
        // Act
        var error = new Error("Code", "Description", errorType);

        // Assert
        Assert.Equal(errorType, error.Type);
    }

    [Fact]
    public void Error_WithEmptyCode_AcceptsEmptyString()
    {
        // Act
        var error = new Error("", "Description", ErrorType.Validation);

        // Assert
        Assert.Equal("", error.Code);
    }

    [Fact]
    public void Error_WithEmptyDescription_AcceptsEmptyString()
    {
        // Act
        var error = new Error("Code", "", ErrorType.Validation);

        // Assert
        Assert.Equal("", error.Description);
    }

    [Fact]
    public void Error_WithSpecialCharacters_PreservesCharacters()
    {
        // Arrange
        var specialCode = "Error.Code_123!@#";
        var specialDescription = "Error with special chars: <>&\"'日本語";

        // Act
        var error = new Error(specialCode, specialDescription, ErrorType.Validation);

        // Assert
        Assert.Equal(specialCode, error.Code);
        Assert.Equal(specialDescription, error.Description);
    }

    #endregion

    #region Error Record Equality Tests

    [Fact]
    public void Error_TwoErrorsWithSameValues_AreEqual()
    {
        // Arrange
        var error1 = new Error("Code", "Description", ErrorType.Validation);
        var error2 = new Error("Code", "Description", ErrorType.Validation);

        // Assert
        Assert.Equal(error1, error2);
    }

    [Fact]
    public void Error_TwoErrorsWithDifferentCodes_AreNotEqual()
    {
        // Arrange
        var error1 = new Error("Code1", "Description", ErrorType.Validation);
        var error2 = new Error("Code2", "Description", ErrorType.Validation);

        // Assert
        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Error_TwoErrorsWithDifferentDescriptions_AreNotEqual()
    {
        // Arrange
        var error1 = new Error("Code", "Description1", ErrorType.Validation);
        var error2 = new Error("Code", "Description2", ErrorType.Validation);

        // Assert
        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Error_TwoErrorsWithDifferentTypes_AreNotEqual()
    {
        // Arrange
        var error1 = new Error("Code", "Description", ErrorType.Validation);
        var error2 = new Error("Code", "Description", ErrorType.NotFound);

        // Assert
        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Error_GetHashCode_SameForEqualErrors()
    {
        // Arrange
        var error1 = new Error("Code", "Description", ErrorType.Validation);
        var error2 = new Error("Code", "Description", ErrorType.Validation);

        // Assert
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    #endregion

    #region Error Record Deconstruction Tests

    [Fact]
    public void Error_CanBeDeconstructed()
    {
        // Arrange
        var error = new Error("TestCode", "Test Description", ErrorType.Conflict);

        // Act
        var (code, description, type) = error;

        // Assert
        Assert.Equal("TestCode", code);
        Assert.Equal("Test Description", description);
        Assert.Equal(ErrorType.Conflict, type);
    }

    #endregion

    #region Error Record With-Expression Tests

    [Fact]
    public void Error_WithExpression_CreatesNewErrorWithModifiedCode()
    {
        // Arrange
        var original = new Error("OriginalCode", "Description", ErrorType.Validation);

        // Act
        var modified = original with { Code = "ModifiedCode" };

        // Assert
        Assert.Equal("ModifiedCode", modified.Code);
        Assert.Equal("Description", modified.Description);
        Assert.Equal(ErrorType.Validation, modified.Type);
    }

    [Fact]
    public void Error_WithExpression_CreatesNewErrorWithModifiedDescription()
    {
        // Arrange
        var original = new Error("Code", "Original Description", ErrorType.Validation);

        // Act
        var modified = original with { Description = "Modified Description" };

        // Assert
        Assert.Equal("Code", modified.Code);
        Assert.Equal("Modified Description", modified.Description);
    }

    [Fact]
    public void Error_WithExpression_CreatesNewErrorWithModifiedType()
    {
        // Arrange
        var original = new Error("Code", "Description", ErrorType.Validation);

        // Act
        var modified = original with { Type = ErrorType.NotFound };

        // Assert
        Assert.Equal(ErrorType.NotFound, modified.Type);
    }

    [Fact]
    public void Error_WithExpression_OriginalRemainsUnchanged()
    {
        // Arrange
        var original = new Error("OriginalCode", "Original Description", ErrorType.Validation);

        // Act
        var modified = original with { Code = "ModifiedCode" };

        // Assert
        Assert.Equal("OriginalCode", original.Code);
        Assert.NotSame(original, modified);
    }

    #endregion
}

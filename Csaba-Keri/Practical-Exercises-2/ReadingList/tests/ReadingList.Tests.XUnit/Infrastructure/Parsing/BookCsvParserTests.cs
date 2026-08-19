using ReadingList.Infrastructure.Parsing;
using ReadingList.Tests.Common.TestData;

namespace ReadingList.Tests.XUnit.Infrastructure.Parsing;

public sealed class BookCsvParserTests
{
    private readonly BookCsvParser _parser = new();

    [Fact]
    public void Parse_WithValidRow_ReturnsBook()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.ValidBookRow);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(1, result.Value.Id);
        Assert.Equal("Clean Code", result.Value.Title);
        Assert.Equal("Robert C. Martin", result.Value.Author);
        Assert.Equal(2008, result.Value.Year);
        Assert.Equal(464, result.Value.Pages);
        Assert.Equal("software", result.Value.Genre);
        Assert.True(result.Value.Finished);
        Assert.Equal(5m, result.Value.Rating);
    }

    [Fact]
    public void Parse_WithCommaInsideQuotedTitle_ParsesTitleCorrectly()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.RowWithCommaInTitle);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(
            "Clean Code, Second Edition",
            result.Value.Title
        );
    }

    [Fact]
    public void Parse_WithRatingOutsideAllowedRange_ReturnsFailure()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.InvalidRatingRow);

        // Assert
        Assert.True(result.IsFailure);

        Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
    }

    [Fact]
    public void Parse_WithMalformedYear_ReturnsFailure()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.MalformedYearRow);

        // Assert
        Assert.True(result.IsFailure);

        Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
    }
}

using ReadingList.Infrastructure.Parsing;
using ReadingList.Tests.Common.TestData;

namespace ReadingList.Tests.NUnit.Infrastructure.Parsing;

internal sealed class BookCsvParserTests
{
    private BookCsvParser _parser;

    [SetUp]
    public void SetUp()
    {
        _parser = new BookCsvParser();
    }

    [Test]
    public void Parse_WithValidRow_ReturnsBook()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.ValidBookRow);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);

            Assert.That(result.Value.Id, Is.EqualTo(1));
            Assert.That(result.Value.Title, Is.EqualTo("Clean Code"));
            Assert.That(result.Value.Author, Is.EqualTo("Robert C. Martin"));
            Assert.That(result.Value.Year, Is.EqualTo(2008));
            Assert.That(result.Value.Pages, Is.EqualTo(464));
            Assert.That(result.Value.Genre, Is.EqualTo("software"));
            Assert.That(result.Value.Finished, Is.True);
            Assert.That(result.Value.Rating, Is.EqualTo(5m));
        });
    }

    [Test]
    public void Parse_WithCommaInsideQuotedTitle_ParsesTitleCorrectly()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.RowWithCommaInTitle);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);

            Assert.That(
                result.Value.Title,
                Is.EqualTo("Clean Code, Second Edition")
            );
        });
    }

    [Test]
    public void Parse_WithRatingOutsideAllowedRange_ReturnsFailure()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.InvalidRatingRow);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);

            Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
        });
    }

    [Test]
    public void Parse_WithMalformedYear_ReturnsFailure()
    {
        // Act
        var result = _parser.Parse(BookCsvTestData.MalformedYearRow);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);

            Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
        });
    }
}

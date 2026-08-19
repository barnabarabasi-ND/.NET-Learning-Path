using ReadingList.Tests.Common.TestData;

namespace ReadingList.Tests.XUnit.Domain.Entities;

public sealed class BookTests
{
    [Fact]
    public void SetRating_WithValidRating_UpdatesRating()
    {
        // Arrange
        var book = new BookBuilder().Build();
        var validRating = 4.5m;

        // Act
        book.SetRating(validRating);

        // Assert
        Assert.Equal(validRating, book.Rating);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5.1)]
    public void SetRating_WithRatingOutsideAllowedRange_Throws(decimal invalidRating)
    {
        // Arrange
        var book = new BookBuilder().Build();

        // Act
        void setInvalidRating() => book.SetRating(invalidRating);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(setInvalidRating);
    }

    [Fact]
    public void MarkAsFinished_SetsFinishedToTrue()
    {
        // Arrange
        var book = new BookBuilder()
            .WithFinished(false)
            .Build();

        // Act
        book.MarkAsFinished();

        // Assert
        Assert.True(book.Finished);
    }
}

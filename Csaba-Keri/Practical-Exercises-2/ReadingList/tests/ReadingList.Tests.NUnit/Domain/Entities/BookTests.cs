using ReadingList.Tests.Common.TestData;

namespace ReadingList.Tests.NUnit.Domain.Entities;

internal sealed class BookTests
{
    [Test]
    public void SetRating_WithValidRating_UpdatesRating()
    {
        // Arrange
        var book = new BookBuilder().Build();
        var validRating = 4.5m;

        // Act
        book.SetRating(validRating);

        // Assert
        Assert.That(book.Rating, Is.EqualTo(validRating));
    }

    [TestCase(-1)]
    [TestCase(5.1)]
    public void SetRating_WithRatingOutsideAllowedRange_Throws(decimal invalidRating)
    {
        // Arrange
        var book = new BookBuilder().Build();

        // Act
        void setInvalidRating() => book.SetRating(invalidRating);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(setInvalidRating);
    }

    [Test]
    public void MarkAsFinished_SetsFinishedToTrue()
    {
        // Arrange
        var book = new BookBuilder()
            .WithFinished(false)
            .Build();

        // Act
        book.MarkAsFinished();

        // Assert
        Assert.That(book.Finished, Is.True);
    }
}

using ReadingList.Tests.Common.Factories;
using ReadingList.Tests.Common.TestData;

namespace ReadingList.Tests.XUnit.Infrastructure.Repositories;

public sealed class InMemoryRepositoryTests
{
    [Fact]
    public void Add_WithNewKey_ReturnsTrueAndStoresItem()
    {
        // Arrange
        var repository = InMemoryRepositoryFactory.CreateBookRepository();
        var book = new BookBuilder().Build();

        // Act
        var added = repository.Add(book);

        // Assert
        Assert.True(added);
        Assert.Same(book, repository.GetById(book.Id));
    }

    [Fact]
    public void Add_WithDuplicateKey_ReturnsFalseAndKeepsFirstItem()
    {
        // Arrange
        var repository = InMemoryRepositoryFactory.CreateBookRepository();

        var bookId = 1;

        var firstBook = new BookBuilder()
            .WithId(bookId)
            .WithTitle("First Book")
            .Build();

        var duplicateBook = new BookBuilder()
            .WithId(bookId)
            .WithTitle("Duplicate Book")
            .Build();

        repository.Add(firstBook);

        // Act
        var duplicateAdded = repository.Add(duplicateBook);

        // Assert
        Assert.False(duplicateAdded);
        Assert.Same(firstBook, repository.GetById(bookId));
    }

    [Fact]
    public void Upsert_WithExistingKey_ReplacesItem()
    {
        // Arrange
        var repository = InMemoryRepositoryFactory.CreateBookRepository();

        var bookId = 1;

        var originalBook = new BookBuilder()
            .WithId(bookId)
            .WithTitle("Original Book")
            .Build();

        var updatedBook = new BookBuilder()
            .WithId(bookId)
            .WithTitle("Updated")
            .Build();

        repository.Upsert(originalBook);

        // Act
        repository.Upsert(updatedBook);

        // Assert
        Assert.Same(updatedBook, repository.GetById(bookId));
    }

    [Fact]
    public void GetById_WithUnknownKey_ReturnsNull()
    {
        // Arrange
        var repository = InMemoryRepositoryFactory.CreateBookRepository();

        // Act
        var result = repository.GetById(1);

        // Assert
        Assert.Null(result);
    }
}

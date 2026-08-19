using ReadingList.Tests.Common.Factories;
using ReadingList.Tests.Common.TestData;

namespace ReadingList.Tests.NUnit.Infrastructure.Repositories;

internal sealed class InMemoryRepositoryTests
{
    [Test]
    public void Add_WithNewKey_ReturnsTrueAndStoresItem()
    {
        // Arrange
        var repository = InMemoryRepositoryFactory.CreateBookRepository();
        var book = new BookBuilder().Build();

        // Act
        var added = repository.Add(book);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(added, Is.True);
            Assert.That(repository.GetById(book.Id), Is.SameAs(book));
        });
    }

    [Test]
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
        Assert.Multiple(() =>
        {
            Assert.That(duplicateAdded, Is.False);
            Assert.That(repository.GetById(bookId), Is.SameAs(firstBook));
        });
    }

    [Test]
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
        Assert.That(repository.GetById(bookId), Is.SameAs(updatedBook));
    }

    [Test]
    public void GetById_WithUnknownKey_ReturnsNull()
    {
        // Arrange
        var repository = InMemoryRepositoryFactory.CreateBookRepository();

        // Act
        var result = repository.GetById(1);

        // Assert
        Assert.That(result, Is.Null);
    }
}

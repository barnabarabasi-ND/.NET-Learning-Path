using ReadingList.Domain.Entities;
using ReadingList.Infrastructure.Repositories;

namespace ReadingList.Tests.Common.Factories;

public static class InMemoryRepositoryFactory
{
    public static InMemoryRepository<Book, int> CreateBookRepository()
    {
        return new(book => book.Id);
    }
}

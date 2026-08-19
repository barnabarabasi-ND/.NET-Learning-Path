namespace ReadingList.Application.Interfaces.Repositories;

public interface IRepository<T, TKey>
    where T : class
    where TKey : notnull
{
    IReadOnlyCollection<T> GetAll();

    T? GetById(TKey id);

    bool Add(T item);

    void Upsert(T item);
}

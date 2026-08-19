using ReadingList.Application.Interfaces;

namespace ReadingList.Infrastructure.Repositories;

public class InMemoryRepository<T, TKey> : IRepository<T, TKey>
    where T : class
    where TKey: notnull
{
    private readonly Dictionary<TKey, T> _items = [];
    private readonly Func<T, TKey> _keySelector;

    public InMemoryRepository(Func<T, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        _keySelector = keySelector;
    }

    public IReadOnlyCollection<T> GetAll()
    {
        return [.. _items.Values];
    }

    public T? GetById(TKey id)
    {
        return _items.GetValueOrDefault(id);
    }

    public bool Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var key = _keySelector(item);

        return _items.TryAdd(key, item);
    }

    public void Upsert(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var key = _keySelector(item);

        _items[key] = item;
    }
}

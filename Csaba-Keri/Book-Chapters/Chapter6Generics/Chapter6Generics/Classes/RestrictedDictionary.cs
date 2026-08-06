namespace Chapter6Generics.Classes;

internal class RestrictedDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    where TKey : System.Enum
    where TValue : class, new()
{
    public T Make<T>(TKey key) where T : TValue, new()
    {
        var value = new T();
        if (!TryGetValue(key, out var list))
        {
            Add(key, new() { value });
        }
        else
        {
            list.Add(value);
        }

        return value;
    }
}

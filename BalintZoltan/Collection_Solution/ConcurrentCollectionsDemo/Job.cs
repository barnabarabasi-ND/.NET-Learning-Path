namespace ConcurrentCollectionsDemo;
public sealed record Job(
    Guid Id,
    string Name,
    DateTimeOffset CreatedAt);

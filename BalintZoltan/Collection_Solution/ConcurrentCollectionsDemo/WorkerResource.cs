namespace ConcurrentCollectionsDemo;

public sealed class WorkerResource
{
    public required int Id { get; init; }

    public void Reset()
    {
        Console.WriteLine($"Resource {Id} reset.");
    }
}
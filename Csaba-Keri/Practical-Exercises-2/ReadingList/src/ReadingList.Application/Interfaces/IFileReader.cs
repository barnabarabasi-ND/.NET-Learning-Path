namespace ReadingList.Application.Interfaces;

public interface IFileReader
{
    Task<IReadOnlyList<string>> ReadAllLinesAsync(
        string path,
        CancellationToken cancellationToken = default
    );
}

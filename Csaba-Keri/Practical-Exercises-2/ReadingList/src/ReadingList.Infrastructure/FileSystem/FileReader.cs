using ReadingList.Application.Interfaces;

namespace ReadingList.Infrastructure.FileSystem;

public class FileReader : IFileReader
{
    public async Task<IReadOnlyList<string>> ReadAllLinesAsync(
        string path,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return await File
            .ReadAllLinesAsync(path, cancellationToken)
            .ConfigureAwait(false);
    }
}

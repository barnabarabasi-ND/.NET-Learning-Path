using ReadingList.Application.Models;

namespace ReadingList.Application.Interfaces;

public interface IBookImportService
{
    Task<BookImportSummary> ImportAsync(
        IReadOnlyCollection<string> filePaths,
        CancellationToken cancellationToken = default
    );
}

using ReadingList.Domain.Entities;

namespace ReadingList.Application.Models;

public record BookFileImportResult(
    string FilePath,
    IReadOnlyList<Book> Books,
    IReadOnlyList<BookImportWarning> Warnings,
    int MalformedCount,
    bool FileFailed
)
{
    public static BookFileImportResult Success(
        string filePath,
        IReadOnlyList<Book> books,
        IReadOnlyList<BookImportWarning> warnings
    )
    {
        return new(
            FilePath: filePath,
            Books: books,
            Warnings: warnings,
            MalformedCount: warnings.Count,
            FileFailed: false
        );
    }

    public static BookFileImportResult Failed(string filePath, string message)
    {
        var warning = new BookImportWarning(filePath, null, message);

        return new(
            FilePath: filePath,
            Books: [],
            Warnings: [warning],
            MalformedCount: 0,
            FileFailed: true
        );
    }
}

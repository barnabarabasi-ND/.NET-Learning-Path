namespace ReadingList.Application.Models;

public record BookImportSummary(
    int ImportedCount,
    int DuplicateCount,
    int MalformedCount,
    int FailedFileCount,
    IReadOnlyList<int> SkippedDuplicateIds,
    IReadOnlyList<BookImportWarning> Warnings
);

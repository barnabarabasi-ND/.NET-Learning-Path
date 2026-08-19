namespace ReadingList.Application.Models;

public record BookImportWarning(
    string FilePath,
    int? LineNumber,
    string Message
);

using ReadingList.Application.Interfaces;
using ReadingList.Application.Models;
using ReadingList.Domain.Entities;

namespace ReadingList.Infrastructure.Importing;

public class BookImportService : IBookImportService
{
    private readonly IFileReader _fileReader;
    private readonly IBookCsvParser _bookParser;
    private readonly IRepository<Book, int> _bookRepository;

    public BookImportService(IFileReader reader, IBookCsvParser parser, IRepository<Book, int> repository)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(parser);
        ArgumentNullException.ThrowIfNull(repository);

        _fileReader = reader;
        _bookParser = parser;
        _bookRepository = repository;
    }

    public async Task<BookImportSummary> ImportAsync(
        IReadOnlyCollection<string> filePaths,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(filePaths);

        if (filePaths.Count == 0)
        {
            throw new ArgumentException(
                "At least one file path must be provided.",
                nameof(filePaths)
            );
        }

        if (filePaths.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException(
                "File paths cannot be empty.",
                nameof(filePaths)
            );
        }

        var importTasks = filePaths
            .Select(path => ReadAndParseFileAsync(path, cancellationToken))
            .ToArray();

        var fileResults = await Task
            .WhenAll(importTasks)
            .ConfigureAwait(false);

        return MergeResults(fileResults);
    }

    private async Task<BookFileImportResult> ReadAndParseFileAsync(
        string filePath,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var lines = await _fileReader
                .ReadAllLinesAsync(filePath, cancellationToken)
                .ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            if (lines.Count == 0)
            {
                return BookFileImportResult.Failed(filePath, "The file is empty.");
            }

            if (!_bookParser.HasValidHeader(lines[0]))
            {
                return BookFileImportResult.Failed(filePath, "The CSV header is invalid.");
            }

            var books = new List<Book>();
            var warnings = new List<BookImportWarning>();

            for (var currentLineIdx = 1; currentLineIdx < lines.Count; currentLineIdx++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var parseResult = _bookParser.Parse(lines[currentLineIdx]);

                if (parseResult.IsSuccess)
                {
                    books.Add(parseResult.Value);
                    continue;
                }

                warnings.Add(
                    new(
                        FilePath: filePath,
                        LineNumber: currentLineIdx + 1,
                        Message: parseResult.ErrorMessage!
                    )
                );
            }

            return BookFileImportResult.Success(filePath, books, warnings);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (IOException exception)
        {
            return BookFileImportResult.Failed(filePath, exception.Message);
        }
        catch (UnauthorizedAccessException exception)
        {
            return BookFileImportResult.Failed(filePath, exception.Message);
        }
    }

    private BookImportSummary MergeResults(IReadOnlyCollection<BookFileImportResult> fileResults)
    {
        var importedCount = 0;
        var duplicateIds = new List<int>();
        var warnings = new List<BookImportWarning>();
        var malformedCount = 0;
        var failedFileCount = 0;

        foreach (var fileResult in fileResults)
        {
            warnings.AddRange(fileResult.Warnings);
            malformedCount += fileResult.MalformedCount;

            if (fileResult.FileFailed)
            {
                failedFileCount++;
                continue;
            }

            foreach (var book in fileResult.Books)
            {
                if (_bookRepository.Add(book))
                {
                    importedCount++;
                    continue;
                }

                duplicateIds.Add(book.Id);
            }
        }

        return new(
            ImportedCount: importedCount,
            DuplicateCount: duplicateIds.Count,
            MalformedCount: malformedCount,
            FailedFileCount: failedFileCount,
            SkippedDuplicateIds: duplicateIds,
            Warnings: warnings
        );
    }
}

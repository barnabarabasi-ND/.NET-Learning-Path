using ReadingList.Application.Common;
using ReadingList.Application.Interfaces.Parsing;
using ReadingList.Domain.Entities;
using ReadingList.Infrastructure.Parsing.Extensions;
using System.Globalization;

namespace ReadingList.Infrastructure.Parsing;

public class BookCsvParser : IBookParser
{
    private const int ExpectedFieldCount = 8;

    public Result<Book> Parse(string line)
    {
        ArgumentNullException.ThrowIfNull(line, nameof(line));

        if (string.IsNullOrWhiteSpace(line))
        {
            return Result<Book>.Failure("The CSV row cannot be empty.");
        }

        var fieldsResult = CsvLineParser.Parse(line);

        if (fieldsResult.IsFailure)
        {
            return Result<Book>.Failure(fieldsResult.ErrorMessage!);
        }

        var fields = fieldsResult.Value;

        if (fields.Count != ExpectedFieldCount)
        {
            return Result<Book>.Failure(
                $"Expected {ExpectedFieldCount} fields but found {fields.Count}."
            );
        }

        if (!TryParsePositiveInt(fields[0], "Id", out var id, out var errorMessage))
        {
            return Result<Book>.Failure(errorMessage!);
        }

        if (string.IsNullOrWhiteSpace(fields[1])) {
            return Result<Book>.Failure("Title cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(fields[2])) {
            return Result<Book>.Failure("Author cannot be empty.");
        }

        if (!TryParsePositiveInt(fields[3], "Year", out var year, out errorMessage))
        {
            return Result<Book>.Failure(errorMessage!);
        }

        if (!TryParsePositiveInt(fields[4], "Pages", out var pages, out errorMessage))
        {
            return Result<Book>.Failure(errorMessage!);
        }

        if (string.IsNullOrWhiteSpace(fields[5]))
        {
            return Result<Book>.Failure("Genre cannot be empty.");
        }

        if (!fields[6].TryParseFinished(out var finished))
        {
            return Result<Book>.Failure($"Invalid Finished value '{fields[6]}'.");
        }

        if (!decimal.TryParse(fields[7], NumberStyles.Number, CultureInfo.InvariantCulture, out var rating))
        {
            return Result<Book>.Failure($"Invalid Rating value '{fields[7]}'.");
        }

        if (rating is < Book.MinimumRating or > Book.MaximumRating)
        {
            return Result<Book>.Failure(
                $"Rating must be between {Book.MinimumRating} and {Book.MaximumRating}."
            );
        }

        var book = new Book(
            id: id,
            title: fields[1],
            author: fields[2],
            year: year,
            pages: pages,
            genre: fields[5],
            finished: finished,
            rating: rating
        );

        return Result<Book>.Success(book);
    }

    private static bool TryParsePositiveInt(
        string value,
        string fieldName,
        out int result,
        out string? errorMessage
    )
    {
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
        {
            errorMessage = $"Invalid {fieldName} value '{value}'.";
            return false;
        }

        if (result <= 0)
        {
            errorMessage = $"{fieldName} must be greater than zero.";
            return false;
        }

        errorMessage = null;
        return true;
    }
}

using ReadingList.Application.Common;
using ReadingList.Application.Interfaces;
using ReadingList.Domain.Entities;
using ReadingList.Infrastructure.Parsing.Extensions;
using System.Globalization;

namespace ReadingList.Infrastructure.Parsing;

public class BookCsvParser : IBookCsvParser
{
    private static readonly string[] ExpectedHeader = [
        "Id",
        "Title",
        "Author",
        "Year",
        "Pages",
        "Genre",
        "Finished",
        "Rating"
    ];

    public bool HasValidHeader(string headerLine)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headerLine);

        var result = CsvLineParser.Parse(headerLine);

        if (result.IsFailure)
        {
            return false;
        }

        var fields = result.Value;

        return fields.SequenceEqual(ExpectedHeader, StringComparer.OrdinalIgnoreCase);
    }

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

        if (fields.Count != ExpectedHeader.Length)
        {
            return Result<Book>.Failure(
                $"Expected {ExpectedHeader.Length} fields but found {fields.Count}."
            );
        }

        if (!TryParsePositiveInt(fields[0], ExpectedHeader[0], out var id, out var errorMessage))
        {
            return Result<Book>.Failure(errorMessage!);
        }

        if (string.IsNullOrWhiteSpace(fields[1])) {
            return Result<Book>.Failure($"{ExpectedHeader[1]} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(fields[2])) {
            return Result<Book>.Failure($"{ExpectedHeader[2]} cannot be empty.");
        }

        if (!TryParsePositiveInt(fields[3], ExpectedHeader[3], out var year, out errorMessage))
        {
            return Result<Book>.Failure(errorMessage!);
        }

        if (!TryParsePositiveInt(fields[4], ExpectedHeader[4], out var pages, out errorMessage))
        {
            return Result<Book>.Failure(errorMessage!);
        }

        if (string.IsNullOrWhiteSpace(fields[5]))
        {
            return Result<Book>.Failure($"{ExpectedHeader[5]} cannot be empty.");
        }

        if (!fields[6].TryParseFinished(out var finished))
        {
            return Result<Book>.Failure($"Invalid {ExpectedHeader[6]} value '{fields[6]}'.");
        }

        if (!decimal.TryParse(fields[7], NumberStyles.Number, CultureInfo.InvariantCulture, out var rating))
        {
            return Result<Book>.Failure($"Invalid {ExpectedHeader[7]} value '{fields[7]}'.");
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

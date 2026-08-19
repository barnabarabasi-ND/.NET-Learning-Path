using ReadingList.Application.Common;
using ReadingList.Domain.Entities;

namespace ReadingList.Application.Interfaces;

public interface IBookCsvParser
{
    bool HasValidHeader(string headerLine);

    Result<Book> Parse(string line);
}

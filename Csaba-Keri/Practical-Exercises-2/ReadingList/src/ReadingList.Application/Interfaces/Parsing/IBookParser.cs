using ReadingList.Application.Common;
using ReadingList.Domain.Entities;

namespace ReadingList.Application.Interfaces.Parsing;

public interface IBookParser
{
    Result<Book> Parse(string line);
}

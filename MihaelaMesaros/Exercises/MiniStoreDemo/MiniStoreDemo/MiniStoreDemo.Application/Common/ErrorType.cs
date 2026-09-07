namespace MiniStoreDemo.Application.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict
}

public sealed record Error(string Code, string Description, ErrorType Type);

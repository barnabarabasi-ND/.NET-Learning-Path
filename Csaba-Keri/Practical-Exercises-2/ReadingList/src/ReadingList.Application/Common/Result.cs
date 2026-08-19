namespace ReadingList.Application.Common;

public class Result<T>
{
    private readonly T? _value;

    private Result(T value)
    {
        _value = value;
        IsSuccess = true;
        ErrorMessage = null;
    }

    private Result(string errorMessage)
    {
        _value = default;
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? ErrorMessage { get; }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("A failed result does not contain a value.");

    public static Result<T> Success(T value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        return new Result<T>(value);
    }

    public static Result<T> Failure(string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage, nameof(errorMessage));

        return new Result<T>(errorMessage);
    }
}

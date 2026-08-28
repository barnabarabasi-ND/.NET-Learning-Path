using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Application.Common;

namespace MiniStoreDemo.Api.Common;

/// <summary>
/// Provides extension methods to map application errors to appropriate HTTP responses.
/// </summary>
public static class ErrorHttpMapper
{
    /// <summary>
    /// Maps an application error to an appropriate HTTP response based on the error type.
    /// </summary>
    /// <param name="error">The application error to be mapped.</param>
    /// <returns>An <see cref="ObjectResult"/> representing the HTTP response.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the error type is not supported.</exception>
    public static ObjectResult ToProblemResult(Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(CreateProblemDetails(StatusCodes.Status400BadRequest, "Validation failed", error)),

            ErrorType.NotFound => new NotFoundObjectResult(CreateProblemDetails(StatusCodes.Status404NotFound, "Resource not found", error)),

            ErrorType.Conflict => new ConflictObjectResult(CreateProblemDetails(StatusCodes.Status409Conflict, "Resource conflict", error)),

            _ => throw new InvalidOperationException($"Unsupported error type: {error.Type}.")
        };
    }

    private static ProblemDetails CreateProblemDetails(int status, string title, Error error)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = error.Description,
            Extensions =
            {
                ["code"] = error.Code
            }
        };
    }
}
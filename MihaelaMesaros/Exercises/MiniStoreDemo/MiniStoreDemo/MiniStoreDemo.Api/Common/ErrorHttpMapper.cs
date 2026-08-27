using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Application.Common;

namespace MiniStoreDemo.Api.Common;

public static class ErrorHttpMapper
{
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
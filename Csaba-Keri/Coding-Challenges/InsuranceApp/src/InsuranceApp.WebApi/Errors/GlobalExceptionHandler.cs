using FluentValidation;
using InsuranceApp.Application.Clients.Exceptions;
using InsuranceApp.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace InsuranceApp.WebApi.Errors;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var (statusCode, title) = exception switch
        {
            ValidationException =>
                (StatusCodes.Status400BadRequest, "Validation failed."),

            EntityNotFoundException =>
                (StatusCodes.Status404NotFound, "Resource not found."),

            DuplicateClientIdentificationException =>
                (StatusCodes.Status409Conflict, "Client already exists."),

            _ =>
                (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unexpected error while processing {Path}.",
                httpContext.Request.Path
            );
        }

        await Results.Problem(
            statusCode: statusCode,
            title: title,
            detail: exception.Message,
            instance: httpContext.Request.Path
        ).ExecuteAsync(httpContext);

        return true;
    }
}

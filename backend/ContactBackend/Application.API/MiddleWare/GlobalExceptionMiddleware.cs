using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ContactBackend.Application.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            LogValidationException(validationException);
            await HandleValidationExceptionAsync(context, validationException);
        }
        catch (Exception exception)
        {
            LogSystemException(exception);
            await HandleSystemExceptionAsync(context, exception);
        }
    }

    private void LogValidationException(ValidationException exception)
    {
        _logger.LogWarning(exception, "A validation exception occurred.");
    }

    private void LogSystemException(Exception exception)
    {
        _logger.LogError(exception, "An unhandled system exception occurred.");
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        ConfigureValidationJsonResponseHeaders(context);
        ValidationProblemDetails problemDetails = BuildValidationProblemDetails(context, exception);
        await WriteProblemDetailsToResponseAsync(context, problemDetails);
    }

    private static async Task HandleSystemExceptionAsync(HttpContext context, Exception exception)
    {
        ConfigureSystemJsonResponseHeaders(context);
        ProblemDetails problemDetails = BuildInternalServerErrorProblemDetails(context, exception);
        await WriteProblemDetailsToResponseAsync(context, problemDetails);
    }

    private static void ConfigureValidationJsonResponseHeaders(HttpContext context)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }

    private static void ConfigureSystemJsonResponseHeaders(HttpContext context)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }

    private static ValidationProblemDetails BuildValidationProblemDetails(HttpContext context, ValidationException exception)
    {
        ValidationProblemDetails problemDetails = new ValidationProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = context.Response.StatusCode,
            Detail = "See the errors property for details.",
            Instance = context.Request.Path
        };

        PopulateValidationErrors(problemDetails, exception);

        return problemDetails;
    }

    private static void PopulateValidationErrors(ValidationProblemDetails problemDetails, ValidationException exception)
    {
        IEnumerable<IGrouping<string, ValidationFailure>> groupedErrors = 
            exception.Errors.GroupBy((ValidationFailure error) => error.PropertyName);

        foreach (IGrouping<string, ValidationFailure> errorGroup in groupedErrors)
        {
            string[] errorMessages = errorGroup
                .Select((ValidationFailure error) => error.ErrorMessage)
                .ToArray();
            problemDetails.Errors.Add(errorGroup.Key, errorMessages);
        }
    }

    private static ProblemDetails BuildInternalServerErrorProblemDetails(HttpContext context, Exception exception)
    {
        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An internal server error occurred.",
            Status = context.Response.StatusCode,
            Detail = exception.Message,
            Instance = context.Request.Path
        };
    }

    private static async Task WriteProblemDetailsToResponseAsync<TProblemDetails>(HttpContext context, TProblemDetails problemDetails)
    {
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
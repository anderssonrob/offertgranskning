using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Offertgranskning.Api.Shared.Exceptions;

namespace Offertgranskning.Api.Infrastructure.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCode: HttpStatusCode.BadRequest,
                Title: "Validation Error",
                Detail: "One or more validation errors occurred.",
                validationException.Errors
            ),
            _ => (
                StatusCode: HttpStatusCode.InternalServerError,
                Title: "Internal Server Error",
                Detail: "An unexpected error occurred while processing your request.",
                Errors: (object?)null
            )
        };
        
        LogException(exception, statusCode);
        
        var response = new
        {
            type = GetProblemType(statusCode),
            title,
            status = (int)statusCode,
            detail,
            traceId = httpContext.TraceIdentifier,
            errors
        };

        httpContext.Response.StatusCode = (int)statusCode;
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        
        return true;
    }
    
    private void LogException(Exception exception, HttpStatusCode statusCode)
    {
        var logLevel = statusCode switch
        {
            HttpStatusCode.InternalServerError => LogLevel.Error,
            HttpStatusCode.BadRequest => LogLevel.Warning,
            HttpStatusCode.NotFound => LogLevel.Information,
            _ => LogLevel.Warning
        };

        _logger.Log(logLevel, exception, "Exception handled: {ExceptionType}", exception.GetType().Name);
    }
    
    private static string GetProblemType(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        HttpStatusCode.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
        HttpStatusCode.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
    };
}
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MyProject.Application.Payloads;

namespace MyProject.WebApi.Middleware;

/// <summary>
/// Handles exceptions globally for the application, logging errors and formatting error responses.
/// </summary>
public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Attempts to handle an exception and write a formatted error response.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the exception was handled; otherwise, false.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId
        );

        var (statusCode, title) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new Result<string>(false, statusCode, exception.Message, title);

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);

        return true;
    }

    /// <summary>
    /// Maps an exception to an HTTP status code and error title.
    /// </summary>
    /// <param name="exception">The exception to map.</param>
    /// <returns>A tuple containing the status code and error title.</returns>
    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException ex => (400, ex.Message),
            ValidationException ex => (400, ex.Message),
            UnauthorizedAccessException ex => (401, ex.Message),
            SecurityTokenExpiredException => (401, "Token has expired."),
            _ => (500, exception.Message)
        };
    }
}
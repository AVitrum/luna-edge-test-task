using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MyProject.Application.Payloads;

namespace MyProject.WebApi.Middleware;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

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

        var response = new Result<string>(false, exception.Message, statusCode, title);

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);

        return true;
    }

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
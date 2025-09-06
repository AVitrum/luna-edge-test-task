using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MyProject.Infrastructure.Persistence;

namespace MyProject.WebApi.Middleware;

/// <summary>
/// Provides extension methods for configuring the application's request pipeline, endpoints, and database migration.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Applies any pending database migrations at application startup.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Throws if migration fails.</exception>
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<AppDbContext>();
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
    
    /// <summary>
    /// Configures the middleware pipeline for the web application, including Swagger, exception handling, HTTPS redirection, and routing.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.UseRouting();
    }
    
    /// <summary>
    /// Maps application endpoints and configures JWT authentication and error handling.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapApplicationEndpoints(this WebApplication app)
    {
        app.UseMiddleware<JwtMiddleware>();
        app.MapControllers();

        app.Map("/error", async (HttpContext context, IExceptionHandler exceptionHandler) =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature?.Error != null)
            {
                await exceptionHandler.TryHandleAsync(context, exceptionFeature.Error, context.RequestAborted);
            }
        });
    }
}
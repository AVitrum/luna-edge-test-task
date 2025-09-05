using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MyProject.Infrastructure.Persistence;

namespace MyProject.WebApi.Middleware;

public static class ApplicationBuilderExtensions
{
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
    
    public static void ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.UseRouting();
    }
    
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
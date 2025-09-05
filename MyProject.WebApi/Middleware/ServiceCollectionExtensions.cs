using Microsoft.AspNetCore.Diagnostics;
using MyProject.Application;
using MyProject.Infrastructure;

namespace MyProject.WebApi.Middleware;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddInfrastructure(configuration);
        services.AddApplicationServices();
        
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();

        return services;
    }
}
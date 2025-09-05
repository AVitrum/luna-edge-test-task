using Microsoft.AspNetCore.Diagnostics;
using MyProject.Application;
using MyProject.Infrastructure;
using MyProject.Infrastructure.Settings;

namespace MyProject.WebApi.Middleware;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStrings>(configuration.GetSection(ConnectionStrings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddInfrastructure(configuration);
        services.AddApplicationServices();
        
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();

        return services;
    }
}
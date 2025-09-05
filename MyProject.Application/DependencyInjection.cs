using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Interfaces;
using MyProject.Application.Services;

namespace MyProject.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITaskService, TaskService>();
        
        return services;
    }
}
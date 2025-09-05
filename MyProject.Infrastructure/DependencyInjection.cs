using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Interfaces;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Persistence;
using MyProject.Infrastructure.Repositories;
using MyProject.Infrastructure.Security;

namespace MyProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt => 
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql());

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}
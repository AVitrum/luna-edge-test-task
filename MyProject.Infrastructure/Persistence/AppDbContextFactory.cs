// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
// using System.IO;
//
// namespace MyProject.Infrastructure.Persistence;
//
// public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//     public AppDbContext CreateDbContext(string[] args)
//     {
//         var configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
//             .AddEnvironmentVariables() 
//             .Build();
//
//         var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//         
//         var connectionString = configuration.GetConnectionString("DB_CONNECTION");
//         optionsBuilder.UseNpgsql(connectionString);
//
//         return new AppDbContext(optionsBuilder.Options);
//     }
// }
using MyProject.Infrastructure.Persistence.Seeds;
using MyProject.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var defaultDirectory = EnvironmentSetup.ConfigureEnvironment(builder);
LoggingConfiguration.ConfigureLogging(builder, defaultDirectory);

builder.Services.AddServiceExtensions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

await app.MigrateDatabaseAsync();

app.ConfigurePipeline();
app.MapApplicationEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
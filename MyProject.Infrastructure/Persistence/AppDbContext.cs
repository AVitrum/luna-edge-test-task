using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;
using Task = MyProject.Domain.Entities.Task;

namespace MyProject.Infrastructure.Persistence;

/// <summary>
/// Represents the Entity Framework database context for the application.
/// Provides access to Users and Tasks entities.
/// </summary>
public sealed class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="AppDbContext"/> with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    /// <summary>
    /// Gets the set of <see cref="User"/> entities in the database.
    /// </summary>
    public DbSet<User> Users => Set<User>();
    
    /// <summary>
    /// Gets the set of <see cref="Task"/> entities in the database.
    /// </summary>
    public DbSet<Task> Tasks => Set<Task>();
    
    /// <summary>
    /// Configures the entity model for the context using the provided <see cref="ModelBuilder"/>.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
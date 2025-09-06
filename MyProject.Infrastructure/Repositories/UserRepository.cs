using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Persistence;

namespace MyProject.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="User"/> entities, including user creation and lookup operations.
/// </summary>
public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Adds a new user if the username and email are unique.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>True if the user was added; false if a user with the same username or email exists.</returns>
    public async Task<bool> AddUserAsync(User user)
    {
        var existingUser = await DbSet
            .FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email);
        if (existingUser != null)
        {
            return false;
        }
        
        await AddAsync(user);
        await SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// Checks if a user exists by username.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if a user exists with the given username; otherwise, false.</returns>
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await DbSet.AnyAsync(u => u.Username == username);
    }

    /// <summary>
    /// Checks if a user exists by email address.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>True if a user exists with the given email; otherwise, false.</returns>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await DbSet.AnyAsync(u => u.Email == email);
    }
}
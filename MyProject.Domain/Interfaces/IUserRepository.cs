using MyProject.Domain.Entities;

namespace MyProject.Domain.Interfaces;

/// <summary>
/// Defines repository operations specific to <see cref="User"/> entities, including creation and lookup.
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Adds a new user if the username and email are unique.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>True if the user was added; false if a user with the same username or email exists.</returns>
    Task<bool> AddUserAsync(User user);

    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Checks if a user exists by username.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if a user exists with the given username; otherwise, false.</returns>
    Task<bool> ExistsByUsernameAsync(string username);

    /// <summary>
    /// Checks if a user exists by email address.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>True if a user exists with the given email; otherwise, false.</returns>
    Task<bool> ExistsByEmailAsync(string email);
}
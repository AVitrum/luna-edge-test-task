using MyProject.Domain.Entities;

namespace MyProject.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> AddUserAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
}
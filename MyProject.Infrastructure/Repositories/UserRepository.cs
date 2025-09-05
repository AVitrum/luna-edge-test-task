using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Persistence;

namespace MyProject.Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<bool> AddUserAsync(User user)
    {
        var existingUser = await _dbSet
            .FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email);
        if (existingUser != null)
        {
            return false;
        }
        
        await AddAsync(user);
        await SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}
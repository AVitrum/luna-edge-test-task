using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Persistence;
using Task = MyProject.Domain.Entities.Task;

namespace MyProject.Infrastructure.Repositories;

public class TaskRepository(AppDbContext context) : GenericRepository<Task>(context), ITaskRepository
{
    public async Task<IEnumerable<Task>> GetAllTasksByUserId(Guid userId, int pageNumber, int pageSize)
    {
        return await _dbSet
            .Where(t => t.UserId == userId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountByUserId(Guid userId)
    {
        return await _dbSet.CountAsync(t => t.UserId == userId);
    }
}
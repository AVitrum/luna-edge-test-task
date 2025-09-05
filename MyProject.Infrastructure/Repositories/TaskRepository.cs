using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Persistence;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Infrastructure.Repositories;

public class TaskRepository(AppDbContext context) : GenericRepository<Task>(context), ITaskRepository
{
    public async Task<IEnumerable<Task>> GetAllTasksByUserIdAsync(Guid userId,
        int pageNumber,
        int pageSize,
        DateTime? dueDate,
        TaskStatus? status,
        TaskPriority? priority)
    {
        var query = BuildFilteredQuery(userId, status, dueDate, priority);

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountByUserIdAsync(Guid userId, TaskStatus? status, DateTime? dueDate, TaskPriority? priority)
    {
        var query = BuildFilteredQuery(userId, status, dueDate, priority);
        return await query.CountAsync();
    }
    
    private IQueryable<Task> BuildFilteredQuery(Guid userId, TaskStatus? status, DateTime? dueDate, TaskPriority? priority)
    {
        var query = _dbSet.Where(t => t.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (dueDate.HasValue)
        {
            query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        return query;
    }
}
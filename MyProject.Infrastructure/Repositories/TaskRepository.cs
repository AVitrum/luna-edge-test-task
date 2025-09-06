using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Persistence;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="Task"/> entities, including filtering and pagination for user tasks.
/// </summary>
public class TaskRepository : GenericRepository<Task>, ITaskRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TaskRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Gets a paginated and filtered list of tasks for a specific user.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="dueDate">Optional filter for due date.</param>
    /// <param name="status">Optional filter for task status.</param>
    /// <param name="priority">Optional filter for task priority.</param>
    /// <returns>A collection of tasks matching the criteria.</returns>
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

    /// <summary>
    /// Gets the total count of tasks for a user matching the specified filters.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="status">Optional filter for task status.</param>
    /// <param name="dueDate">Optional filter for due date.</param>
    /// <param name="priority">Optional filter for task priority.</param>
    /// <returns>The total count of matching tasks.</returns>
    public async Task<int> GetTotalCountByUserIdAsync(Guid userId, TaskStatus? status, DateTime? dueDate, TaskPriority? priority)
    {
        var query = BuildFilteredQuery(userId, status, dueDate, priority);
        return await query.CountAsync();
    }
    
    /// <summary>
    /// Builds a filtered query for tasks by user and optional criteria.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="status">Optional filter for task status.</param>
    /// <param name="dueDate">Optional filter for due date.</param>
    /// <param name="priority">Optional filter for task priority.</param>
    /// <returns>An <see cref="IQueryable{Task}"/> for further composition.</returns>
    private IQueryable<Task> BuildFilteredQuery(Guid userId, TaskStatus? status, DateTime? dueDate, TaskPriority? priority)
    {
        var query = DbSet.Where(t => t.UserId == userId);

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
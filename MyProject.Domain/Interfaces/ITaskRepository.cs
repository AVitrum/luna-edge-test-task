using MyProject.Domain.Enums;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Domain.Interfaces;

/// <summary>
/// Defines repository operations specific to <see cref="Task"/> entities, including user-based filtering and pagination.
/// </summary>
public interface ITaskRepository : IGenericRepository<Task>
{
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
    Task<IEnumerable<Task>> GetAllTasksByUserIdAsync(Guid userId, int pageNumber, int pageSize, DateTime? dueDate,
        TaskStatus? status, TaskPriority? priority);

    /// <summary>
    /// Gets the total count of tasks for a user matching the specified filters.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="status">Optional filter for task status.</param>
    /// <param name="dueDate">Optional filter for due date.</param>
    /// <param name="priority">Optional filter for task priority.</param>
    /// <returns>The total count of matching tasks.</returns>
    Task<int> GetTotalCountByUserIdAsync(Guid userId, TaskStatus? status, DateTime? dueDate, TaskPriority? priority);
}
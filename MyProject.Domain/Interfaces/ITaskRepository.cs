using MyProject.Domain.Enums;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Domain.Interfaces;

public interface ITaskRepository : IGenericRepository<Task>
{
    Task<IEnumerable<Task>> GetAllTasksByUserIdAsync(Guid userId, int pageNumber, int pageSize, DateTime? dueDate,
        TaskStatus? status, TaskPriority? priority);
    Task<int> GetTotalCountByUserIdAsync(Guid userId, TaskStatus? status, DateTime? dueDate, TaskPriority? priority);
}
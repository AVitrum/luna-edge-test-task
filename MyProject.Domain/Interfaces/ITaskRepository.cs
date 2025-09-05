using Task = MyProject.Domain.Entities.Task;

namespace MyProject.Domain.Interfaces;

public interface ITaskRepository : IGenericRepository<Task>
{
    Task<IEnumerable<Task>> GetAllTasksByUserId(Guid userId, int pageNumber, int pageSize);
    Task<int> GetTotalCountByUserId(Guid userId);
}
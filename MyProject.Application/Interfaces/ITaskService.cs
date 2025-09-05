using MyProject.Application.Payloads;
using MyProject.Application.Payloads.Dtos;
using MyProject.Application.Payloads.Responses;

namespace MyProject.Application.Interfaces;

public interface ITaskService
{
    Task<Result<bool>> CreateTaskAsync(Guid userId, string title, string? description, DateTime? dueDate, string status, string priority);
    Task<Result<GetTasksResponse>> GetTasksAsync(Guid userId, int pageNumber, int pageSize, DateTime? dueDate,
        string? status, string? priority);
    Task<Result<TaskDto>> GetTaskByIdAsync(Guid id, Guid userId);
    Task<Result<bool>> UpdateTaskAsync(Guid id, Guid userId, string? title, string? description, DateTime? dueDate, string? status, string? priority);
    Task<Result<bool>> DeleteTaskAsync(Guid id, Guid userId);
}
using MyProject.Application.Payloads;
using MyProject.Application.Payloads.Dtos;
using MyProject.Application.Payloads.Responses;

namespace MyProject.Application.Interfaces;

/// <summary>
/// Provides methods for managing user tasks, including creation, retrieval, update, and deletion.
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Creates a new task for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user creating the task.</param>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task (optional).</param>
    /// <param name="dueDate">The due date of the task (optional).</param>
    /// <param name="status">The status of the task.</param>
    /// <param name="priority">The priority of the task.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result<bool>> CreateTaskAsync(Guid userId, string title, string? description, DateTime? dueDate, string status, string priority);

    /// <summary>
    /// Retrieves a paginated list of tasks for the specified user, with optional filters.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The page size for pagination.</param>
    /// <param name="dueDate">Optional due date filter.</param>
    /// <param name="status">Optional status filter.</param>
    /// <param name="priority">Optional priority filter.</param>
    /// <returns>A result containing the paginated list of tasks.</returns>
    Task<Result<GetTasksResponse>> GetTasksAsync(Guid userId, int pageNumber, int pageSize, DateTime? dueDate,
        string? status, string? priority);

    /// <summary>
    /// Retrieves a specific task by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A result containing the task details.</returns>
    Task<Result<TaskDto>> GetTaskByIdAsync(Guid id, Guid userId);

    /// <summary>
    /// Updates a specific task by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="title">The new title of the task (optional).</param>
    /// <param name="description">The new description of the task (optional).</param>
    /// <param name="dueDate">The new due date of the task (optional).</param>
    /// <param name="status">The new status of the task (optional).</param>
    /// <param name="priority">The new priority of the task (optional).</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result<bool>> UpdateTaskAsync(Guid id, Guid userId, string? title, string? description, DateTime? dueDate, string? status, string? priority);

    /// <summary>
    /// Deletes a specific task by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result<bool>> DeleteTaskAsync(Guid id, Guid userId);
}
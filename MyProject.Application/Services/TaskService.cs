using Microsoft.Extensions.Logging;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads;
using MyProject.Application.Payloads.Dtos;
using MyProject.Application.Payloads.Responses;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Application.Services;

/// <summary>
/// Service for handling task-related operations.
/// </summary>
public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskService"/> class.
    /// </summary>
    /// <param name="taskRepository">The task repository.</param>
    /// <param name="logger">The logger.</param>
    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new task asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user creating the task.</param>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="dueDate">The due date of the task.</param>
    /// <param name="status">The status of the task.</param>
    /// <param name="priority">The priority of the task.</param>
    /// <returns>A result object indicating success or failure.</returns>
    public async Task<Result<bool>> CreateTaskAsync(Guid userId, string title, string? description, DateTime? dueDate, string status, string priority)
    {
        _logger.LogInformation("Attempting to create task for user {UserId} with title {Title}", userId, title);
        try
        {
            if (!Enum.TryParse<TaskStatus>(status, true, out var parsedStatus))
            {
                var allowed = string.Join(", ", Enum.GetNames(typeof(TaskStatus)));
                return new Result<bool>(false, 400, $"Invalid status value. Allowed values: {allowed}.", false);
            }
            
            if (!Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority))
            {
                var allowed = string.Join(", ", Enum.GetNames(typeof(TaskPriority)));
                return new Result<bool>(false, 400, $"Invalid priority value. Allowed values: {allowed}.", false);
            }
            
            var newTask = new Task
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Status = parsedStatus,
                Priority = parsedPriority,
                UserId = userId
            };
            await _taskRepository.AddAsync(newTask);
            await _taskRepository.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} created successfully for user {UserId}", newTask.Id, userId);
            return new Result<bool>(true, 201, "Task created successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a task for user {UserId}", userId);
            return new Result<bool>(false, 500, $"An error occurred: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Retrieves a paginated list of tasks for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks are to be retrieved.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The page size for pagination.</param>
    /// <param name="dueDate">Optional filter for the due date.</param>
    /// <param name="status">Optional filter for the task status.</param>
    /// <param name="priority">Optional filter for the task priority.</param>
    /// <returns>A result object containing the list of tasks and pagination details.</returns>
    public async Task<Result<GetTasksResponse>> GetTasksAsync(Guid userId,
        int pageNumber,
        int pageSize,
        DateTime? dueDate,
        string? status,
        string? priority)
    {
        _logger.LogInformation("Attempting to retrieve tasks for user {UserId}", userId);
        try
        {
            TaskStatus? statusFilter = null;
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!Enum.TryParse<TaskStatus>(status, true, out var parsedStatus))
                {
                    var allowed = string.Join(", ", Enum.GetNames(typeof(TaskStatus)));
                    return new Result<GetTasksResponse>(false, 400, $"Invalid status value. Allowed values: {allowed}.", null!);
                }
                statusFilter = parsedStatus;
            }
            TaskPriority? priorityFilter = null;
            if (!string.IsNullOrWhiteSpace(priority))
            {
                if (!Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority))
                {
                    var allowed = string.Join(", ", Enum.GetNames(typeof(TaskPriority)));
                    return new Result<GetTasksResponse>(false, 400, $"Invalid priority value. Allowed values: {allowed}.", null!);
                }
                priorityFilter = parsedPriority;
            }
            var tasks = (await _taskRepository.GetAllTasksByUserIdAsync(userId, pageNumber, pageSize, dueDate, statusFilter, priorityFilter)).ToList();
            var totalCount = await _taskRepository.GetTotalCountByUserIdAsync(userId, statusFilter, dueDate, priorityFilter);
            if (tasks.Count == 0)
            {
                _logger.LogWarning("No tasks found for user {UserId} with the specified criteria", userId);
                return new Result<GetTasksResponse>(false, 404, "No tasks found.", new GetTasksResponse
                {
                    Tasks = null,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                });
            }
            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt!.Value
            }).ToList();
            var response = new GetTasksResponse
            {
                Tasks = taskDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
            _logger.LogInformation("Successfully retrieved {TaskCount} tasks for user {UserId}", tasks.Count, userId);
            return new Result<GetTasksResponse>(true, 200, "Tasks retrieved successfully.", response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving tasks for user {UserId}", userId);
            return new Result<GetTasksResponse>(false, 500, $"An error occurred: {ex.Message}", null!);
        }
    }

    /// <summary>
    /// Retrieves a specific task by its ID for a specific user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns>A result object containing the task data transfer object.</returns>
    public async Task<Result<TaskDto>> GetTaskByIdAsync(Guid id, Guid userId)
    {
        _logger.LogInformation("Attempting to retrieve task {TaskId} for user {UserId}", id, userId);
        try
        {
            var task = await GetTaskIfExistsAsync(id, userId);
            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}", id, userId);
                return new Result<TaskDto>(false, 404, "Task not found.", null!);
            }

            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt!.Value
            };

            _logger.LogInformation("Successfully retrieved task {TaskId} for user {UserId}", id, userId);
            return new Result<TaskDto>(true, 200, "Task retrieved successfully.", taskDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving task {TaskId} for user {UserId}", id, userId);
            return new Result<TaskDto>(false, 500, $"An error occurred: {ex.Message}", null!);
        }
    }

    /// <summary>
    /// Updates an existing task asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <param name="title">The new title of the task.</param>
    /// <param name="description">The new description of the task.</param>
    /// <param name="dueDate">The new due date of the task.</param>
    /// <param name="status">The new status of the task.</param>
    /// <param name="priority">The new priority of the task.</param>
    /// <returns>A result object indicating success or failure.</returns>
    public async Task<Result<bool>> UpdateTaskAsync(
        Guid id,
        Guid userId,
        string? title,
        string? description,
        DateTime? dueDate,
        string? status,
        string? priority)
    {
        _logger.LogInformation("Attempting to update task {TaskId} for user {UserId}", id, userId);
        var task = await GetTaskIfExistsAsync(id, userId);
        if (task == null)
        {
            _logger.LogWarning("Update failed. Task {TaskId} not found for user {UserId}", id, userId);
            return new Result<bool>(false, 404, "Task not found.", false);
        }
        if (!string.IsNullOrWhiteSpace(title)) task.Title = title;
        if (!string.IsNullOrWhiteSpace(description)) task.Description = description;
        if (dueDate.HasValue) task.DueDate = dueDate.Value;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<TaskStatus>(status, true, out var parsedStatus))
            {
                var allowed = string.Join(", ", Enum.GetNames(typeof(TaskStatus)));
                return new Result<bool>(false, 400, $"Invalid status value. Allowed values: {allowed}.", false);
            }
            task.Status = parsedStatus;
        }
        if (!string.IsNullOrWhiteSpace(priority))
        {
            if (!Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority))
            {
                var allowed = string.Join(", ", Enum.GetNames(typeof(TaskPriority)));
                return new Result<bool>(false, 400, $"Invalid priority value. Allowed values: {allowed}.", false);
            }
            task.Priority = parsedPriority;
        }
        try
        {
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            _logger.LogInformation("Task {TaskId} updated successfully for user {UserId}", id, userId);
            return new Result<bool>(true, 200, "Task updated successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating task {TaskId} for user {UserId}", id, userId);
            return new Result<bool>(false, 500, $"An error occurred: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Deletes a task asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns>A result object indicating success or failure.</returns>
    public async Task<Result<bool>> DeleteTaskAsync(Guid id, Guid userId)
    {
        _logger.LogInformation("Attempting to delete task {TaskId} for user {UserId}", id, userId);
        var task = await GetTaskIfExistsAsync(id, userId);
        if (task == null)
        {
            _logger.LogWarning("Delete failed. Task {TaskId} not found for user {UserId}", id, userId);
            return new Result<bool>(false, 404, "Task not found.", false);
        }

        try
        {
            _taskRepository.Remove(task);
            await _taskRepository.SaveChangesAsync();
            _logger.LogInformation("Task {TaskId} deleted successfully for user {UserId}", id, userId);
            return new Result<bool>(true, 200, "Task deleted successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting task {TaskId} for user {UserId}", id, userId);
            return new Result<bool>(false, 500, $"An error occurred: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Retrieves a task by its ID if it exists and belongs to the specified user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The task if found and owned by the user; otherwise, null.</returns>
    private async Task<Task?> GetTaskIfExistsAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null && task.UserId == userId ? task : null;
    }
}
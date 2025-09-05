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

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> CreateTaskAsync(Guid userId, string title, string? description, DateTime? dueDate, string status, string priority)
    {
        _logger.LogInformation("Attempting to create task for user {UserId} with title {Title}", userId, title);
        try
        {
            var newTask = new Task
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Status = Enum.Parse<TaskStatus>(status, true),
                Priority = Enum.Parse<TaskPriority>(priority, true),
                UserId = userId
            };

            await _taskRepository.AddAsync(newTask);
            await _taskRepository.SaveChangesAsync();
            
            _logger.LogInformation("Task {TaskId} created successfully for user {UserId}", newTask.Id, userId);
            return new Result<bool>(true, "Task created successfully.", 201, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a task for user {UserId}", userId);
            return new Result<bool>(false, $"An error occurred: {ex.Message}", 500, false);
        }
    }

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
            Enum.TryParse<TaskStatus>(status, true, out var parsedStatus);
            var statusFilter = !string.IsNullOrWhiteSpace(status) ? parsedStatus : (TaskStatus?)null;

            Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority);
            var priorityFilter = !string.IsNullOrWhiteSpace(priority) ? parsedPriority : (TaskPriority?)null;

            var tasks = (await _taskRepository.GetAllTasksByUserIdAsync(userId, pageNumber, pageSize, dueDate, statusFilter, priorityFilter)).ToList();
            var totalCount = await _taskRepository.GetTotalCountByUserIdAsync(userId, statusFilter, dueDate, priorityFilter);

            if (tasks.Count == 0)
            {
                _logger.LogWarning("No tasks found for user {UserId} with the specified criteria", userId);
                return new Result<GetTasksResponse>(false, "No tasks found.", 404, new GetTasksResponse
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
            return new Result<GetTasksResponse>(true, "Tasks retrieved successfully.", 200, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving tasks for user {UserId}", userId);
            return new Result<GetTasksResponse>(false, $"An error occurred: {ex.Message}", 500, null!);
        }
    }

    public async Task<Result<TaskDto>> GetTaskByIdAsync(Guid id, Guid userId)
    {
        _logger.LogInformation("Attempting to retrieve task {TaskId} for user {UserId}", id, userId);
        try
        {
            var task = await GetTaskIfExistsAsync(id, userId);
            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}", id, userId);
                return new Result<TaskDto>(false, "Task not found.", 404, null!);
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
            return new Result<TaskDto>(true, "Task retrieved successfully.", 200, taskDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving task {TaskId} for user {UserId}", id, userId);
            return new Result<TaskDto>(false, $"An error occurred: {ex.Message}", 500, null!);
        }
    }

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
            return new Result<bool>(false, "Task not found.", 404, false);
        }

        if (!string.IsNullOrWhiteSpace(title)) task.Title = title;
        if (!string.IsNullOrWhiteSpace(description)) task.Description = description;
        if (dueDate.HasValue) task.DueDate = dueDate.Value;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskStatus>(status, true, out var parsedStatus)) task.Status = parsedStatus;
        if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority)) task.Priority = parsedPriority;

        try
        {
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            _logger.LogInformation("Task {TaskId} updated successfully for user {UserId}", id, userId);
            return new Result<bool>(true, "Task updated successfully.", 200, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating task {TaskId} for user {UserId}", id, userId);
            return new Result<bool>(false, $"An error occurred: {ex.Message}", 500, false);
        }
    }

    public async Task<Result<bool>> DeleteTaskAsync(Guid id, Guid userId)
    {
        _logger.LogInformation("Attempting to delete task {TaskId} for user {UserId}", id, userId);
        var task = await GetTaskIfExistsAsync(id, userId);
        if (task == null)
        {
            _logger.LogWarning("Delete failed. Task {TaskId} not found for user {UserId}", id, userId);
            return new Result<bool>(false, "Task not found.", 404, false);
        }

        try
        {
            _taskRepository.Remove(task);
            await _taskRepository.SaveChangesAsync();
            _logger.LogInformation("Task {TaskId} deleted successfully for user {UserId}", id, userId);
            return new Result<bool>(true, "Task deleted successfully.", 200, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting task {TaskId} for user {UserId}", id, userId);
            return new Result<bool>(false, $"An error occurred: {ex.Message}", 500, false);
        }
    }

    private async Task<Task?> GetTaskIfExistsAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null && task.UserId == userId ? task : null;
    }
}
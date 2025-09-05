using MyProject.Application.Interfaces;
using MyProject.Application.Payloads;
using MyProject.Application.Payloads.Dtos;
using MyProject.Application.Payloads.Responses;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<bool>> CreateTaskAsync(Guid userId, string title, string? description, DateTime? dueDate, string status, string priority)
    {
        try
        {
            var newTask = new Task
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Status = Enum.Parse<TaskStatus>(status, true),
                Priority = Enum.Parse<TaskPriority>(priority, true),
                UserId = userId,
            };

            await _taskRepository.AddAsync(newTask);
            await _taskRepository.SaveChangesAsync();
            return new Result<bool>(true, "Task created successfully.", 201, true);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"An error occurred: {ex.Message}", 500, false);
        }
    }

    public async Task<Result<GetTasksResponse>> GetTasksAsync(Guid userId, int pageNumber, int pageSize)
    {
        try
        {
            var tasks = (await _taskRepository.GetAllTasksByUserId(userId, pageNumber, pageSize)).ToList();
            var totalCount = await _taskRepository.GetTotalCountByUserId(userId);
            
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
                TotalCount = totalCount,
            };
            
            var result = new Result<GetTasksResponse>(true, "Tasks retrieved successfully.", 200, response);

            return result;
        }
        catch (Exception ex)
        {
            return new Result<GetTasksResponse>(false, $"An error occurred: {ex.Message}", 500, null!);
        }
    }

    public async Task<Result<TaskDto>> GetTaskByIdAsync(Guid id, Guid userId)
    {
        try
        {
            var task = await GetTaskIfExistsAsync(id, userId);
            if (task == null)
            {
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
            
            return new Result<TaskDto>(true, "Task retrieved successfully.", 200, taskDto);
        }
        catch (Exception ex)
        {
            return new Result<TaskDto>(false, $"An error occurred: {ex.Message}", 500, null!);
        }
    }

    public async Task<Result<bool>> UpdateTaskAsync(Guid id, Guid userId, string? title, string? description, DateTime? dueDate, string? status, string? priority)
    {
        var task = await GetTaskIfExistsAsync(id, userId);
        if (task == null)
        {
            return new Result<bool>(false, "Task not found.", 404, false);
        }
        
        if (!string.IsNullOrWhiteSpace(title))
        {
            task.Title = title;
        }
        
        if (!string.IsNullOrWhiteSpace(description))
        {
            task.Description = description;
        }

        if (dueDate.HasValue)
        {
            task.DueDate = dueDate.Value;
        }
        
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskStatus>(status, true, out var parsedStatus))
        {
            task.Status = parsedStatus;
        }
        
        if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority))
        {
            task.Priority = parsedPriority;
        }
        
        try
        {
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return new Result<bool>(true, "Task updated successfully.", 200, true);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"An error occurred: {ex.Message}", 500, false);
        }
    }

    public async Task<Result<bool>> DeleteTaskAsync(Guid id, Guid userId)
    {
        var task = await GetTaskIfExistsAsync(id, userId);
        if (task == null)
        {
            return new Result<bool>(false, "Task not found.", 404, false);
        }
        
        try
        {
            _taskRepository.Remove(task);
            await _taskRepository.SaveChangesAsync();
            return new Result<bool>(true, "Task deleted successfully.", 200, true);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"An error occurred: {ex.Message}", 500, false);
        }
    }
    
    private async Task<Task?> GetTaskIfExistsAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null && task.UserId == userId ? task : null;
    }
}
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads.Requests;
using MyProject.Domain.Entities;
using MyProject.WebApi.Attributes;

namespace MyProject.WebApi.Controllers;

/// <summary>
/// Controller for managing user tasks. Provides endpoints for creating, retrieving, updating, and deleting tasks.
/// All endpoints require authentication.
/// </summary>
[ApiController]
[Route("[controller]")]
[CustomAuthorize]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Creates a new task for the authenticated user.
    /// </summary>
    /// <param name="request">The task creation request payload.</param>
    /// <returns>Result of the task creation operation.</returns>
    /// <remarks>
    /// Route: POST /tasks<br/>
    /// Authentication: Required<br/>
    /// <b>Acceptable variables:</b><br/>
    /// <ul>
    ///   <li><b>Title</b> (string, required): Task name.</li>
    ///   <li><b>Description</b> (string, optional): Task details.</li>
    ///   <li><b>DueDate</b> (string, optional): Date in <c>yyyy-MM-dd</c> or <c>yyyy-MM-ddTHH:mm:ssZ</c> (ISO 8601). Example: <c>2025-09-06</c> or <c>2025-09-06T12:00:00Z</c>.</li>
    ///   <li><b>Status</b> (string, optional): <c>Pending</c>, <c>InProgress</c>, <c>Completed</c>. Default: <c>Pending</c>.</li>
    ///   <li><b>Priority</b> (string, optional): <c>Low</c>, <c>Medium</c>, <c>High</c>. Default: <c>High</c>.</li>
    /// </ul>
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var user = HttpContext.Items["User"] as User;
        DateTime? dueDate = null;
        if (!string.IsNullOrWhiteSpace(request.DueDate))
        {
            var formats = new[] { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ssZ", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fffZ" };
            if (!DateTime.TryParseExact(request.DueDate, formats, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var parsedDate))
            {
                return BadRequest(new {
                    Success = false,
                    Code = 400,
                    Message = "Invalid dueDate format. Expected format: yyyy-MM-dd or yyyy-MM-ddTHH:mm:ssZ (ISO 8601).",
                    Data = (object?)null
                });
            }
            dueDate = parsedDate;
        }
        var result = await _taskService.CreateTaskAsync(
            user!.Id, request.Title, request.Description, dueDate, request.Status, request.Priority);
        
        return StatusCode(result.Code, result);
    }
    
    /// <summary>
    /// Retrieves a paginated list of tasks for the authenticated user.
    /// </summary>
    /// <param name="paginationParams">Pagination parameters.</param>
    /// <param name="status">Optional status filter. Accepts: Pending, InProgress, Completed.</param>
    /// <param name="dueDate">Optional due date filter. Format: yyyy-MM-dd or ISO 8601 (e.g. 2025-09-06 or 2025-09-06T00:00:00Z).</param>
    /// <param name="priority">Optional priority filter. Accepts: Low, Medium, High.</param>
    /// <returns>Paginated list of tasks.</returns>
    /// <remarks>
    /// Route: GET /tasks
    /// Authentication: Required
    /// <br/>
    /// <b>Status values:</b> Pending, InProgress, Completed
    /// <br/>
    /// <b>Priority values:</b> Low, Medium, High
    /// <br/>
    /// <b>Date format:</b> yyyy-MM-dd or ISO 8601 (e.g. 2025-09-06 or 2025-09-06T00:00:00Z)
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] string? status,
        [FromQuery] DateTime? dueDate,
        [FromQuery] string? priority)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.GetTasksAsync(
            user!.Id, paginationParams.PageNumber, paginationParams.PageSize, dueDate, status, priority);

        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Retrieves a specific task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>Details of the requested task.</returns>
    /// <remarks>
    /// Route: GET /tasks/{id}
    /// Authentication: Required
    /// </remarks>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.GetTaskByIdAsync(id, user!.Id);
        
        return StatusCode(result.Code, result);
    }
    
    /// <summary>
    /// Updates a specific task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <param name="request">The update request payload.</param>
    /// <returns>Result of the update operation.</returns>
    /// <remarks>
    /// Route: PUT /tasks/{id}<br/>
    /// Authentication: Required<br/>
    /// <b>Acceptable variables:</b><br/>
    /// <ul>
    ///   <li><b>Title</b> (string, optional): New task name.</li>
    ///   <li><b>Description</b> (string, optional): New task details.</li>
    ///   <li><b>DueDate</b> (DateTime?, optional): Date in <c>yyyy-MM-dd</c> or <c>yyyy-MM-ddTHH:mm:ssZ</c> (ISO 8601). Example: <c>2025-09-06</c> or <c>2025-09-06T12:00:00Z</c>.</li>
    ///   <li><b>Status</b> (string, optional): <c>Pending</c>, <c>InProgress</c>, <c>Completed</c>.</li>
    ///   <li><b>Priority</b> (string, optional): <c>Low</c>, <c>Medium</c>, <c>High</c>.</li>
    /// </ul>
    /// </remarks>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.UpdateTaskAsync(
            id, user!.Id, request.Title, request.Description, request.DueDate, request.Status, request.Priority);
        
        return StatusCode(result.Code, result);
    }
    
    /// <summary>
    /// Deletes a specific task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>Result of the deletion operation.</returns>
    /// <remarks>
    /// Route: DELETE /tasks/{id}
    /// Authentication: Required
    /// </remarks>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.DeleteTaskAsync(id, user!.Id);
        
        return StatusCode(result.Code, result);
    }
}
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads.Requests;
using MyProject.Domain.Entities;
using MyProject.WebApi.Attributes;

namespace MyProject.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.CreateTaskAsync(
            user!.Id, request.Title, request.Description, request.DueDate, request.Status, request.Priority);
        
        return StatusCode(result.Code, result);
    }
    
    [HttpGet]
    [CustomAuthorize]
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

    [HttpGet("{id:guid}")]
    [CustomAuthorize]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.GetTaskByIdAsync(id, user!.Id);
        
        return StatusCode(result.Code, result);
    }
    
    [HttpPut("{id:guid}")]
    [CustomAuthorize]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.UpdateTaskAsync(
            id, user!.Id, request.Title, request.Description, request.DueDate, request.Status, request.Priority);
        
        return StatusCode(result.Code, result);
    }
    
    [HttpDelete("{id:guid}")]
    [CustomAuthorize]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var user = HttpContext.Items["User"] as User;
        var result = await _taskService.DeleteTaskAsync(id, user!.Id);
        
        return StatusCode(result.Code, result);
    }
}
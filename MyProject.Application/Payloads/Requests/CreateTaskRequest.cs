using System.ComponentModel.DataAnnotations;

namespace MyProject.Application.Payloads.Requests;

/// <summary>
/// Request payload for creating a new task.
/// </summary>
/// <remarks>
/// <b>Date format:</b> <c>yyyy-MM-dd</c> or <c>yyyy-MM-ddTHH:mm:ssZ</c> (ISO 8601). Example: <c>2025-09-06</c> or <c>2025-09-06T12:00:00Z</c>.<br/>
/// <b>Status values:</b> <c>Pending</c>, <c>InProgress</c>, <c>Completed</c>.<br/>
/// <b>Priority values:</b> <c>Low</c>, <c>Medium</c>, <c>High</c>.
/// </remarks>
public class CreateTaskRequest
{
    /// <summary>
    /// Title of the task. Required.
    /// </summary>
    [Required (ErrorMessage = "Title is required.")]
    public required string Title { get; init; }
    
    /// <summary>
    /// Description of the task (optional).
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Due date of the task. Format: yyyy-MM-dd or yyyy-MM-ddTHH:mm:ssZ (ISO 8601). Optional.
    /// </summary>
    public string? DueDate { get; init; }
    
    /// <summary>
    /// Status of the task. Accepts: Pending, InProgress, Completed. Default: Pending.
    /// </summary>
    public string Status { get; init; } = "Pending";
    
    /// <summary>
    /// Priority of the task. Accepts: Low, Medium, High. Default: High.
    /// </summary>
    public string Priority { get; init; } = "High";
}
namespace MyProject.Application.Payloads.Requests;

/// <summary>
/// Request payload for updating a task.
/// </summary>
public class UpdateTaskRequest
{
    /// <summary>
    /// New title for the task.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// New description for the task.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// New due date for the task. Format: yyyy-MM-dd or yyyy-MM-ddTHH:mm:ssZ (ISO 8601).
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// New status for the task. Accepts: Pending, InProgress, Completed.
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// New priority for the task. Accepts: Low, Medium, High.
    /// </summary>
    public string? Priority { get; set; }
}
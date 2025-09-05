using System.ComponentModel.DataAnnotations;

namespace MyProject.Application.Payloads.Requests;

public class CreateTaskRequest
{
    [Required (ErrorMessage = "Title is required.")]
    public required string Title { get; init; }
    public string? Description { get; init; }
    public DateTime? DueDate { get; init; }
    public string Status { get; init; } = "Pending";
    public string Priority { get; init; } = "High";
}
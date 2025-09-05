namespace MyProject.Application.Payloads.Dtos;

public class TaskDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; } 
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public required string Status { get; set; }
    public required string Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
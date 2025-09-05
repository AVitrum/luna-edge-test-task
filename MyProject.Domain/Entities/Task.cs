using MyProject.Domain.Enums;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Domain.Entities;

public class Task : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    
    public Guid UserId { get; set; }
}
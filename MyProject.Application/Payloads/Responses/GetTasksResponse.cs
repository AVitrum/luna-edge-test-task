using MyProject.Application.Payloads.Dtos;

namespace MyProject.Application.Payloads.Responses;

public class GetTasksResponse
{
    public IEnumerable<TaskDto>? Tasks { get; set; } 
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
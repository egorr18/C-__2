using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Application.DTOs;

public class ProjectItemQuery
{
    public string? Search { get; set; }
    public ProjectItemStatus? Status { get; set; }
    public int? ProjectId { get; set; }
}

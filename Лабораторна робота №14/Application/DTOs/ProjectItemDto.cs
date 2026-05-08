namespace ProjectBoard.Application.DTOs;

public class ProjectItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? DueDateUtc { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }
    public int CommentsCount { get; set; }
}

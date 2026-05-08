namespace ProjectBoard.Domain.Entities;

public class ProjectItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectItemStatus Status { get; set; } = ProjectItemStatus.New;
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? DueDateUtc { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public int? AssignedUserId { get; set; }
    public AppUser? AssignedUser { get; set; }
    public ICollection<ItemComment> Comments { get; set; } = new List<ItemComment>();
}

namespace ProjectBoard.Domain.Entities;

public class ItemComment
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public int ProjectItemId { get; set; }
    public ProjectItem? ProjectItem { get; set; }
    public int AuthorId { get; set; }
    public AppUser? Author { get; set; }
}

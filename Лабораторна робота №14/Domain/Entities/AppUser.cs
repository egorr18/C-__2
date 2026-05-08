namespace ProjectBoard.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public ICollection<ProjectItem> AssignedItems { get; set; } = new List<ProjectItem>();
    public ICollection<ItemComment> Comments { get; set; } = new List<ItemComment>();
}

using System.ComponentModel.DataAnnotations;
using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Application.DTOs;

public class CreateProjectItemDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 5)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 3)]
    public ProjectItemStatus Status { get; set; } = ProjectItemStatus.New;

    [Range(0, 3)]
    public Priority Priority { get; set; } = Priority.Medium;

    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }

    [Range(1, int.MaxValue)]
    public int? AssignedUserId { get; set; }

    public DateTime? DueDateUtc { get; set; }
}

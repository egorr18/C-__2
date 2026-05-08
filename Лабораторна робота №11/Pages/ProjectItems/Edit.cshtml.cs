using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Application.Services;
using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Pages.ProjectItems;

public class EditModel : PageModel
{
    private readonly IProjectItemService _service;

    public EditModel(IProjectItemService service)
    {
        _service = service;
    }

    [BindProperty]
    public UpdateProjectItemDto Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        Input = new UpdateProjectItemDto
        {
            Title = item.Title,
            Description = item.Description,
            Status = Enum.Parse<ProjectItemStatus>(item.Status),
            Priority = Enum.Parse<Priority>(item.Priority),
            ProjectId = item.ProjectId,
            AssignedUserId = item.AssignedUserId,
            DueDateUtc = item.DueDateUtc
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var updated = await _service.UpdateAsync(id, Input, BuildUiUser(), cancellationToken);
        return updated ? RedirectToPage("Details", new { id }) : NotFound();
    }

    private static ClaimsPrincipal BuildUiUser()
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "Razor UI User"),
            new Claim(ClaimTypes.Role, "Manager"),
            new Claim("permission", "items.edit")
        }, "RazorDemo"));
    }
}

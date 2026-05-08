using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Application.Services;

namespace ProjectBoard.Pages.ProjectItems;

public class CreateModel : PageModel
{
    private readonly IProjectItemService _service;

    public CreateModel(IProjectItemService service)
    {
        _service = service;
    }

    [BindProperty]
    public CreateProjectItemDto Input { get; set; } = new() { ProjectId = 1 };

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _service.CreateAsync(Input, BuildUiUser(), cancellationToken);
        return RedirectToPage("Index");
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

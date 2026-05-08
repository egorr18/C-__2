using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Application.Services;

namespace ProjectBoard.Pages.ProjectItems;

public class DetailsModel : PageModel
{
    private readonly IProjectItemService _service;

    public DetailsModel(IProjectItemService service)
    {
        _service = service;
    }

    public ProjectItemDto? Item { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Item = await _service.GetByIdAsync(id, cancellationToken);
        return Item is null ? NotFound() : Page();
    }
}

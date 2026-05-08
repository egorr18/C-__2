using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Application.Services;

namespace ProjectBoard.Pages.ProjectItems;

public class IndexModel : PageModel
{
    private readonly IProjectItemService _service;

    public IndexModel(IProjectItemService service)
    {
        _service = service;
    }

    public IReadOnlyList<ProjectItemDto> Items { get; private set; } = Array.Empty<ProjectItemDto>();

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Items = await _service.GetAllAsync(new ProjectItemQuery { Search = Search }, cancellationToken);
    }
}

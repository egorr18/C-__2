using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Application.Services;
using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Controllers;

[ApiController]
[Route("api/project-items")]
public class ProjectItemsController : ControllerBase
{
    private readonly IProjectItemService _service;
    private readonly ILogger<ProjectItemsController> _logger;

    public ProjectItemsController(IProjectItemService service, ILogger<ProjectItemsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProjectItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProjectItemDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] ProjectItemStatus? status,
        [FromQuery] int? projectId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(new ProjectItemQuery
        {
            Search = search,
            Status = status,
            ProjectId = projectId
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProjectItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectItemDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [Authorize(Policy = "CanEditItems")]
    [HttpPost]
    [ProducesResponseType(typeof(ProjectItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectItemDto>> Create([FromBody] CreateProjectItemDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, User, cancellationToken);
        _logger.LogInformation("Claims received while creating item: {Claims}", string.Join("; ", User.Claims.Select(c => $"{c.Type}={c.Value}")));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Policy = "CanEditItems")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectItemDto dto, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(id, dto, User, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("simulate-data-error")]
    public IActionResult SimulateDataError()
    {
        throw new InvalidOperationException("Simulated data access exception for middleware demonstration.");
    }
}

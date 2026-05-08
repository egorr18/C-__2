using System.Security.Claims;
using ProjectBoard.Application.DTOs;

namespace ProjectBoard.Application.Services;

public interface IProjectItemService
{
    Task<IReadOnlyList<ProjectItemDto>> GetAllAsync(ProjectItemQuery query, CancellationToken cancellationToken = default);
    Task<ProjectItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectItemDto> CreateAsync(CreateProjectItemDto dto, ClaimsPrincipal user, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateProjectItemDto dto, ClaimsPrincipal user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

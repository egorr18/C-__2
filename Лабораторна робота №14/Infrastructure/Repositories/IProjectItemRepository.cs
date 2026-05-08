using ProjectBoard.Application.DTOs;
using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Infrastructure.Repositories;

public interface IProjectItemRepository
{
    Task<IReadOnlyList<ProjectItem>> GetAllAsync(ProjectItemQuery query, CancellationToken cancellationToken = default);
    Task<ProjectItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectItem> AddAsync(ProjectItem item, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProjectItem item, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

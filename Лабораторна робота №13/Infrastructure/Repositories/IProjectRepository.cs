using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Infrastructure.Repositories;

public interface IProjectRepository
{
    Task<IReadOnlyList<Project>> GetProjectsAsync(CancellationToken cancellationToken = default);
}

using Microsoft.EntityFrameworkCore;
using ProjectBoard.Domain.Entities;
using ProjectBoard.Infrastructure.Data;

namespace ProjectBoard.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;

    public ProjectRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Project>> GetProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Projects
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

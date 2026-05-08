using Microsoft.EntityFrameworkCore;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Domain.Entities;
using ProjectBoard.Infrastructure.Data;

namespace ProjectBoard.Infrastructure.Repositories;

public class ProjectItemRepository : IProjectItemRepository
{
    private readonly AppDbContext _db;

    public ProjectItemRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProjectItem>> GetAllAsync(ProjectItemQuery query, CancellationToken cancellationToken = default)
    {
        var items = _db.ProjectItems
            .Include(x => x.Project)
            .Include(x => x.AssignedUser)
            .Include(x => x.Comments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            items = items.Where(x => x.Title.Contains(query.Search) || x.Description.Contains(query.Search));
        }

        if (query.Status is not null)
        {
            items = items.Where(x => x.Status == query.Status);
        }

        if (query.ProjectId is not null)
        {
            items = items.Where(x => x.ProjectId == query.ProjectId);
        }

        return await items
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.DueDateUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<ProjectItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _db.ProjectItems
            .Include(x => x.Project)
            .Include(x => x.AssignedUser)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ProjectItem> AddAsync(ProjectItem item, CancellationToken cancellationToken = default)
    {
        _db.ProjectItems.Add(item);
        await _db.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(item.Id, cancellationToken))!;
    }

    public async Task UpdateAsync(ProjectItem item, CancellationToken cancellationToken = default)
    {
        _db.ProjectItems.Update(item);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _db.ProjectItems.FindAsync(new object[] { id }, cancellationToken);
        if (item is null)
        {
            return false;
        }

        _db.ProjectItems.Remove(item);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

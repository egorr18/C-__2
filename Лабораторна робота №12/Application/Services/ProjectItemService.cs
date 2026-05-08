using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ProjectBoard.Application.DTOs;
using ProjectBoard.Domain.Entities;
using ProjectBoard.Infrastructure.Repositories;

namespace ProjectBoard.Application.Services;

public class ProjectItemService : IProjectItemService
{
    private const string AllItemsCacheKey = "project-items:all";
    private readonly IProjectItemRepository _items;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProjectItemService> _logger;

    public ProjectItemService(
        IProjectItemRepository items,
        IMemoryCache cache,
        ILogger<ProjectItemService> logger)
    {
        _items = items;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProjectItemDto>> GetAllAsync(ProjectItemQuery query, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var useCache = string.IsNullOrWhiteSpace(query.Search) && query.Status is null && query.ProjectId is null;

        IReadOnlyList<ProjectItem> entities;
        if (useCache && _cache.TryGetValue(AllItemsCacheKey, out IReadOnlyList<ProjectItem>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache hit for project item list. Count={Count}", cached.Count);
            entities = cached;
        }
        else
        {
            _logger.LogInformation("Cache miss for project item list. Loading from database.");
            entities = await _items.GetAllAsync(query, cancellationToken);

            if (useCache)
            {
                _cache.Set(AllItemsCacheKey, entities, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Project item list cached. Count={Count}, TtlMinutes={Ttl}", entities.Count, 5);
            }
        }

        stopwatch.Stop();
        _logger.LogInformation(
            "Project items loaded. Count={Count}, ElapsedMs={ElapsedMs}",
            entities.Count,
            stopwatch.ElapsedMilliseconds);

        return entities.Select(MapToDto).ToList();
    }

    public async Task<ProjectItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _items.GetByIdAsync(id, cancellationToken);
        if (item is null)
        {
            _logger.LogWarning("Project item was not found. Id={Id}", id);
            return null;
        }

        _logger.LogInformation("Project item details loaded. Id={Id}", id);
        return MapToDto(item);
    }

    public async Task<ProjectItemDto> CreateAsync(CreateProjectItemDto dto, ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        ValidateBusinessRules(dto.Title, dto.DueDateUtc);
        var item = new ProjectItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            ProjectId = dto.ProjectId,
            AssignedUserId = dto.AssignedUserId,
            DueDateUtc = dto.DueDateUtc
        };

        var userName = user.Identity?.Name ?? "anonymous";
        var permissions = string.Join(",", user.Claims.Where(c => c.Type == "permission").Select(c => c.Value));
        _logger.LogInformation("Creating project item. Title={Title}, User={User}, Permissions={Permissions}", dto.Title, userName, permissions);

        var created = await _items.AddAsync(item, cancellationToken);
        InvalidateListCache();
        _logger.LogInformation("Project item created. Id={Id}, Title={Title}", created.Id, created.Title);

        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectItemDto dto, ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        ValidateBusinessRules(dto.Title, dto.DueDateUtc);
        var existing = await _items.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            _logger.LogWarning("Update failed: project item not found. Id={Id}", id);
            return false;
        }

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.Status = dto.Status;
        existing.Priority = dto.Priority;
        existing.ProjectId = dto.ProjectId;
        existing.AssignedUserId = dto.AssignedUserId;
        existing.DueDateUtc = dto.DueDateUtc;

        await _items.UpdateAsync(existing, cancellationToken);
        InvalidateListCache();

        _logger.LogInformation(
            "Project item updated. Id={Id}, User={User}, Role={Role}",
            id,
            user.Identity?.Name ?? "anonymous",
            user.FindFirst(ClaimTypes.Role)?.Value ?? "none");

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _items.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed: project item not found. Id={Id}", id);
            return false;
        }

        InvalidateListCache();
        _logger.LogInformation("Project item deleted. Id={Id}", id);
        return true;
    }

    private void InvalidateListCache()
    {
        _cache.Remove(AllItemsCacheKey);
        _logger.LogInformation("Project item list cache invalidated. Key={CacheKey}", AllItemsCacheKey);
    }

    private static ProjectItemDto MapToDto(ProjectItem item)
    {
        return new ProjectItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Status = item.Status.ToString(),
            Priority = item.Priority.ToString(),
            CreatedAtUtc = item.CreatedAtUtc,
            DueDateUtc = item.DueDateUtc,
            ProjectId = item.ProjectId,
            ProjectName = item.Project?.Name ?? string.Empty,
            AssignedUserId = item.AssignedUserId,
            AssignedUserName = item.AssignedUser?.FullName,
            CommentsCount = item.Comments.Count
        };
    }

    private static void ValidateBusinessRules(string title, DateTime? dueDateUtc)
    {
        if (title.Contains("forbidden", StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException("The title contains a forbidden word.");
        }

        if (dueDateUtc is not null && dueDateUtc.Value.Date < DateTime.UtcNow.Date)
        {
            throw new BusinessRuleException("Due date cannot be in the past.");
        }
    }
}

using ProjectBoard.Domain.Entities;

namespace ProjectBoard.Infrastructure.Data;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Projects.Any())
        {
            return;
        }

        var admin = new AppUser { FullName = "Admin User", Email = "admin@example.com", Role = "Admin" };
        var manager = new AppUser { FullName = "Manager User", Email = "manager@example.com", Role = "Manager" };
        var user = new AppUser { FullName = "Student User", Email = "student@example.com", Role = "User" };

        var project = new Project
        {
            Name = "Course Work Tracker",
            Description = "Board for tracking practical and laboratory assignments."
        };

        project.Items.Add(new ProjectItem
        {
            Title = "Create EF Core model",
            Description = "Define projects, users, items and comments with relationships.",
            Status = ProjectItemStatus.Done,
            Priority = Priority.High,
            AssignedUser = admin
        });
        project.Items.Add(new ProjectItem
        {
            Title = "Build API endpoints",
            Description = "Expose CRUD routes with DTOs and validation.",
            Status = ProjectItemStatus.InProgress,
            Priority = Priority.Critical,
            AssignedUser = manager
        });
        project.Items.Add(new ProjectItem
        {
            Title = "Add Razor Pages UI",
            Description = "Create list, details, create, edit and delete pages.",
            Status = ProjectItemStatus.New,
            Priority = Priority.Medium,
            AssignedUser = user
        });

        db.Users.AddRange(admin, manager, user);
        db.Projects.Add(project);
        await db.SaveChangesAsync();
    }
}

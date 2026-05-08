using ProjectBoard.Application.Services;
using ProjectBoard.Infrastructure.Data;
using ProjectBoard.Infrastructure.Repositories;
using ProjectBoard.Web.Auth;
using ProjectBoard.Web.Filters;
using ProjectBoard.Web.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";
var connectionString = provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
    ? builder.Configuration.GetConnectionString("SqlServerConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectItemRepository, ProjectItemRepository>();
builder.Services.AddScoped<IProjectItemService, ProjectItemService>();

builder.Services.AddAuthentication(DemoAuthenticationDefaults.Scheme)
    .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>(
        DemoAuthenticationDefaults.Scheme,
        _ => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditItems", policy =>
        policy.RequireClaim("permission", "items.edit"));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
    options.Filters.Add<ApiActionLoggingFilter>();
});

builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "ProjectBoard API",
        Version = "v1",
        Description = "Educational ASP.NET Core API for EF Core, Swagger, middleware, filters, caching, logging, Razor Pages and authorization."
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await DemoDataSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/ProjectItems"));

app.Run();


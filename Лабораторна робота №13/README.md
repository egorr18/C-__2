# ProjectBoard

Окремий ASP.NET Core проєкт для папки: Лабораторна робота №13.

## Реалізовано

- EF Core: сутності, DbContext, SQLite/SQL Server connection string, CRUD.
- Web API: controllers, DTO, validation, Swagger, routing, HTTP status codes.
- Razor Pages UI: list, details, create, edit, delete.
- Layered architecture: Domain, Application, Infrastructure, Web.
- Manual DTO mapping у сервісному шарі.
- Middleware для логування запитів і централізованої обробки помилок.
- MVC filters для логування дій і business exceptions.
- ILogger та IMemoryCache з cache hit/cache miss і cache invalidation.
- Demo authorization через headers: roles, claims, policies.

## Запуск

```powershell
dotnet restore
dotnet run
```

- Swagger: `https://localhost:7044/swagger`
- UI: `https://localhost:7044/ProjectItems`

Якщо поточний термінал ще не бачить `dotnet`, закрийте його і відкрийте знову або використайте:

```powershell
& "C:\Program Files\dotnet\dotnet.exe" run
```

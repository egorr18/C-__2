# Лабораторна робота №14

Фокус цієї роботи: Логування CRUD операцій, cache hit/cache miss, IMemoryCache та інвалідація кешу.

У цій папці лежить окремий ASP.NET Core проєкт `ProjectBoard`, який можна відкрити і запустити незалежно від інших папок.

## Запуск

```powershell
dotnet restore
dotnet run
```

Після запуску:

- Swagger: `https://localhost:7044/swagger`
- Razor Pages UI: `https://localhost:7044/ProjectItems`

Якщо порт зайнятий, змініть `Properties/launchSettings.json` або запустіть з іншим URL.

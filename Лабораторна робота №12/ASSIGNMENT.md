# Лабораторна робота №12

Фокус цієї роботи: Багатошарова архітектура, Application/Infrastructure/Web, AutoMapper, повернення DTO замість EF entity.

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

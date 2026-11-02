# API mínima + test xUnit

Se conserva la aplicación ASP.NET Core MVC original y se añade una API mínima:

- `GET /api/saludo` → HTTP 200 con el texto `Hola mundo`.
- `tests/App.Tests/SaludoTests.cs` → test de integración con xUnit y WebApplicationFactory.

## Requisitos

SDK de .NET 6 (o SDK compatible con compilación para net6.0 y runtime ASP.NET Core 6). Se necesita acceso a NuGet para restaurar paquetes la primera vez. .NET 6 ya no tiene soporte oficial.

## Ejecutar el test

Desde la carpeta `03-app-dotnet`:

```bash
dotnet test tests/App.Tests/App.Tests.csproj
```

## Ejecutar la aplicación

```bash
dotnet run
```

Abre la URL indicada por el servidor y añade `/api/saludo`.

El test usa un servidor de pruebas en memoria, sin iniciar la aplicación manualmente.

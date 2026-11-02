# API y test xUnit

La API respeta la estructura de controladores del proyecto MVC.

- `Controllers/SaludoController.cs`: endpoint `GET /api/saludo` que devuelve `Hola mundo`.
- `tests/App.Tests/SaludoTests.cs`: test xUnit que verifica HTTP 200 y el texto de respuesta.
- `.github/workflows/tests.yml`: ejecuta el test automáticamente al abrir o actualizar una Pull Request.

## Ejecutar tests

Desde la carpeta `03-app-dotnet`:

```bash
dotnet test tests/App.Tests/App.Tests.csproj
```

## Probar la API

```bash
dotnet run
```

Visita `http://localhost:PUERTO/api/saludo` con el puerto HTTP mostrado en consola.

Nota: esta implementación usa un controlador Web API, coherente con la estructura MVC existente, en lugar de Minimal API mediante `MapGet`.

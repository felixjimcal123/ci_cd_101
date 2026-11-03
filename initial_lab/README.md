# Rindus ROAD · Misión 01 — Laboratorio GitHub Actions + Terraform + Azure

Laboratorio incremental: API ASP.NET Core 10 + xUnit + Docker + GitHub Actions + Terraform + Azure Container Apps + OIDC.

> **IMPORTANTE:** el código está preparado como material inicial, pero no se ha compilado ni ejecutado en este entorno porque aquí no está instalado el SDK .NET. Valida todo localmente antes de utilizar recursos Azure. Ningún workflow despliega por sí solo al hacer `push`; `deploy-dev.yml` requiere ejecución manual y configuración OIDC. El workflow completo de aplicación de Terraform por CI/CD se construirá después de OIDC, no está incluido como operativo todavía.

## Paso 0 — primera tarea (sin Azure)

Requisitos: .NET SDK 10.0 y Git. Docker se utilizará en la lección 9. VS Code o Rider son opcionales.

```powershell
dotnet --version
dotnet restore tests/Integration.Api.Tests/Integration.Api.Tests.csproj
dotnet test tests/Integration.Api.Tests/Integration.Api.Tests.csproj -c Release
```

Arranca la API:

```powershell
dotnet run --project src/Integration.Api/Integration.Api.csproj
```

Prueba `http://localhost:5080/health` y `http://localhost:5080/api/articles/42`.

**Criterio de terminado:** `dotnet test` devuelve éxito y `/health` responde HTTP 200. Trae la salida del comando y tu explicación de por qué estas pruebas serán el gate de la PR.

## Archivos

```text
src/Integration.Api/               API (sin BD ni colas)
tests/Integration.Api.Tests/        tests unitarios y de endpoints
Dockerfile                          build multistage para lección 9
.github/workflows/ci.yml            PR -> tests + artefacto
.github/workflows/image-ghcr.yml    main -> imagen a GHCR con SHA
.github/workflows/terraform-ci.yml  PR -> fmt/init/validate sin Azure
.github/workflows/deploy-dev.yml    manual -> OIDC -> Azure -> smoke
infra/                              Terraform para Azure Container Apps
infra/modules/container_app/        módulo Terraform reutilizable
docs/RUTA.md                        qué hacer en cada clase
docs/AZURE.md                       preparación de suscripción, backend y costes
docs/RUNBOOK.md                     diagnóstico y recuperación
docs/EVIDENCIAS.md                  registro demostrable de la misión
```

**Secuencia:** paso 0 -> clase 9 -> clase 15 (infraestructura y diseño, sin credenciales antiguas) -> 20 y 21 -> 22 y 23 -> despliegue Azure / validación / recuperación. Sigue `docs/RUTA.md`.

Al ejecutar Terraform localmente, **versiona `.terraform.lock.hcl`** para fijar las versiones y checksums de proveedores. No se incluyen contraseñas, credenciales de Azure ni suscripciones reales. Nunca subas archivos `*.tfstate`, `*.tfplan` o `terraform.tfvars` al repositorio.

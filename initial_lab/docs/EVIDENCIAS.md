# Rindus ROAD · Mision 01 · Evidencias

Estado inicial: curso 2-6 discutidos conceptualmente; API/CI proporcionados como starter (NO probados).

| Capacidad | Estado | Evidencia / criterio |
|---|---|---|
| API y tests locales | Pendiente | salida `dotnet test` y HTTP 200 |
| PR CI + proteccion | Pendiente | workflow run + ruleset o comprobacion de merge |
| Fallo intencional de tests | Pendiente | PR roja que no permite merge |
| Publicar imagen GHCR por digest | Pendiente | URL/digest del artefacto publicado |
| Terraform fmt/validate/plan | Pendiente | plan revisado + justificacion de cambios |
| OIDC + permisos minimos | Pendiente | identidad federada y scope verificables (sin secretos) |
| Deploy Container Apps | Pendiente | revision exitosa + endpoint |
| Smoke y recuperacion | Pendiente | HTTP 200 + simulacion fallo y recuperacion |
| Reusable workflows | Pendiente | ejecucion de caller y reusable workflow |

Proxima tarea unica: `dotnet test` en local y comprobar `/health`.

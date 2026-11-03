# Runbook CI/CD — escenarios para diagnostico

## CI / PR
- Test fallido: localizar job, test, commit y condicion de branch protection. `workflow` fallido no garantiza que se impida el merge sin ruleset.
- Error de compilacion: confirmar SDK, TargetFramework y error de compilador; no alterar dependencias a ciegas.
- Accion externa: validar confianza y version; para uso empresarial, pin a commit SHA verificado.

## Imagen
- `Dockerfile` multistage y runtime no root. En PR build sin publicar; en main push usando `GITHUB_TOKEN` con `packages: write`.
- Imagen debe identificarse por digest, no por `latest`. GHCR publica solo si el proyecto es expresamente de practica.
- Antes de Azure comprobar que la imagen es accesible desde donde va a hacer pull (publica o autenticacion de registry).

## Terraform
- `terraform fmt`, `validate`, Checkov (agregar y calibrar), y plan NO son sinonimos.
- Revisar plan: creaciones/modificaciones/eliminaciones, cambios de acceso publico, costes e impacto en usuarios.
- Aprobacion: si un plan cambia durante revision, regenerar y aprobar uno nuevo. El backend de state no se crea en el mismo apply que lo necesita.
- State no es copia de seguridad de recursos. Nunca usar un tfstate antiguo como rollback cotidiano.
- Si `apply` falla: delimitar cambios reales, leer errores/logs, plan contra estado actual, corregir o revertir configuracion y verificar.

## CD de aplicacion
- Antes de despliegue: revisar que `image_ref` coincide con una imagen publicada y autorizada.
- Despues: `az containerapp revision list` + `/health` y un endpoint funcional.
- Si falla: leer revision y logs, contener impacto, redeploy de digest anterior conocido y volver a verificar. La pipeline actual NO hace rollback automatico: ejercicio posterior.

## OIDC
- Si falla login: revisar `permissions.id-token`, IDs, subject GitHub (branch vs environment), audience, asignacion RBAC y scope real.
- Los IDs Azure no son secretos, pero no hay razon para difundirlos por el chat. El token OIDC es efimero, no permiso ilimitado.

## Evidencia
- Guardar logs de jobs, sha de commit, digest de imagen, resultado de tests, texto revisado del plan sin secretos, resultado de smoke, decisiones de permisos y correcciones.

# Ruta de aprendizaje por lecciones — Mision 01

**Metodo:** ver la clase -> examinar el material -> ejecutar una sola prueba -> explicar como funciona -> revisar un fallo/decision. Nada se marca demostrado sin logs o resultado.

## Pre-lab (ahora, antes de usar Azure)
- Ejecutar `dotnet test` y `dotnet run` localmente.
- Evidencia: 5 tests (2 de catalogo + 3 HTTP) verdes, `/health` HTTP 200.
- Sin Azure, GHCR ni gasto.

## Leccion 9 — Build and push docker image to container registry in Github Workflow
- Inspeccionar `Dockerfile` y `.github/workflows/image-ghcr.yml`.
- PR: CI valida tests y se puede construir imagen localmente con `docker build -t integration-api:lab .`.
- Push a main: workflow publica `ghcr.io/<owner>/<repo>:<SHA>`.
- Resultado: imagen identificable por digest; NO utilizar `latest` para CD.
- Importante: GHCR crea imagen privada por defecto; para este lab sin credenciales de pull se debe hacer publica manualmente (irreversible) o adaptar a ACR privado, que tiene coste. No hacer publica una imagen con secretos o codigo empresarial.

## Leccion 15 — Creating the CI/CD pipeline
- Leer `infra/` y `docs/AZURE.md`.
- Comparar los workflows: tests, imagen, Terraform (format/validate) y deploy.
- Entender bootstrap del state, `plan -out` -> review -> `apply tfplan`, image ownership y smoke.
- Por ahora NO desplegar: reservar ejecucion real para despues de OIDC. `terraform fmt -check -recursive infra`, `terraform init -backend=false` y `terraform validate` no exigen crear recursos Azure.

## Lecciones 20 y 21 — Reusable workflows
- Identificar duplicacion real y convertir un workflow en `workflow_call` con `inputs`, `secrets` y `permissions` delimitados.
- Evidencia: una PR que invoca el workflow reutilizable y pasa el mismo test.
- Evitar crear abstracciones antes de detectar duplicacion.

## Lecciones 22 y 23 — OIDC
- Crear identidad federada GitHub -> Entra para Environment `development`.
- Dar a identidad de CD `Container Apps Contributor` restringido a RG/recurso aplicable.
- Dar rol de Terraform a identidad IaC, separado, y `Storage Blob Data Contributor` SOLO al backend necesario.
- Configurar variables no secretas `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`, `AZURE_RESOURCE_GROUP`, `AZURE_CONTAINER_APP_NAME`.
- Validar `id-token: write` y reglas de proteccion del entorno.
- Primero crear infraestructura con `terraform plan -out=tfplan` + autorizacion + `apply tfplan` en entorno autorizado; despues invocar CD manual con imagen `ghcr.io/...@sha256:...`.
- Resultado: `/health` 200 desde Azure, un despliegue fallido diagnosticado y version anterior reimplementada si procede.

## Puerta de salida
- CI PR con tests / fallo intencionado; permisos correctos; imagen por digest; IaC format/validate/plan; OIDC con scope limitado; deploy aislado; smoke test; recuperacion probada; explicar decisiones y fallos.

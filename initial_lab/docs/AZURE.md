# Azure — Preparacion y seguridad del laboratorio (NO ejecutar antes de la leccion 15/22)

## 0. Costes, prerequisitos y decisiones

- Necesitas permisos para crear grupos de recursos y asignar roles (Owner/User Access Administrator o equivalentes para RBAC). No asumir que tu suscripcion los incluye.
- Elegimos Azure Container Apps Consumption (min_replicas=0, max_replicas=1), Log Analytics y 1 Storage Account Standard_LRS de backend. El Storage y los logs pueden generar cargos. No creamos SQL, AKS ni ACR.
- Establece un presupuesto y alertas (Azure Cost Management) antes de crear recursos. Un presupuesto no equivale a un limite de gasto tecnico.
- Usa exclusivamente recursos y suscripcion propios de pruebas; no recursos empresariales.

## 1. Datos que el alumno debe obtener (NO compartir secretos)

1. Subscription ID (puede compartirse parcialmente, mejor no es necesario en chat).
2. Tenant ID (no es una contrasena).
3. Region, p.ej. westeurope.
4. Permisos disponibles para crear RG y asignar RBAC.
5. GitHub owner/repo y decision imagen publica GHCR vs ACR privado.

```powershell
az login
az account list --output table
az account set --subscription '<YOUR_SUBSCRIPTION_ID>'
az account show --query '{name:name,id:id,tenantId:tenantId}' --output table
```

**Nunca compartas** `client_secret`, perfiles XML de publicacion, claves de Storage, tokens de acceso o archivos de state/plan. Si se publican, rotarlos.

## 2. Bootstrap del backend (manual y una sola vez)

El backend de Terraform debe EXISTIR antes de `terraform init`. La pipeline principal no puede depender de un storage que ella misma aun debe crear.

```powershell
# Elige un sufijo aleatorio para evitar colision de nombres globales
$stateRg = 'rg-roadlab-state'
$appRg   = 'rg-roadlab-dev'
$region  = 'westeurope'
$storage = 'stroadlab' + '<SUFIJO_UNICO_ALFANUMERICO>'

az group create --name $stateRg --location $region
az group create --name $appRg --location $region
az storage account create --name $storage --resource-group $stateRg --location $region --sku Standard_LRS --kind StorageV2 --min-tls-version TLS1_2 --allow-blob-public-access false
az storage container create --name tfstate --account-name $storage --auth-mode login
```

Si el ultimo comando devuelve autorizacion denegada, comprueba que tu usuario tenga `Storage Blob Data Contributor` sobre ese contenedor/cuenta; Azure RBAC puede tardar en propagarse. No copies claves para sortear permisos sin revisar el problema. Activa versionado/soft-delete del blob y revisa acceso/red del Storage antes de usar el backend con datos sensibles. Para un lab, los comandos mostrados son una base, no una configuracion enterprise completa.

## 3. Terraform local

Requisitos Terraform >=1.10; Azure CLI con `az login` y suscripcion correcta; cuenta con roles pertinentes.

```powershell
$env:ARM_SUBSCRIPTION_ID = (az account show --query id -o tsv)
cd infra
Copy-Item terraform.tfvars.example terraform.tfvars
# Edita valores de RG, prefijo, region e imagen GHCR publica por digest.
terraform fmt -check -recursive
terraform init -backend-config="resource_group_name=rg-roadlab-state" -backend-config="storage_account_name=$storage" -backend-config="container_name=tfstate" -backend-config="key=roadlab-dev.tfstate" -backend-config="use_azuread_auth=true" -backend-config="use_cli=true"
terraform validate
terraform plan -out=tfplan
terraform show tfplan
# Solo tras examinar costes, permisos y recursos del plan:
terraform apply tfplan
terraform output api_url
```

**ATENCION:** `tfplan` y `tfstate` pueden contener valores sensibles, no subirlos a artefactos publicos ni Git.

## 4. OIDC y permisos (lecciones 22/23)

- Crear App Registration/Service Principal en Entra ID y una credencial federada tipo GitHub Actions con subject EXACTO `repo:<OWNER>/<REPO>:environment:development`.
- GitHub `Environment` llamado `development`, con aprobaciones/restricciones que permita tu plan. Revisa limites del plan y repositorio.
- Una identidad de CD solo necesita administrar la app (rol Azure `Container Apps Contributor` en scope especifico).
- Terraform/IaC necesita permisos para crear log workspace/environment/app en RG, por lo que la identidad IaC requiere alcance distinto; si accede al backend, conceder `Storage Blob Data Contributor` en scope minimo. No otorgar `Owner` a todas las pipelines.
- En `.github/workflows/deploy-dev.yml` se usa `azure/login@v2` y `id-token: write` con repo vars; no `AZURE_CREDENTIALS` ni XML de publish profile.
- No ejecutar `deploy-dev.yml` sin cumplir todo lo anterior.

## 5. Limpieza

1. En el directorio `infra/`, crear/revisar `terraform plan -destroy`, despues `terraform destroy` con aprobacion consciente.
2. Confirmar que se eliminaron la Container App, entorno y logs. El RG de la app se creo fuera de Terraform y puede eliminarse tras comprobar que esta vacio.
3. Solo AL FINAL, tras verificar que no necesitas recuperar el estado, eliminar de forma controlada el RG del backend (eso destruye almacenamiento y versiones). No borres el state primero ni lo restaures para efectuar un rollback.
4. Confirmar que no queda facturacion residual y revocar credenciales federadas/roles no necesarios.

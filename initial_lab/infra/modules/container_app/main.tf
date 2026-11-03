resource "azurerm_container_app" "api" {
  name                         = var.name
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"

  ingress {
    external_enabled = true
    target_port      = 8080
    transport        = "auto"

    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "integration-api"
      image  = var.container_image
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }

  lifecycle {
    # La pipeline de la aplicacion gestiona la imagen; Terraform gestiona el resto.
    # Esta separacion evita que terraform apply revierta cada despliegue de app.
    ignore_changes = [template[0].container[0].image]
  }

  tags = {
    project = "rindus-road"
    stage   = "development"
  }
}

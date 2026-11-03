# El RG es un limite de seguridad preparado por bootstrap, no lo recreamos en cada deploy.
data "azurerm_resource_group" "lab" {
  name = var.resource_group_name
}

resource "azurerm_log_analytics_workspace" "lab" {
  name                = "${var.name_prefix}-logs-dev"
  location            = var.location
  resource_group_name = data.azurerm_resource_group.lab.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags = {
    project = "rindus-road"
    stage   = "development"
  }
}

resource "azurerm_container_app_environment" "lab" {
  name                       = "${var.name_prefix}-env-dev"
  location                   = var.location
  resource_group_name        = data.azurerm_resource_group.lab.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.lab.id
  tags = {
    project = "rindus-road"
    stage   = "development"
  }
}

module "api" {
  source                       = "./modules/container_app"
  name                         = "${var.name_prefix}-api-dev"
  resource_group_name          = data.azurerm_resource_group.lab.name
  container_app_environment_id = azurerm_container_app_environment.lab.id
  container_image              = var.container_image
}

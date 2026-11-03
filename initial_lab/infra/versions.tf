terraform {
  required_version = ">= 1.10.0, < 2.0.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }

  # El almacenamiento backend debe crearse antes de terraform init.
  # Configurar parametros por CLI: ver docs/AZURE.md
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
  # Los Resource Providers se registran por separado,
  # con una identidad autorizada a nivel de suscripción.
  # La pipeline de despliegue conserva permisos limitados al RG.
  resource_provider_registrations = "none"
}

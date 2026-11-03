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
}

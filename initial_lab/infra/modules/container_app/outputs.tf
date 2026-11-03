output "name" {
  value = azurerm_container_app.api.name
}

output "fqdn" {
  value = azurerm_container_app.api.ingress[0].fqdn
}

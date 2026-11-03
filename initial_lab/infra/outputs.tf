output "container_app_name" {
  value = module.api.name
}

output "api_url" {
  value = "https://${module.api.fqdn}"
}

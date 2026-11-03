variable "resource_group_name" {
  description = "Resource group ya creado para el laboratorio. Limita el alcance RBAC."
  type        = string
}

variable "location" {
  description = "Region Azure para la infraestructura de la app."
  type        = string
  default     = "northeurope"
}

variable "name_prefix" {
  description = "Prefijo corto y unico (minusculas y guiones) para los nombres del lab."
  type        = string
  default     = "roadlab"
}

variable "container_image" {
  description = "Imagen GHCR PUBLICA creada en la clase 9, preferentemente por digest."
  type        = string
}

terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version         = "=1.29"
  subscription_id = "a3fa6f62-77b0-4b51-8017-5f1496d0a7ff"
}

variable "prefix" {
  default = "bluJournal"
}

variable "env" {}

resource "azurerm_resource_group" "rg" {
  name     = "${var.prefix}-${var.env}-rg"
  location = "East US"
}

module "app-insights" {
  source   = "dfar-io/app-insights/azurerm"
  version  = "1.0.1"
  location = "${azurerm_resource_group.rg.location}"
  name     = "${var.prefix}-${var.env}-rg"
  rg_name  = "${azurerm_resource_group.rg.name}"
}

module "function-app" {
  source                 = "dfar-io/function-app/azurerm"
  version                = "1.4.0"
  function_app_name      = "blujournal"
  function_app_plan_name = "${var.prefix}-${var.env}-asp"
  rg_location            = "${azurerm_resource_group.rg.location}"
  rg_name                = "${azurerm_resource_group.rg.name}"
  storage_account_name   = "${lower(substr(var.prefix, 0, 15))}${lower(var.env)}fa"
  storage_account_kind   = "StorageV2"

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = "${module.app-insights.instrumentation_key}"
  }
}

output "StorageAccount" {
  value = "Turn on static website in storage account."
}

output "StorageAccountKey" {
  value = "Add storage account key to Jenkins - ${module.function-app.storage_account_primary_access_key}"
}

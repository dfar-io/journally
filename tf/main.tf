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
  source                                    = "dfar-io/function-app/azurerm"
  version                                   = "1.5.0"
  function_app_name                         = "blujournal"
  function_app_plan_name                    = "${var.prefix}-${var.env}-asp"
  rg_location                               = "${azurerm_resource_group.rg.location}"
  rg_name                                   = "${azurerm_resource_group.rg.name}"
  storage_account_name                      = "${lower(substr(var.prefix, 0, 15))}${lower(var.env)}fa"
  storage_account_kind                      = "StorageV2"
  storage_account_enable_https_traffic_only = "true"

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = "${module.app-insights.instrumentation_key}"
  }
}

module "key-vault" {
  source            = "dfar-io/key-vault/azurerm"
  version           = "1.0.0"
  keyvault_admin_id = "8443b328-b32b-4626-b306-29762939ff37"
  name              = "${var.prefix}-${var.env}-kv"
  rg_location       = "${azurerm_resource_group.rg.location}"
  rg_name           = "${azurerm_resource_group.rg.name}"
  tenant_id         = "f3cd45d5-927c-47e7-838a-f48b95dc4fd7"
}

resource "azurerm_cdn_profile" "cdn" {
  name                = "${var.prefix}-${var.env}-cdn"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  sku                 = "Premium_Verizon"
}

resource "azurerm_cdn_endpoint" "endpoint" {
  name                          = "${var.prefix}-${var.env}-cdnEndpoint"
  profile_name                  = "${azurerm_cdn_profile.cdn.name}"
  location                      = "${azurerm_resource_group.rg.location}"
  resource_group_name           = "${azurerm_resource_group.rg.name}"
  querystring_caching_behaviour = "NotSet"

  origin {
    name      = "blujournal"
    host_name = "blujournalprodfa.z13.web.core.windows.net"
  }
}

output "StorageAccount" {
  value = "Turn on static website in storage account."
}

output "StorageAccountKey" {
  value = "Add storage account key to Jenkins - ${module.function-app.storage_account_primary_access_key}"
}

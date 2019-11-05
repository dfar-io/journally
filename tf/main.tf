terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version         = "=1.29"
  subscription_id = "a3fa6f62-77b0-4b51-8017-5f1496d0a7ff"
}

variable "prefix" {
  default = "journally"
}

variable "env" {}

resource "azurerm_resource_group" "rg" {
  name     = "${var.prefix}-${var.env}-rg"
  location = "East US"
}

resource "random_string" "dbServerPassword" {
  length  = 128
  special = true
}

module "app-insights" {
  source   = "dfar-io/app-insights/azurerm"
  version  = "1.0.1"
  location = "${azurerm_resource_group.rg.location}"
  name     = "${var.prefix}-${var.env}-ai"
  rg_name  = "${azurerm_resource_group.rg.name}"
  web_tests = {
    "${var.prefix}-${var.env}-uptime" = "https://journally.io"
  }
}

module "function-app" {
  source                                    = "dfar-io/function-app/azurerm"
  version                                   = "1.5.0"
  function_app_name                         = "journally"
  function_app_plan_name                    = "${var.prefix}-${var.env}-asp"
  rg_location                               = "${azurerm_resource_group.rg.location}"
  rg_name                                   = "${azurerm_resource_group.rg.name}"
  storage_account_name                      = "${lower(substr(var.prefix, 0, 15))}${lower(var.env)}fa"
  storage_account_kind                      = "StorageV2"
  storage_account_enable_https_traffic_only = "true"

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = "${module.app-insights.instrumentation_key}"
    JOURNALLY_CONN_STR             = "Server=tcp:${module.sql-server.sqlserver_name}.database.windows.net:1433;Initial Catalog=Journally;Persist Security Info=False;User ID=${module.sql-server.sqlserver_administrator_login};Password=${module.sql-server.sqlserver_administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    JOURNALLY_JWT_SECRET           = "${random_string.dbServerPassword.result}"
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

module "sql-server" {
  source    = "dfar-io/sql-server/azurerm"
  version   = "1.1.1"
  name      = "${var.prefix}-${var.env}-sql"
  location  = "${azurerm_resource_group.rg.location}"
  rg_name   = "${azurerm_resource_group.rg.name}"
  databases = ["journally"]
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
  origin_host_header            = "journallyprodfa.z13.web.core.windows.net"

  origin {
    name      = "journally"
    host_name = "journallyprodfa.z13.web.core.windows.net"

  }
}

output "StorageAccount" {
  value = "1. Turn on static website in storage account."
}

output "StorageAccountKey" {
  value = "2. Add storage account to Jenkins:\nStorage Account Name: ${lower(substr(var.prefix, 0, 15))}${lower(var.env)}fa\nStorage Account Key: ${module.function-app.storage_account_primary_access_key}\nID: journally"
}


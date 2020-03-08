import * as pulumi from "@pulumi/pulumi";
import * as azure from "@pulumi/azure";
import * as config from "./config";

// Create an Azure Resource Group
export const resourceGroup = new azure.core.ResourceGroup(config.resourceGroup, {
  location: config.location,
  name: config.resourceGroup
});


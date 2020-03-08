import * as pulumi from "@pulumi/pulumi";
import * as azure from "@pulumi/azure";

const config = new pulumi.Config();

export const resourceGroup = config.get("resouceGroup") || "lab";
export const location = config.get("location") || azure.Locations.CentralUS;

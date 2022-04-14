using System;
using Pulumi;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Network;
using Inputs = Pulumi.AzureNative.Network.Inputs;
using System.Text.Json;

namespace Resources
{
    class TemplateSubNet
    {

        public TemplateSubNet()
        {
        }
        public Subnet Create(JsonElement sNet, string appServicePlanName, string location, string resourceName,
            InputMap<string> tags)
        {
            string appSubNetName = $"snet-{appServicePlanName}";
            var appSubNetTemplateIAC = new Subnet(appSubNetName, new SubnetArgs
            {
                SubnetName = appSubNetName,
                VirtualNetworkName = sNet.GetProperty("vnetName").ToString(),
                ResourceGroupName = sNet.GetProperty("rgName").ToString(),
                AddressPrefix = sNet.GetProperty("prefixesSubnet").ToString(),
                ServiceEndpoints =
            {
                new Inputs.ServiceEndpointPropertiesFormatArgs
                {
                    Service = "Microsoft.Sql",
                },
                new Inputs.ServiceEndpointPropertiesFormatArgs
                {
                    Service = "Microsoft.Web",
                }
            },
                Delegations =  {
                new Inputs.DelegationArgs {
                    Name = $"delegation-{appSubNetName}",
                    ServiceName = "Microsoft.Web/serverFarms"
                    }
                }
            });
            return appSubNetTemplateIAC;
        }

    }
}

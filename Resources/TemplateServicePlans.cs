using System;
using Pulumi;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using System.Text.Json;

namespace Resources
{
    public class TemplateServicePlans 
    {

        public TemplateServicePlans()
        { 

        }
        public AppServicePlan Create(JsonElement splans, string location, string resourceName, 
            InputMap<string> tags)
        {
            string appServicePlanName = splans.GetProperty("name").ToString();
            var appPlanTemplateIAC = new AppServicePlan(appServicePlanName, new AppServicePlanArgs
            {
                Name = appServicePlanName,
                ResourceGroupName = resourceName,
                Location = location,
                Kind = splans.GetProperty("kind").ToString(),
                Sku = new SkuDescriptionArgs
                {
                    Name = splans.GetProperty("name").ToString(),
                    Size = splans.GetProperty("size").ToString(),
                    Tier = splans.GetProperty("tier").ToString(),
                    Capacity = Convert.ToInt32(splans.GetProperty("capacity").ToString())
                },
                Tags = tags
            });
            return appPlanTemplateIAC;            
        }        
    }
}
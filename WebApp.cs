using System;
using Configs;
using Resources;
using System.Text.Json;
using Pulumi;

namespace Template.IaC
{
    public class WebApp
    {
        public WebApp()
        {
        }

        public void Create()
        {

            // Variable that generates the Pulumi Config (ex. Pulumi.dev.yaml)
            var config = new Config();
            // Variable that takes the Stack that is currently being used
            var environment = Pulumi.Deployment.Instance.StackName;
            // Variable that takes the resource from Pulumi Config
            var resource = config.RequireObject<JsonElement>("resource");
            // Variable that takes the azure-native from Pulumi Config
            var azureNative = config.RequireObject<JsonElement>("azure-native");
            // Variable that takes the name from resource
            var resourceName = resource.GetProperty("name").ToString();
            // Variable that takes the location from azure-native
            var location = azureNative.GetProperty("location").ToString();

            // Variable that takes info from Pulumi Config and sends it to the constructor GetTags
            var tags = CustomConfigs.GetTags(config.RequireObject<JsonElement>("tagSettings"), environment);
            
            try {
                // Variable that takes serviceplans from Pulumi Config
                var sPlans = config.RequireObject<JsonElement>("serviceplans");
            
                foreach (var i in sPlans.EnumerateArray())
                {

                    var servicePlans = new TemplateServicePlans(); var appPlan =
                        servicePlans.Create(i.GetProperty("serviceplan"), location, resourceName, tags);

                    var webApp = new TemplateWebApp();
                    // Variable that takes webapps from serviceplan
                    var wApps = i.GetProperty("serviceplan").GetProperty("webapps");

                    foreach (var j in wApps.EnumerateArray())
                    {
                        webApp.Create(j, appPlan, location, resourceName, tags);
                    }

                    // Variable that takes the name from serviceplan
                    var appPlanName = i.GetProperty("serviceplan").GetProperty("name").ToString();

                    // Variable that takes the snetConfigs from Pulumi Config
                    var sNet = config.RequireObject<JsonElement>("snetConfigs");
                    foreach (var k in sNet.EnumerateArray())
                    {
                        var subNet = new TemplateSubNet();
                        subNet.Create(k.GetProperty("snetConfig"), appPlanName, location, resourceName, tags);
                    }
                }
            } 
            catch (Exception exc)
            {
                System.Console.WriteLine($"The webapp will not be created because the serviceplan is out of yaml or yaml is broken {exc.Message}");
            }
        }
    }
}
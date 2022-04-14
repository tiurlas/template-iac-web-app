using Pulumi;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using System.Text.Json;

namespace Resources
{
    public class TemplateWebApp {

        public TemplateWebApp()
        {

        }
        public WebApp Create(JsonElement webapps, AppServicePlan appPlan, string location, 
            string resourceName, InputMap<string> tags)
        {
            string appServiceWebName = webapps.GetProperty("name").ToString();

            var appWebTemplateIAC = new WebApp(appServiceWebName, new WebAppArgs
            {
                Name = appServiceWebName,
                ResourceGroupName = resourceName,
                ServerFarmId = appPlan.Id,
                Location = location,
                SiteConfig = new SiteConfigArgs
                {
                    AppSettings = {
                    new NameValuePairArgs {
                        Name = "WEBSITE_TIME_ZONE",
                        Value ="E. South America Standard Time"
                    }
                }
                },
                Tags = tags
            }, new CustomResourceOptions  { DependsOn = appPlan } );
            return appWebTemplateIAC;
        }
    }
}
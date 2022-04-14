using Pulumi;
using System.Text.Json;

namespace Configs
{
    class CustomConfigs
    {
        public static InputMap<string> GetTags(JsonElement tags, string environment)
        {
            return new InputMap<string>
            {
                { "env", environment },
                { "createdby", tags.GetProperty("createdby").ToString() },
                { "keeper", tags.GetProperty("keeper").ToString() },
                { "managedby", tags.GetProperty("managedby").ToString() },
                { "product", tags.GetProperty("product").ToString() },
                { "access", tags.GetProperty("access").ToString() },
                { "repo", tags.GetProperty("repo").ToString() }
            };
        }
    }
}
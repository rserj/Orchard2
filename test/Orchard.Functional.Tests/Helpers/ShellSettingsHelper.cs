using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchard.Environment.Shell;

namespace Orchard.Functional.Tests
{
    public static class ShellSettingsHelper
    {
        public static ShellSettings GetShellSettings(string name)
        {
            var preShellSettings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(
                File.ReadAllText(Path.Combine(EnvironmentHelpers.GetApplicationPath(), "tenants.json")));

            var shellSettings = new List<ShellSettingsWithTenants>();
            foreach (var preShellSetting in preShellSettings)
            {
                var shellSetting = JObject.FromObject(preShellSetting.Value).ToObject<ShellSettingsWithTenants>();

                shellSetting.Name = preShellSetting.Key;

                foreach (var vals in preShellSetting.Value)
                {
                    shellSetting[vals.Key] = vals.Value.ToString();
                }

                shellSettings.Add(shellSetting);
            }

            return shellSettings.Single(x => x.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}

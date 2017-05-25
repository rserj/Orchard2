using System.IO;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using Orchard.Environment.Shell;

namespace Orchard.GoogleCloud
{
    public class GoogleShellSettings
    {
        public GoogleShellSettings(ShellSettings shellSettings)
        {
            var googleCloudSection = shellSettings["googlecloud"];
            var rawCredentials = JObject.Parse(googleCloudSection)["credentials"];
            ProjectId = rawCredentials.Value<string>("project_id");

            Credentials = CreateCredential(rawCredentials.ToString());
        }

        public string ProjectId { get; private set; }

        public GoogleCredential Credentials { get; private set; }

        private static GoogleCredential CreateCredential(string value)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                stream.Position = 0;
                return GoogleCredential.FromStream(stream);
            }
        }
    }
}

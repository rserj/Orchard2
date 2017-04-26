using System.Threading.Tasks;
using Coypu;
using Xunit;

namespace Orchard.Functional.Tests
{
    public class SetupTests
    {
        [Fact]
        public async Task X() {
            using (var deployer = ApplicationDeployerFactoryHelpers.Create())
            {
                var deploymentResult = await deployer.DeployAsync();

                var sessionConfiguration = new SessionConfiguration
                {
                    AppHost = deploymentResult.ApplicationBaseUri,
                    Browser = Coypu.Drivers.Browser.Chrome,
                    Port = 5000
                };

                using (var browser = new BrowserSession(sessionConfiguration))
                {
                    browser.Visit("/");

                }
            }
        }
    }
}

using System.Threading.Tasks;
using Coypu;
using Xunit;

namespace Orchard.Functional.Tests
{
    public class SetupTests
    {
        [Fact]
        public async Task RunSmokeTests() {
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
                    SetupWithSqlCe(browser);



                }
            }
        }

        public void SetupWithSqlCe(BrowserSession browser) {
            browser.Visit("/");

            browser.FillIn("What is the name of your site?").With("Orchard Smoke Tests");

            browser.Select("Sqlite").From("What type of database to use?");

            browser.FillIn("User name").With("stAdmin");
            browser.FillIn("Email").With("smoke_tests_admin@orchardproject.net");
            browser.FillIn("Password").With("Password123_");
            browser.FillIn("Password Confirmation").With("Password123_");
            browser.ClickButton("Finish Setup");

        }
    }
}

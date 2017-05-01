using System.Threading.Tasks;
using Coypu;
using Xunit;
using Microsoft.AspNetCore.Server.IntegrationTesting;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Testing.xunit;

namespace Orchard.Functional.Tests
{
    public class SmokeTests
    {
        public class SmokeTests_X64
        {
            [ConditionalTheory, Trait("E2Etests", "Smoke")]
            [OSSkipCondition(OperatingSystems.Linux)]
            [OSSkipCondition(OperatingSystems.MacOSX)]
            //[InlineData(ServerType.WebListener, RuntimeFlavor.Clr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            //[InlineData(ServerType.WebListener, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            //[InlineData(ServerType.WebListener, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Standalone)]
            //[InlineData(ServerType.Kestrel, RuntimeFlavor.Clr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            [InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            //[InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Standalone)]
            //[InlineData(ServerType.IISExpress, RuntimeFlavor.Clr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            //[InlineData(ServerType.IISExpress, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            //[InlineData(ServerType.IISExpress, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Standalone)]
            public async Task WindowsOS(
                ServerType serverType,
                RuntimeFlavor runtimeFlavor,
                RuntimeArchitecture architecture,
                ApplicationType applicationType)
            {
                var smokeTestRunner = new SmokeTests();
                await smokeTestRunner.SmokeTestSuite(serverType, runtimeFlavor, architecture, applicationType);
            }

            //[ConditionalTheory, Trait("E2Etests", "Smoke")]
            //[OSSkipCondition(OperatingSystems.Windows)]
            //[InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Portable)]
            //[InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, ApplicationType.Standalone)]
            //public async Task NonWindowsOS(
            //    ServerType serverType,
            //    RuntimeFlavor runtimeFlavor,
            //    RuntimeArchitecture architecture,
            //    ApplicationType applicationType)
            //{
            //    var smokeTestRunner = new SmokeTests();
            //    await smokeTestRunner.SmokeTestSuite(serverType, runtimeFlavor, architecture, applicationType);
            //}
        }

        public async Task SmokeTestSuite(
            ServerType serverType,
            RuntimeFlavor runtimeFlavor,
            RuntimeArchitecture architecture,
            ApplicationType applicationType,
            bool noSource = false)
        {
            var deploymentParameters = new DeploymentParameters(
               EnvironmentHelpers.GetApplicationPath(), serverType, runtimeFlavor, architecture)
            {
                EnvironmentName = "OrchardTesting",
                ServerConfigTemplateContent = (serverType == ServerType.IISExpress) ? File.ReadAllText("Http.config") : null,
                SiteName = "Orchard Core Web",
                PublishApplicationBeforeDeployment = false,
                PreservePublishedApplicationForDebugging = false,
                TargetFramework = runtimeFlavor == RuntimeFlavor.Clr ? "net46" : "netcoreapp1.1",
                Configuration = EnvironmentHelpers.GetCurrentBuildConfiguration(),
                ApplicationType = applicationType,
                UserAdditionalCleanup = parameters =>
                {
                    Directory.Delete(Path.Combine(EnvironmentHelpers.GetApplicationPath(), "App_Data"), true);
                }
            };


            using (var deployer = ApplicationDeployerFactory.Create(deploymentParameters, new LoggerFactory()))
            {
                var deploymentResult = await deployer.DeployAsync();

                RunSmokeTests(deploymentResult);
            }
        }

        public void RunSmokeTests(DeploymentResult deploymentResult) {
            var sessionConfiguration = new SessionConfiguration
            {
                AppHost = deploymentResult.ApplicationBaseUri,
                Browser = Coypu.Drivers.Browser.Chrome,
                Port = 5000
            };

            using (var browser = new BrowserSession(sessionConfiguration))
            {
                SetupWithSqlCe(browser);

                LoginWithAdmin(browser);

                var urlOne = CreatePage(browser, "New Test Page");
                var urlTwo = CreatePage(browser, "New Test Page 2");

                browser.Visit(urlOne);
                Assert.True(browser.HasContent("New Test Page"));

                browser.Visit(urlTwo);
                Assert.True(browser.HasContent("New Test Page 2"));
            }
        }

        public void SetupWithSqlCe(BrowserSession browser)
        {
            browser.Visit("/");

            browser.FillIn("What is the name of your site?").With("Orchard Smoke Tests");

            browser.Select("Sqlite").From("What type of database to use?");

            browser.FillIn("User name").With("stAdmin");
            browser.FillIn("Email").With("smoke_tests_admin@orchardproject.net");
            browser.FillIn("Password").With("Password123_");
            browser.FillIn("Password Confirmation").With("Password123_");
            browser.ClickButton("Finish Setup");
        }

        public void LoginWithAdmin(BrowserSession browser)
        {
            browser.Visit("/Login");
            browser.FillIn("Username").With("stAdmin");
            browser.FillIn("Password").With("Password123_");
            browser.ClickButton("Log in");
            browser.Visit("/Admin");
        }

        public string CreatePage(BrowserSession browser, string name)
        {
            browser.FindId("new").Click();
            browser.ClickLink("Page");

            browser.FillIn("Title").With(name);
            // TODO: Test for auto generate link
            browser.FindCss(".trumbowyg-editor").SendKeys("Some test content");
            browser.ClickButton("Publish");

            return "/" + name.ToLowerInvariant().Replace(" ", "-");
        }
    }
}

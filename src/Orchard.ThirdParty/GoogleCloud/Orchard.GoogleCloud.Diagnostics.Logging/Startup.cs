using System;
using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orchard.Environment.Shell;

namespace Orchard.GoogleCloud.Diagnostics.Logging
{
    public class Startup : StartupBase
    {
        private readonly GoogleShellSettings _googleShellSettings;

        public Startup(
            ShellSettings shellSettings)
        {
            _googleShellSettings = new GoogleShellSettings(shellSettings);
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddGoogle(_googleShellSettings.ProjectId);
        }
    }
}

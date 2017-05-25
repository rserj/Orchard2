using System;
using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.Extensions.DependencyInjection;
using Orchard.Environment.Shell;

namespace Orchard.GoogleCloud.Diagnostics.Logging
{
    public static class Log4NetExtensions
    {
        public static Microsoft.Extensions.Logging.ILoggerFactory AddLog4Net(
            this Microsoft.Extensions.Logging.ILoggerFactory loggerFactory,
            IApplicationBuilder app)
        {
            // https://github.com/GoogleCloudPlatform/google-cloud-dotnet/blob/97ddf80e840b02539c5d1e3a8f1b1efa1499fefc/apis/Google.Cloud.Diagnostics.AspNetCore/Google.Cloud.Diagnostics.AspNetCore/Logging/GoogleLoggerFactoryExtensions.cs

            return null;
        }
    }
}

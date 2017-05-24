using System;
using System.Diagnostics;
using Google.Cloud.Trace.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Orchard.GoogleCloud.Diagnostics.Trace
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TraceServiceClient>(sp => {
                var clock = sp.GetService<Google.Api.Gax.IClock>();

                return Google.Cloud.Trace.V1.TraceServiceClient.Create(
                    settings: new TraceServiceSettings {
                        Clock = clock
                    }
                );
            });

            services.AddSingleton<GoogleCloudTraceListener>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var diagnosticListener = serviceProvider.GetRequiredService<DiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(serviceProvider.GetRequiredService<GoogleCloudTraceListener>());
        }
    }
}

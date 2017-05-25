using System;
using System.Diagnostics;
using Google.Cloud.Diagnostics.AspNetCore;
using Google.Cloud.Trace.V1;
using Grpc.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Orchard.Environment.Shell;

namespace Orchard.GoogleCloud.Diagnostics.Trace
{
    public class Startup : StartupBase
    {
        private readonly GoogleShellSettings _shellSettings;
        private readonly IClock _clock;

        public Startup(
            ShellSettings shellSettings,
            IClock clock)
        {
            _shellSettings = new GoogleShellSettings(shellSettings);
            _clock = clock;
        }

        public override void ConfigureServices(
            IServiceCollection services)
        {
            services.AddSingleton<GoogleCloudTraceListener>();

            services.AddGoogleTrace(_shellSettings.ProjectId,
                client: CreateTraceServiceClient(new GoogleClock(_clock)));
        }

        public override void Configure(
            IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // Add Automatic Trace Middleware
            app.UseGoogleTrace();

            // Add Diagnotic Listeners
            var diagnosticListener = serviceProvider
                .GetRequiredService<DiagnosticListener>();
            
            diagnosticListener.SubscribeWithAdapter(
                serviceProvider.GetRequiredService<GoogleCloudTraceListener>());
        }

        private TraceServiceClient CreateTraceServiceClient(Google.Api.Gax.IClock clock) {
            return Google.Cloud.Trace.V1.TraceServiceClient.Create(
                settings: new TraceServiceSettings
                {
                    Clock = clock,
                    PatchTracesSettings =
                        Google.Api.Gax.Grpc.CallSettings.FromCallCredentials(
                            _shellSettings.Credentials.ToCallCredentials()
                        ),
                    CallSettings =
                        Google.Api.Gax.Grpc.CallSettings.FromCallCredentials(
                            _shellSettings.Credentials.ToCallCredentials()
                        ),
                    GetTraceSettings =
                        Google.Api.Gax.Grpc.CallSettings.FromCallCredentials(
                            _shellSettings.Credentials.ToCallCredentials()
                        ),
                    ListTracesSettings =
                        Google.Api.Gax.Grpc.CallSettings.FromCallCredentials(
                            _shellSettings.Credentials.ToCallCredentials()
                        )
                        /* Scheduler : TODO: Do I need to set? */
                }
            );
        }
    }
}

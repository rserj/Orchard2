using Microsoft.AspNetCore.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Orchard.GoogleCloud.Core
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Google.Api.Gax.IClock, GoogleClock>();
        }
    }
}

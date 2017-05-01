using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Orchard.Environment.Shell;
using Orchard.Hosting.ShellBuilders;
using Xunit;

namespace Microsoft.AspNetCore.Modules.Tests
{
    public class ModularTenantRouterMiddlewareTests
    {
        [Fact]
        public async Task ShouldNotServeRequestIfTenantHasNotBeenResolved()
        {
            var url = "http://localhost:8000";
            var builder = new WebHostBuilder()
                .UseUrls(url)
                .ConfigureServices(conf => {
                    conf.AddScoped<IRunningShellTable, RunningShellTable>();
                    conf.AddScoped<IShellHost, StubShellHost>();
                })
                .Configure(applicationBuilder =>
                {
                    applicationBuilder.UseMiddleware<ModularTenantContainerMiddleware>();
                });

            var server = new TestServer(builder);
            var result = await server.CreateClient().GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("", await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ShouldReturnOkIfTenantResolved()
        {
            var url = "http://localhost:8000";
            var builder = new WebHostBuilder()
                .UseUrls(url)
                .ConfigureServices(conf => {
                    conf.AddScoped<IRunningShellTable, RunningShellTable>();
                    conf.AddScoped<IShellHost, StubShellHost>();
                })
                .Configure(applicationBuilder =>
                {
                    applicationBuilder.UseMiddleware<ModularTenantContainerMiddleware>();
                    applicationBuilder.UseMiddleware<OkMiddleware>();

                    applicationBuilder
                        .ApplicationServices
                        .GetService<IRunningShellTable>()
                        .Add(new ShellSettings { Name = "ShellOne", RequestUrlHost = "localhost" });
                });

            var server = new TestServer(builder);
            var result = await server.CreateClient().GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("ok", await result.Content.ReadAsStringAsync());
        }
    }

    public class StubShellHost : IShellHost
    {
        public Task<ShellContext> CreateShellContextAsync(ShellSettings settings)
        {
            throw new NotImplementedException();
        }

        public ShellContext GetOrCreateShellContext(ShellSettings settings)
        {
            return new ShellContext {
                Settings = settings,
                ServiceProvider = new ServiceCollection().BuildServiceProvider()
            };
        }

        public void Initialize()
        {
        }

        public IEnumerable<ShellContext> ListShellContexts()
        {
            throw new NotImplementedException();
        }

        public void ReloadShellContext(ShellSettings settings)
        {
            throw new NotImplementedException();
        }

        public void UpdateShellSettings(ShellSettings settings)
        {
            throw new NotImplementedException();
        }
    }

    public class OkMiddleware
    {
        private readonly RequestDelegate _next;

        public OkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await httpContext.Response.WriteAsync("ok");
        }
    }
}

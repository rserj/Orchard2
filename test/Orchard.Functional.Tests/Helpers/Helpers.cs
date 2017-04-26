using System;
using System.Net.Http;
using Microsoft.AspNetCore.Server.IntegrationTesting;

namespace Orchard.Functional.Tests
{
    public static class Helpers
    {
        public static string GiveMeATestEmail()
        {
            return $"{GiveMeATestId()}_test@orchardproject.com";
        }

        public static string GiveMeATestId() {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static HttpClient CreateHttpClient(DeploymentResult result, int timeoutTime)
        {
            var httpClientHandler = new HttpClientHandler();
            return new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(result.ApplicationBaseUri),
                Timeout = TimeSpan.FromMinutes(timeoutTime),
            };
        }
    }
}

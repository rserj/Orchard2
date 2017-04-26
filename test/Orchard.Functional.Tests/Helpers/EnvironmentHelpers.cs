using System;
using System.IO;

namespace Orchard.Functional.Tests
{
    public static class EnvironmentHelpers
    {
        public static string GetApplicationPath()
        {
            var current = new DirectoryInfo(AppContext.BaseDirectory);
            while (current != null)
            {
                if (File.Exists(Path.Combine(current.FullName, "Orchard.sln")))
                {
                    break;
                }
                current = current.Parent;
            }

            if (current == null)
            {
                throw new InvalidOperationException("Could not find the solution directory");
            }

            return Path.GetFullPath(Path.Combine(current.FullName, "src", "Orchard.Mvc.Web"));
        }

        public static string GetCurrentBuildConfiguration()
        {
            var configuration = "Debug";
            if (string.Equals(System.Environment.GetEnvironmentVariable("Configuration"), "Release", StringComparison.OrdinalIgnoreCase))
            {
                configuration = "Release";
            }

            return configuration;
        }
    }
}

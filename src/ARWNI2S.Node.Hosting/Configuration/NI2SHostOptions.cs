using ARWNI2S.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Reflection;

namespace ARWNI2S.Hosting.Configuration
{
    internal sealed class NI2SHostOptions
    {
        public NI2SHostOptions(IConfiguration primaryConfiguration, IConfiguration fallbackConfiguration = null, IHostEnvironment environment = null)
        {
            ArgumentNullException.ThrowIfNull(primaryConfiguration);

            string GetConfig(string key) => primaryConfiguration[key] ?? fallbackConfiguration?[key];

            EngineName = environment?.ApplicationName ?? GetConfig(NI2SHostDefaults.ApplicationKey) ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
            StartupAssembly = GetConfig(NI2SHostDefaults.StartupAssemblyKey);
            DetailedErrors = GetConfig(NI2SHostDefaults.DetailedErrorsKey).ParseBool();
            CaptureStartupErrors = GetConfig(NI2SHostDefaults.CaptureStartupErrorsKey).ParseBool();
            Environment = environment?.EnvironmentName ?? GetConfig(NI2SHostDefaults.EnvironmentKey);
            NodeRoot = GetConfig(NI2SHostDefaults.NodeRootKey);
            ContentRootPath = environment?.ContentRootPath ?? GetConfig(NI2SHostDefaults.ContentRootKey);
            PreventHostingStartup = GetConfig(NI2SHostDefaults.PreventHostingStartupKey).ParseBool();
            SuppressStatusMessages = GetConfig(NI2SHostDefaults.SuppressStatusMessagesKey).ParseBool();
            ServerUrls = GetConfig(NI2SHostDefaults.ServerUrlsKey);
            PreferHostingUrls = GetConfig(NI2SHostDefaults.PreferHostingUrlsKey).ParseBool();

            // Search the primary assembly and configured assemblies.
            HostingStartupAssemblies = Split(EngineName, GetConfig(NI2SHostDefaults.HostingStartupAssembliesKey));
            HostingStartupExcludeAssemblies = Split(GetConfig(NI2SHostDefaults.HostingStartupExcludeAssembliesKey));

            var timeout = GetConfig(NI2SHostDefaults.ShutdownTimeoutKey);
            if (!string.IsNullOrEmpty(timeout)
                && int.TryParse(timeout, NumberStyles.None, CultureInfo.InvariantCulture, out var seconds))
            {
                ShutdownTimeout = TimeSpan.FromSeconds(seconds);
            }
        }

        public string EngineName { get; }

        public bool PreventHostingStartup { get; }

        public bool SuppressStatusMessages { get; }

        public IReadOnlyList<string> HostingStartupAssemblies { get; }

        public IReadOnlyList<string> HostingStartupExcludeAssemblies { get; }

        public bool DetailedErrors { get; }

        public bool CaptureStartupErrors { get; }

        public string Environment { get; }

        public string StartupAssembly { get; }

        public string NodeRoot { get; }

        public string ContentRootPath { get; }

        public TimeSpan ShutdownTimeout { get; } = TimeSpan.FromSeconds(30);

        public string ServerUrls { get; }

        public bool PreferHostingUrls { get; }

        public IEnumerable<string> GetFinalHostingStartupAssemblies()
        {
            return HostingStartupAssemblies.Except(HostingStartupExcludeAssemblies, StringComparer.OrdinalIgnoreCase);
        }

        private static IReadOnlyList<string> Split(string value)
        {
            return value?.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                ?? [];
        }

        private static IReadOnlyList<string> Split(string applicationName, string environment)
        {
            if (string.IsNullOrEmpty(environment))
            {
                return [applicationName];
            }

            return Split($"{applicationName};{environment}");
        }
    }
}
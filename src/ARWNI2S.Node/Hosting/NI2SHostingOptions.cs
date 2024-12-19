using ARWNI2S.Engine.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Reflection;

namespace ARWNI2S.Node.Hosting
{
    internal sealed class NI2SHostingOptions
    {
        public NI2SHostingOptions(IConfiguration primaryConfiguration, IConfiguration fallbackConfiguration = null, IHostEnvironment environment = null)
        {
            ArgumentNullException.ThrowIfNull(primaryConfiguration);

            string GetConfig(string key) => primaryConfiguration[key] ?? fallbackConfiguration?[key];

            EngineName = environment?.ApplicationName ?? GetConfig(NI2SHostingDefaults.ApplicationKey) ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
            StartupAssembly = GetConfig(NI2SHostingDefaults.StartupAssemblyKey);
            DetailedErrors = GetConfig(NI2SHostingDefaults.DetailedErrorsKey).ParseBool();
            CaptureStartupErrors = GetConfig(NI2SHostingDefaults.CaptureStartupErrorsKey).ParseBool();
            Environment = environment?.EnvironmentName ?? GetConfig(NI2SHostingDefaults.EnvironmentKey);
            NI2SRoot = GetConfig(NI2SHostingDefaults.NI2SRootKey);
            ContentRootPath = environment?.ContentRootPath ?? GetConfig(NI2SHostingDefaults.ContentRootKey);
            PreventHostingStartup = GetConfig(NI2SHostingDefaults.PreventHostingStartupKey).ParseBool();
            SuppressStatusMessages = GetConfig(NI2SHostingDefaults.SuppressStatusMessagesKey).ParseBool();
            ServerUrls = GetConfig(NI2SHostingDefaults.ServerUrlsKey);
            PreferHostingUrls = GetConfig(NI2SHostingDefaults.PreferHostingUrlsKey).ParseBool();

            // Search the primary assembly and configured assemblies.
            HostingStartupAssemblies = Split(EngineName, GetConfig(NI2SHostingDefaults.HostingStartupAssembliesKey));
            HostingStartupExcludeAssemblies = Split(GetConfig(NI2SHostingDefaults.HostingStartupExcludeAssembliesKey));

            var timeout = GetConfig(NI2SHostingDefaults.ShutdownTimeoutKey);
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

        public string NI2SRoot { get; }

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
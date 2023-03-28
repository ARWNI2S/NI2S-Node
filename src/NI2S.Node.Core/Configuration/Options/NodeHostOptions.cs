using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Hosting;
using NI2S.Node.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NI2S.Node.Configuration
{
    internal sealed class NodeHostOptions
    {
        public NodeHostOptions(IConfiguration primaryConfiguration, IConfiguration fallbackConfiguration = null, IHostEnvironment environment = null)
        {
            if (primaryConfiguration is null)
            {
                throw new ArgumentNullException(nameof(primaryConfiguration));
            }

            string GetConfig(string key) => primaryConfiguration[key] ?? fallbackConfiguration?[key];

            ApplicationName = environment?.ApplicationName ?? GetConfig(NodeHostDefaults.ApplicationKey) ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
            StartupAssembly = GetConfig(NodeHostDefaults.StartupAssemblyKey);
            DetailedErrors = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.DetailedErrorsKey));
            CaptureStartupErrors = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.CaptureStartupErrorsKey));
            Environment = environment?.EnvironmentName ?? GetConfig(NodeHostDefaults.EnvironmentKey);
            AssetsRoot = GetConfig(NodeHostDefaults.AssetsRootKey);
            ContentRootPath = environment?.ContentRootPath ?? GetConfig(NodeHostDefaults.ContentRootKey);
            PreventHostingStartup = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.PreventHostingStartupKey));
            SuppressStatusMessages = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.SuppressStatusMessagesKey));
            // TODO: NODE PORTS
            //ServerUrls = GetConfig(NodeHostDefaults.ServerUrlsKey);
            // TODO: NODE PORTS
            //PreferHostingUrls = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.PreferHostingUrlsKey));

            // Search the primary assembly and configured assemblies.
            HostingStartupAssemblies = Split(ApplicationName, GetConfig(NodeHostDefaults.HostingStartupAssembliesKey));
            HostingStartupExcludeAssemblies = Split(GetConfig(NodeHostDefaults.HostingStartupExcludeAssembliesKey));

            var timeout = GetConfig(NodeHostDefaults.ShutdownTimeoutKey);
            if (!string.IsNullOrEmpty(timeout)
                && int.TryParse(timeout, NumberStyles.None, CultureInfo.InvariantCulture, out var seconds))
            {
                ShutdownTimeout = TimeSpan.FromSeconds(seconds);
            }
        }

        public string ApplicationName { get; }

        public bool PreventHostingStartup { get; }

        public bool SuppressStatusMessages { get; }

        public IReadOnlyList<string> HostingStartupAssemblies { get; }

        public IReadOnlyList<string> HostingStartupExcludeAssemblies { get; }

        public bool DetailedErrors { get; }

        public bool CaptureStartupErrors { get; }

        public string Environment { get; }

        public string StartupAssembly { get; }

        public string AssetsRoot { get; }

        public string ContentRootPath { get; }

        public TimeSpan ShutdownTimeout { get; } = TimeSpan.FromSeconds(5);

        // TODO: NODE PORTS
        //public string ServerUrls { get; }

        // TODO: NODE PORTS
        //public bool PreferHostingUrls { get; }

        public IEnumerable<string> GetFinalHostingStartupAssemblies()
        {
            return HostingStartupAssemblies.Except(HostingStartupExcludeAssemblies, StringComparer.OrdinalIgnoreCase);
        }

        private static IReadOnlyList<string> Split(string value)
        {
            return value?.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                ?? Array.Empty<string>();
        }

        private static IReadOnlyList<string> Split(string applicationName, string environment)
        {
            if (string.IsNullOrEmpty(environment))
            {
                return new[] { applicationName };
            }

            return Split($"{applicationName};{environment}");
        }
    }
}

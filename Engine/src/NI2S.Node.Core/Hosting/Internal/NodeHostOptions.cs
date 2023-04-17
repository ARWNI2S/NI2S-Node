// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NI2S.Node.Hosting.Internal
{
    internal sealed class NodeHostOptions
    {
        /* 001.3.1.1.1 */
        /* 001.3.2.1.1 */
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
            NodeRoot = GetConfig(NodeHostDefaults.NodeRootKey);
            ContentRootPath = environment?.ContentRootPath ?? GetConfig(NodeHostDefaults.ContentRootKey);
            PreventHostingStartup = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.PreventHostingStartupKey));
            SuppressStatusMessages = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.SuppressStatusMessagesKey));
            ServerUrls = GetConfig(NodeHostDefaults.ServerUrlsKey);
            PreferHostingUrls = StringUtilities.ParseBool(GetConfig(NodeHostDefaults.PreferHostingUrlsKey));

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

        public string NodeRoot { get; }

        public string ContentRootPath { get; }

        public TimeSpan ShutdownTimeout { get; } = TimeSpan.FromSeconds(5);

        public string ServerUrls { get; }

        public bool PreferHostingUrls { get; }

        /* 001.3.1.1.3 */
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
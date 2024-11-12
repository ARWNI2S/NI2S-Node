﻿using ARWNI2S.Infrastructure.Extensions;
using ARWNI2S.Runtime.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Reflection;

namespace ARWNI2S.Runtime.Configuration.Options
{
    internal sealed class NodeHostOptions
    {
        public NodeHostOptions(IConfiguration primaryConfiguration, IConfiguration fallbackConfiguration = null, IHostEnvironment environment = null)
        {
            ArgumentNullException.ThrowIfNull(primaryConfiguration);

            string GetConfig(string key) => primaryConfiguration[key] ?? fallbackConfiguration?[key];

            EngineName = environment?.ApplicationName ?? GetConfig(NodeHostDefaults.EngineKey) ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
            StartupAssembly = GetConfig(NodeHostDefaults.StartupAssemblyKey);
            DetailedErrors = GetConfig(NodeHostDefaults.DetailedErrorsKey).ParseBool();
            CaptureStartupErrors = GetConfig(NodeHostDefaults.CaptureStartupErrorsKey).ParseBool();
            Environment = environment?.EnvironmentName ?? GetConfig(NodeHostDefaults.EnvironmentKey);
            NodeRoot = GetConfig(NodeHostDefaults.NodeRootKey);
            ContentRootPath = environment?.ContentRootPath ?? GetConfig(NodeHostDefaults.ContentRootKey);
            PreventHostingStartup = GetConfig(NodeHostDefaults.PreventHostingStartupKey).ParseBool();
            SuppressStatusMessages = GetConfig(NodeHostDefaults.SuppressStatusMessagesKey).ParseBool();
            ServerUrls = GetConfig(NodeHostDefaults.ServerUrlsKey);
            PreferHostingUrls = GetConfig(NodeHostDefaults.PreferHostingUrlsKey).ParseBool();

            // Search the primary assembly and configured assemblies.
            HostingStartupAssemblies = Split(EngineName, GetConfig(NodeHostDefaults.HostingStartupAssembliesKey));
            HostingStartupExcludeAssemblies = Split(GetConfig(NodeHostDefaults.HostingStartupExcludeAssembliesKey));

            var timeout = GetConfig(NodeHostDefaults.ShutdownTimeoutKey);
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
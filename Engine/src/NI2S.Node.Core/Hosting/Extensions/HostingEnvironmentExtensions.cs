// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Hosting.Internal;
using System;
using System.IO;

namespace NI2S.Node.Hosting
{
    internal static class HostingEnvironmentExtensions
    {
        /* 001.3.2.1.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() -> configureAppAction(Context, _builder.Configuration) 
                         -> GetNodeHostBuilderContext(context) -> nodeHostBuilderContext.HostingEnvironment.Initialize(...) */
        internal static void Initialize(
            this INodeHostEnvironment hostingEnvironment,
            string contentRootPath,
            NodeHostOptions options,
            IHostEnvironment baseEnvironment = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            if (string.IsNullOrEmpty(contentRootPath))
            {
                throw new ArgumentException("A valid non-empty content root must be provided.", nameof(contentRootPath));
            }
            if (!Directory.Exists(contentRootPath))
            {
                throw new ArgumentException($"The content root '{contentRootPath}' does not exist.", nameof(contentRootPath));
            }

            hostingEnvironment.ApplicationName = baseEnvironment?.ApplicationName ?? options.ApplicationName;
            hostingEnvironment.ContentRootPath = contentRootPath;
            hostingEnvironment.ContentRootFileProvider = baseEnvironment?.ContentRootFileProvider ?? new PhysicalFileProvider(hostingEnvironment.ContentRootPath);

            var nodeRoot = options.NodeRoot;
            if (nodeRoot == null)
            {
                // Default to /niisroot if it exists.
                var niisroot = Path.Combine(hostingEnvironment.ContentRootPath, "niisroot");
                if (Directory.Exists(niisroot))
                {
                    hostingEnvironment.NodeRootPath = niisroot;
                }
            }
            else
            {
                hostingEnvironment.NodeRootPath = Path.Combine(hostingEnvironment.ContentRootPath, nodeRoot);
            }

            if (!string.IsNullOrEmpty(hostingEnvironment.NodeRootPath))
            {
                hostingEnvironment.NodeRootPath = Path.GetFullPath(hostingEnvironment.NodeRootPath);
                if (!Directory.Exists(hostingEnvironment.NodeRootPath))
                {
                    Directory.CreateDirectory(hostingEnvironment.NodeRootPath);
                }
                hostingEnvironment.NodeRootFileProvider = new PhysicalFileProvider(hostingEnvironment.NodeRootPath);
            }
            else
            {
                hostingEnvironment.NodeRootFileProvider = new NullFileProvider();
            }

            hostingEnvironment.EnvironmentName =
                baseEnvironment?.EnvironmentName ??
                options.Environment ??
                hostingEnvironment.EnvironmentName;
        }
    }
}

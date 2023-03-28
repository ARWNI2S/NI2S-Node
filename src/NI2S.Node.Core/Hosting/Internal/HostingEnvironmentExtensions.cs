using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration;
using System;
using System.IO;

namespace NI2S.Node.Hosting
{
    internal static class HostingEnvironmentExtensions
    {
        internal static void Initialize(this INodeHostEnvironment hostEnvironment, string contentRootPath, NodeHostOptions options)
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

            hostEnvironment.ApplicationName = options.ApplicationName;
            hostEnvironment.ContentRootPath = contentRootPath;
            hostEnvironment.ContentRootFileProvider = new PhysicalFileProvider(hostEnvironment.ContentRootPath);

            var assetsRoot = options.AssetsRoot;
            if (assetsRoot == null)
            {
                // Default to /niisroot if it exists.
                var niisroot = Path.Combine(hostEnvironment.ContentRootPath, "niisroot");
                if (Directory.Exists(niisroot))
                {
                    hostEnvironment.AssetsRootPath = niisroot;
                }
            }
            else
            {
                hostEnvironment.AssetsRootPath = Path.Combine(hostEnvironment.ContentRootPath, assetsRoot);
            }

            if (!string.IsNullOrEmpty(hostEnvironment.AssetsRootPath))
            {
                hostEnvironment.AssetsRootPath = Path.GetFullPath(hostEnvironment.AssetsRootPath);
                if (!Directory.Exists(hostEnvironment.AssetsRootPath))
                {
                    Directory.CreateDirectory(hostEnvironment.AssetsRootPath);
                }
                hostEnvironment.AssetsRootFileProvider = new PhysicalFileProvider(hostEnvironment.AssetsRootPath);
            }
            else
            {
                hostEnvironment.AssetsRootFileProvider = new NullFileProvider();
            }

            hostEnvironment.EnvironmentName =
                options.Environment ??
                hostEnvironment.EnvironmentName;
        }

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

            var assetsRoot = options.AssetsRoot;
            if (assetsRoot == null)
            {
                // Default to /niisroot if it exists.
                var niisroot = Path.Combine(hostingEnvironment.ContentRootPath, "niisroot");
                if (Directory.Exists(niisroot))
                {
                    hostingEnvironment.AssetsRootPath = niisroot;
                }
            }
            else
            {
                hostingEnvironment.AssetsRootPath = Path.Combine(hostingEnvironment.ContentRootPath, assetsRoot);
            }

            if (!string.IsNullOrEmpty(hostingEnvironment.AssetsRootPath))
            {
                hostingEnvironment.AssetsRootPath = Path.GetFullPath(hostingEnvironment.AssetsRootPath);
                if (!Directory.Exists(hostingEnvironment.AssetsRootPath))
                {
                    Directory.CreateDirectory(hostingEnvironment.AssetsRootPath);
                }
                hostingEnvironment.AssetsRootFileProvider = new PhysicalFileProvider(hostingEnvironment.AssetsRootPath);
            }
            else
            {
                hostingEnvironment.AssetsRootFileProvider = new NullFileProvider();
            }

            hostingEnvironment.EnvironmentName =
                baseEnvironment?.EnvironmentName ??
                options.Environment ??
                hostingEnvironment.EnvironmentName;
        }
    }
}

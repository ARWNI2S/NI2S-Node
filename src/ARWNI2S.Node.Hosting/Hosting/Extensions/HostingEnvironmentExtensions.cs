using ARWNI2S.Node.Configuration.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Extensions
{
    internal static class HostingEnvironmentExtensions
    {
#pragma warning disable CS0618 // Type or member is obsolete
        internal static void Initialize(this IHostingEnvironment hostingEnvironment, string contentRootPath, NodeHostOptions options)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentException.ThrowIfNullOrEmpty(contentRootPath);
            if (!Directory.Exists(contentRootPath))
            {
                throw new ArgumentException($"The content root '{contentRootPath}' does not exist.", nameof(contentRootPath));
            }

            hostingEnvironment.ApplicationName = options.EngineName;
            hostingEnvironment.ContentRootPath = contentRootPath;
            hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(hostingEnvironment.ContentRootPath);

            var webRoot = options.NodeRoot;
            if (webRoot == null)
            {
                // Default to /wwwroot if it exists.
                var wwwroot = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot");
                if (Directory.Exists(wwwroot))
                {
                    hostingEnvironment.NodeRootPath = wwwroot;
                }
            }
            else
            {
                hostingEnvironment.NodeRootPath = Path.Combine(hostingEnvironment.ContentRootPath, webRoot);
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
                options.Environment ??
                hostingEnvironment.EnvironmentName;
        }

        internal static void Initialize(
            this INodeHostEnvironment hostingEnvironment,
            string contentRootPath,
            NodeHostOptions options,
            IHostEnvironment baseEnvironment = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentException.ThrowIfNullOrEmpty(contentRootPath);
            if (!Directory.Exists(contentRootPath))
            {
                throw new ArgumentException($"The content root '{contentRootPath}' does not exist.", nameof(contentRootPath));
            }

            hostingEnvironment.ApplicationName = baseEnvironment?.ApplicationName ?? options.EngineName;
            hostingEnvironment.ContentRootPath = contentRootPath;
            hostingEnvironment.ContentRootFileProvider = baseEnvironment?.ContentRootFileProvider ?? new PhysicalFileProvider(hostingEnvironment.ContentRootPath);

            var webRoot = options.NodeRoot;
            if (webRoot == null)
            {
                // Default to /wwwroot if it exists.
                var wwwroot = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot");
                if (Directory.Exists(wwwroot))
                {
                    hostingEnvironment.NodeRootPath = wwwroot;
                }
            }
            else
            {
                hostingEnvironment.NodeRootPath = Path.Combine(hostingEnvironment.ContentRootPath, webRoot);
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

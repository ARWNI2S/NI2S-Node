using ARWNI2S.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal static class HostingEnvironmentExtensions
    {
        internal static void Initialize(this INiisHostEnvironment hostingEnvironment,
            string contentRootPath, NI2SHostingOptions options, IHostEnvironment baseEnvironment = null)
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

            var nodeRoot = options.NodeRoot;
            if (nodeRoot == null)
            {
                // Default to /Content if it exists.
                var rootPath = Path.Combine(hostingEnvironment.ContentRootPath, "Content");
                if (Directory.Exists(rootPath))
                {
                    hostingEnvironment.NodeRootPath = rootPath;
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

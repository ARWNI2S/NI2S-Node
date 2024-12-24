using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal static class HostingEnvironmentExtensions
    {
        internal static void Initialize(
            this INiisHostEnvironment hostingEnvironment,
            string contentRootPath,
            NI2SHostingOptions options,
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

            var webRoot = options.NI2SRoot;
            if (webRoot == null)
            {
                // Default to /Content if it exists.
                var rootPath = Path.Combine(hostingEnvironment.ContentRootPath, "Content");
                if (Directory.Exists(rootPath))
                {
                    hostingEnvironment.NI2SRootPath = rootPath;
                }
            }
            else
            {
                hostingEnvironment.NI2SRootPath = Path.Combine(hostingEnvironment.ContentRootPath, webRoot);
            }

            if (!string.IsNullOrEmpty(hostingEnvironment.NI2SRootPath))
            {
                hostingEnvironment.NI2SRootPath = Path.GetFullPath(hostingEnvironment.NI2SRootPath);
                if (!Directory.Exists(hostingEnvironment.NI2SRootPath))
                {
                    Directory.CreateDirectory(hostingEnvironment.NI2SRootPath);
                }
                hostingEnvironment.NI2SRootFileProvider = new PhysicalFileProvider(hostingEnvironment.NI2SRootPath);
            }
            else
            {
                hostingEnvironment.NI2SRootFileProvider = new NullFileProvider();
            }

            hostingEnvironment.EnvironmentName =
                baseEnvironment?.EnvironmentName ??
                options.Environment ??
                hostingEnvironment.EnvironmentName;
        }
    }
}

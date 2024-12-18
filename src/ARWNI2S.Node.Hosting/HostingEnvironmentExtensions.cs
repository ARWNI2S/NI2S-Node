using ARWNI2S.Hosting.Configuration;

namespace ARWNI2S.Hosting
{
    internal static class HostingEnvironmentExtensions
    {
#pragma warning disable CS0618 // Type or member is obsolete
        internal static void Initialize(this IHostingEnvironment hostingEnvironment, string contentRootPath, NI2SHostOptions options)
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

            var contentRoot = options.NodeRoot;
            if (contentRoot == null)
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
                hostingEnvironment.NodeRootPath = Path.Combine(hostingEnvironment.ContentRootPath, contentRoot);
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
            this INiisHostEnvironment hostingEnvironment,
            string contentRootPath,
            NI2SHostOptions options,
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
                // Default to /Content if it exists.
                var rootPath = Path.Combine(hostingEnvironment.ContentRootPath, "Content");
                if (Directory.Exists(rootPath))
                {
                    hostingEnvironment.NodeRootPath = rootPath;
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

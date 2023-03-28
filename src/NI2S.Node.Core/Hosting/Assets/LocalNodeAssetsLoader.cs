#nullable enable
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Reflection;

namespace NI2S.Node.Hosting.Assets
{
    /// <summary>
    /// Loader for local node assets
    /// </summary>
    public class LocalAssetsLoader
    {
        /// <summary>
        /// Configure the <see cref="INodeHostEnvironment"/> to use static web assets.
        /// </summary>
        /// <param name="environment">The application <see cref="INodeHostEnvironment"/>.</param>
        /// <param name="configuration">The host <see cref="IConfiguration"/>.</param>
        public static void UseLocalAssets(INodeHostEnvironment environment, IConfiguration configuration)
        {
            var manifest = ResolveManifest(environment, configuration);
            if (manifest != null)
            {
                using (manifest)
                {
                    UseLocalAssetsCore(environment, manifest);
                }
            }
        }

        internal static void UseLocalAssetsCore(INodeHostEnvironment environment, Stream manifest)
        {
            var staticWebAssetManifest = ManifestLocalAssetFileProvider.LocalAssetManifest.Parse(manifest);
            var provider = new ManifestLocalAssetFileProvider(
                staticWebAssetManifest,
                (contentRoot) => new PhysicalFileProvider(contentRoot));

            environment.AssetsRootFileProvider = new CompositeFileProvider(new[] { provider, environment.AssetsRootFileProvider });
        }

        internal static Stream? ResolveManifest(INodeHostEnvironment environment, IConfiguration configuration)
        {
            try
            {
                var candidate = configuration[NodeHostDefaults.LocalAssetsKey] ?? ResolveRelativeToAssembly(environment);
                if (candidate != null && File.Exists(candidate))
                {
                    return File.OpenRead(candidate);
                }

                // A missing manifest might simply mean that the feature is not enabled, so we simply
                // return early. Misconfigurations will be uncommon given that the entire process is automated
                // at build time.
                return default;
            }
            catch
            {
                return default;
            }
        }

        private static string? ResolveRelativeToAssembly(INodeHostEnvironment environment)
        {
            if (string.IsNullOrEmpty(environment.ApplicationName))
            {
                return null;
            }
            var assembly = Assembly.Load(environment.ApplicationName);
            var basePath = string.IsNullOrEmpty(assembly.Location) ? AppContext.BaseDirectory : Path.GetDirectoryName(assembly.Location);
            return Path.Combine(basePath!, $"{environment.ApplicationName}.staticwebassets.runtime.json");
        }
    }
}
#nullable restore

﻿namespace ARWNI2S.Hosting.LocalAssets
{
    /// <summary>
    /// Loader for static web assets
    /// </summary>
    public class LocalAssetsLoader
    {
        /// <summary>
        /// Configure the <see cref="INiisHostEnvironment"/> to use static web assets.
        /// </summary>
        /// <param name="environment">The application <see cref="INiisHostEnvironment"/>.</param>
        /// <param name="configuration">The host <see cref="IConfiguration"/>.</param>
        public static void UseLocalAssets(INiisHostEnvironment environment, IConfiguration configuration)
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

        internal static void UseLocalAssetsCore(INiisHostEnvironment environment, Stream manifest)
        {
            var staticWebAssetManifest = ManifestLocalAssetFileProvider.LocalAssetManifest.Parse(manifest);
            var provider = new ManifestLocalAssetFileProvider(
                staticWebAssetManifest,
                (contentRoot) => new PhysicalFileProvider(contentRoot));

            environment.NodeRootFileProvider = new CompositeFileProvider(new[] { provider, environment.NodeRootFileProvider });
        }

        internal static Stream ResolveManifest(INiisHostEnvironment environment, IConfiguration configuration)
        {
            try
            {
                var candidate = configuration[NI2SHostDefaults.LocalAssetsKey] ?? ResolveRelativeToAssembly(environment);
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

        [UnconditionalSuppressMessage("SingleFile", "IL3000:Assembly.Location",
            Justification = "The code handles if the Assembly.Location is empty by calling AppContext.BaseDirectory. Workaround https://github.com/dotnet/runtime/issues/83607")]
        private static string ResolveRelativeToAssembly(INiisHostEnvironment environment)
        {
            if (string.IsNullOrEmpty(environment.ApplicationName))
            {
                return null;
            }
            var assembly = Assembly.Load(environment.ApplicationName);
            var assemblyLocation = assembly.Location;
            var basePath = string.IsNullOrEmpty(assemblyLocation) ? AppContext.BaseDirectory : Path.GetDirectoryName(assemblyLocation);
            return Path.Combine(basePath!, $"{environment.ApplicationName}.staticwebassets.runtime.json");
        }
    }
}
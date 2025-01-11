using ARWNI2S.Configuration;
using ARWNI2S.Engine.Caching;
using ARWNI2S.Engine.Configuration;
using ARWNI2S.Engine.Environment;
using ARWNI2S.Environment;
using ARWNI2S.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Hosting
{
    public static class CommonServiceCollectionExtensions
    {
        public static ITypeFinder GetOrCreateTypeFinder(this IServiceCollection services)
        {
            if (Singleton<ITypeFinder>.Instance == null)
            {
                //register type finder
                Singleton<ITypeFinder>.Instance = new NI2STypeFinder();
                services.AddSingleton(sp => Singleton<ITypeFinder>.Instance);
            }
            return Singleton<ITypeFinder>.Instance;
        }

        internal static void ConfigureFileProvider(this IServiceCollection services, INiisHostEnvironment hostingEnvironment)
        {
            NI2SFileProvider.Default = new NI2SFileProvider(hostingEnvironment);
            services.AddScoped<INiisFileProvider, NI2SFileProvider>();
        }

        internal static NI2SSettings BindNodeSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //add configuration parameters
            var configurations = services.GetOrCreateTypeFinder()
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

            var nodeSettings = Singleton<NI2SSettings>.Instance;

            if (nodeSettings == null)
            {
                nodeSettings = NI2SSettingsHelper.SaveNodeSettings(configurations, NI2SFileProvider.Default, false);
                services.AddSingleton(nodeSettings);
            }
            else
            {
                var needToUpdate = configurations.Any(conf => !nodeSettings.Configuration.ContainsKey(conf.Name));
                NI2SSettingsHelper.SaveNodeSettings(configurations, NI2SFileProvider.Default, needToUpdate);
            }

            return nodeSettings;
        }

        internal static void AddContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<INiisContextAccessor, NI2SContextAccessor>();
        }

        /// <summary>
        /// Adds services required for distributed cache
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        internal static void AddDistributedCache(this IServiceCollection services)
        {
            var nodeSettings = Singleton<NI2SSettings>.Instance;
            var distributedCacheConfig = nodeSettings.Get<DistributedCacheConfig>();

            if (!distributedCacheConfig.Enabled)
                return;

            switch (distributedCacheConfig.DistributedCacheType)
            {
                case DistributedCacheType.Memory:
                    services.AddDistributedMemoryCache();
                    break;

                case DistributedCacheType.SqlServer:
                    services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = distributedCacheConfig.ConnectionString;
                        options.SchemaName = distributedCacheConfig.SchemaName;
                        options.TableName = distributedCacheConfig.TableName;
                    });
                    break;

                case DistributedCacheType.Redis:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;

                case DistributedCacheType.RedisSynchronizedMemory:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;
            }
        }
    }
}

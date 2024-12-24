using ARWNI2S.Engine.Collections;
using ARWNI2S.Engine.Configuration;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Caching
{
    public class Module : IEngineModule
    {
        /// <summary>
        /// Add and configure any of the engine services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //static cache manager
            var niisSettings = Singleton<NI2SSettings>.Instance;
            var distributedCacheConfig = niisSettings.Get<DistributedCacheConfig>();

            services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));

            services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
            services.AddScoped<IShortTermCacheManager, PerFrameCacheManager>();

            if (distributedCacheConfig.Enabled)
            {
                switch (distributedCacheConfig.DistributedCacheType)
                {
                    case DistributedCacheType.Memory:
                        services.AddScoped<IStaticCacheManager, MemoryDistributedCacheManager>();
                        services.AddScoped<ICacheKeyService, MemoryDistributedCacheManager>();
                        break;
                    case DistributedCacheType.SqlServer:
                        services.AddScoped<IStaticCacheManager, MsSqlServerCacheManager>();
                        services.AddScoped<ICacheKeyService, MsSqlServerCacheManager>();
                        break;
                    case DistributedCacheType.Redis:
                        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                        services.AddScoped<IStaticCacheManager, RedisCacheManager>();
                        services.AddScoped<ICacheKeyService, RedisCacheManager>();
                        break;
                    case DistributedCacheType.RedisSynchronizedMemory:
                        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                        services.AddSingleton<ISynchronizedMemoryCache, RedisSynchronizedMemoryCache>();
                        services.AddSingleton<IStaticCacheManager, SynchronizedMemoryCacheManager>();
                        services.AddScoped<ICacheKeyService, SynchronizedMemoryCacheManager>();
                        break;
                }

                services.AddSingleton<ILocker, DistributedCacheLocker>();
            }
            else
            {
                services.AddSingleton<ILocker, MemoryCacheLocker>();
                services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
                services.AddScoped<ICacheKeyService, MemoryCacheManager>();
            }

        }

        /// <summary>
        /// Configure the using of added components
        /// </summary>
        /// <param name="engine">Builder for configuring a node's NI2S engine</param>
        public void ConfigureEngine(IEngineBuilder engine)
        {

        }

        ///// <summary>
        ///// Gets order of this startup configuration implementation
        ///// </summary>
        //public int Order => InitStage.DbInit;
    }
}

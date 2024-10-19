﻿using ARWNI2S.Infrastructure;
using ARWNI2S.Infrastructure.Collections;
using ARWNI2S.Infrastructure.Configuration;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Events;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Core.Services.Helpers;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Services.Caching;
using ARWNI2S.Node.Services.Clustering;
using ARWNI2S.Node.Services.Common;
using ARWNI2S.Node.Services.Configuration;
using ARWNI2S.Node.Services.Directory;
using ARWNI2S.Node.Services.Events;
using ARWNI2S.Node.Services.Installation;
using ARWNI2S.Node.Services.Localization;
using ARWNI2S.Node.Services.Logging;
using ARWNI2S.Node.Services.Plugins;
using ARWNI2S.Node.Services.ScheduleTasks;
using ARWNI2S.Node.Services.Security;
using ARWNI2S.Node.Services.Users;
using ARWNI2S.Runtime.Services.Installation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Infrastructure
{
    /// <summary>
    /// Represents the registering services on application startup
    /// </summary>
    public partial class NI2SStartup : INI2SStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //file provider
            services.AddScoped<IEngineFileProvider, EngineFileProvider>();
            //node helper
            services.AddScoped<INodeHelper, NodeHelper>();

            //modules
            services.AddScoped<IModuleService, ModuleService>();
            //services.AddScoped<OfficialFeedManager>();

            //static cache manager
            var ni2sSettings = Singleton<NI2SSettings>.Instance;
            var distributedCacheConfig = ni2sSettings.Get<DistributedCacheConfig>();

            services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));

            services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
            services.AddScoped<IShortTermCacheManager, PerRequestCacheManager>();

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

            //work context
            services.AddScoped<IWorkContext, NodeWorkContext>();

            //node context
            services.AddScoped<INodeContext, LocalNodeContext>();

            //services
            services.AddScoped<IGenericAttributeService, GenericAttributeService>();
            //services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAclService, AclService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IMeasureService, MeasureService>();
            services.AddScoped<IClusteringService, ClusteringService>();
            services.AddScoped<INodeMappingService, NodeMappingService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<ILogService, DefaultLogger>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddSingleton<INodeEventPublisher, EventPublisher>();
            services.AddScoped<ISettingService, SettingService>();

            //module managers
            services.AddScoped(typeof(IModuleManager<>), typeof(ModuleManager<>));
            //services.AddScoped<IAuthenticationModuleManager, AuthenticationModuleManager>();
            //services.AddScoped<IMultiFactorAuthenticationModuleManager, MultiFactorAuthenticationModuleManager>();
            //services.AddScoped<IWidgetModuleManager, WidgetModuleManager>();
            //services.AddScoped<IExchangeRateModuleManager, ExchangeRateModuleManager>();
            //services.AddScoped<ITaxModuleManager, TaxModuleManager>();

            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //register all settings
            var typeFinder = Singleton<ITypeFinder>.Instance;

            var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
            {
                services.AddScoped(setting, serviceProvider =>
                {
                    var nodeId = DataSettingsManager.IsDatabaseInstalled()
                        ? serviceProvider.GetRequiredService<INodeContext>().GetCurrentNode()?.Id ?? 0
                        : 0;

                    return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, nodeId).Result;
                });
            }

            //installation service      
            services.AddScoped<IInstallationService, InstallationService>();

            if (!DataSettingsManager.IsDatabaseInstalled() && Environment.UserInteractive)
                services.AddScoped<IDatabaseInstaller, ConsoleDatabaseInstaller>();

            //schedule tasks
            services.AddSingleton<ITaskScheduler, Node.Services.ScheduleTasks.TaskScheduler>();
            services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);

            //register the Lazy resolver for .Net IoC
            var useAutofac = ni2sSettings.Get<CommonConfig>().UseAutofac;
            if (!useAutofac)
                services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));
        }

        public void Configure(IHost application)
        {
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 2000;
    }
}

using ARWNI2S.Caching;
using ARWNI2S.Configuration;
using ARWNI2S.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ARWNI2S.Engine
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class EngineServiceCollectionExtensions
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

        /// <summary>
        /// Registers a default context accessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDefaultContextAccessor(this IServiceCollection services)
        {
            services.TryAddSingleton<IFrameContextAccessor, DefaultContextAccessor>();
        }

        /// <summary>
        /// Adds services required for distributed cache
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCache(this IServiceCollection services)
        {
            var niisSettings = Singleton<NI2SSettings>.Instance;
            var distributedCacheConfig = niisSettings.Get<DistributedCacheConfig>();

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

        ///// <summary>
        ///// Add and configure MVRM for the engine
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        ///// <returns>A builder for configuring MVRM services</returns>
        //public static IMvrmBuilder AddNI2SMvrm(this IServiceCollection services)
        //{
        //    //add basic MVRM feature
        //    var mvrmBuilder = services.AddSimulationWithScenes();

        //    mvrmBuilder.AddRazorRuntimeCompilation();

        //    var niisSettings = Singleton<NI2SSettings>.Instance;
        //    if (niisSettings.Get<CommonConfig>().UseSceneStateTempDataProvider)
        //    {
        //        //use scene-based temp data provider
        //        mvrmBuilder.AddSceneStateTempDataProvider();
        //    }
        //    else
        //    {
        //        //use tag-based temp data provider
        //        mvrmBuilder.AddTagTempDataProvider(options =>
        //        {
        //            options.Tag.Name = $"{NI2STagDefaults.Prefix}{NI2STagDefaults.TempDataTag}";
        //            options.Tag.SecurePolicy = TagSecurePolicy.SameAsOrigin;
        //        });
        //    }

        //    services.AddRenderModel();

        //    //MVC now serializes JSON with camel case names by default, use this code to avoid it
        //    mvrmBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

        //    //set some options
        //    mvrmBuilder.AddMvrmOptions(options =>
        //    {
        //        options.ModelBinderProviders.Insert(1, new NI2SModelBinderProvider());
        //        //add custom display metadata provider 
        //        options.ModelMetadataDetailsProviders.Add(new NI2SMetadataProvider());

        //        //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
        //        //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
        //        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => NopValidationDefaults.NotNullValidationLocaleName);
        //    });

        //    //add fluent validation
        //    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        //    //register all available validators from Nop assemblies
        //    var assemblies = mvrmBuilder.PartManager.ApplicationParts
        //        .OfType<AssemblyPart>()
        //        .Where(part => part.Name.StartsWith("ARWNI2S", StringComparison.InvariantCultureIgnoreCase))
        //        .Select(part => part.Assembly);
        //    services.AddModulesFromAssemblies(assemblies);

        //    //register controllers as services, it'll allow to override them
        //    mvrmBuilder.AddSimulationAsService();

        //    return mvrmBuilder;
        //}
    }
}

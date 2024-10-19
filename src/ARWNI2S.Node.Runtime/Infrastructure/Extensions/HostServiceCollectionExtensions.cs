using ARWNI2S.Infrastructure;
using ARWNI2S.Infrastructure.Configuration;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Core.Runtime;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Services.Clustering;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using StackExchange.Profiling.Internal;
using StackExchange.Profiling;
using StackExchange.Redis;
using System.Net;
using StackExchange.Profiling.Storage;
using ARWNI2S.Node.Services.Security;
using ARWNI2S.Runtime.Data;
using ARWNI2S.Runtime.Clustering;
using ARWNI2S.Runtime.Profiling;
using ARWNI2S.Runtime.Core.Extensions;

namespace ARWNI2S.Runtime.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class HostServiceCollectionExtensions
    {
        /// <summary>
        /// Configure base application settings
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="context">A builder for web applications and services</param>
        public static void ConfigureApplicationSettings(this IServiceCollection services,
            HostBuilderContext context)
        {
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            //create default file provider
            CommonHelper.DefaultFileProvider = new EngineFileProvider(context.HostingEnvironment);

            //register type finder
            var typeFinder = new NI2SNodeTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                context.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

            //TODO: READ SECRETS FROM KEYVAULT use SecretAttribute decorated properties

            var nodeSettings = NI2SSettingsHelper.SaveNI2SSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(nodeSettings);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="context">A builder for web applications and services</param>
        public static void ConfigureApplicationServices(this IServiceCollection services,
            HostBuilderContext context)
        {
            //add accessor to Context
            services.AddContextAccessor();

            //add core services
            var ni2sCoreBuilder = services.AddNI2SCore();

            //initialize modules
            var moduleConfig = new ModuleConfig();
            context.Configuration.GetSection(nameof(ModuleConfig)).Bind(moduleConfig, options => options.BindNonPublicProperties = true);
            ni2sCoreBuilder.PartManager.InitializeModules(moduleConfig);

            //create engine and configure service provider
            var engine = EngineContext.Create();

            engine.ConfigureServices(services, context.Configuration);
        }

        /// <summary>
        /// Register RuntimeContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IExecutionContextAccessor, RuntimeContextAccessor>();
        }

        /// <summary>
        /// Adds services required for distributed cache
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCache(this IServiceCollection services)
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
                case DistributedCacheType.RedisSynchronizedMemory:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;
            }
        }

        /// <summary>
        /// Adds frontline (orleans client) services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddNI2SFrontline(this IServiceCollection services)
        {
            var ni2sSettings = Singleton<NI2SSettings>.Instance;
            var nodeConfig = ni2sSettings.Get<NodeConfig>();

            // notice: Orleans Silo Nodes will throw exception if UseFrontline enabled.
            // frontline services enables orleans client access, disable if no realtime simulation data is needed.
            if (nodeConfig.NodeType == NodeType.Frontline)
            {
                var clusterConfig = ni2sSettings.Get<ClusterConfig>();

                services.AddOrleansClient(client =>
                {
                    client = client.Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = clusterConfig.ClusterId;
                        options.ServiceId = clusterConfig.ServiceId;
                    }).Configure<ClientMessagingOptions>(options =>
                    {
                        // Configurar el tiempo de espera para respuestas
                        options.ResponseTimeout = TimeSpan.FromSeconds(60); // Timeout para mensajes
                        options.ResponseTimeoutWithDebugger = TimeSpan.FromSeconds(180); // Timeout extendido si se está depurando
                    });


                    if (!clusterConfig.IsDevelopment)
                    {
                        switch (clusterConfig.SiloStorageClustering)
                        {
                            case SimulationClusteringType.AzureStorage:
                                {
                                    if (string.IsNullOrEmpty(clusterConfig.ConnectionString))
                                        throw new NodeException("Unable to configure Azure storage clustering: missing connection string.");

                                    // Configurar el cluster Orleans
                                    client = client.UseAzureStorageClustering(options => options.TableServiceClient = new TableServiceClient(clusterConfig.ConnectionString, options.ClientOptions));
                                    break;
                                }
                            case SimulationClusteringType.Redis:
                                {
                                    if (string.IsNullOrEmpty(clusterConfig.ConnectionString))
                                        throw new NodeException("Unable to configure Redis storage clustering: missing connection string.");

                                    client = client.UseRedisClustering(options =>
                                    {
                                        // Configura los detalles de conexión a Redis
                                        options.ConfigurationOptions = new ConfigurationOptions().ParseConnectionString(clusterConfig.ConnectionString);
                                    });
                                    break;
                                }
                            case SimulationClusteringType.SqlServer:
                                {
                                    if (string.IsNullOrEmpty(clusterConfig.ConnectionString))
                                        throw new NodeException("Unable to configure SqlServer storage clustering: missing connection string.");
                                    client = client.UseAdoNetClustering(options =>
                                    {
                                        options.Invariant = Constants.INVARIANT_NAME_SQL_SERVER;
                                        options.ConnectionString = clusterConfig.ConnectionString;
                                    });
                                    break;
                                }
                            case SimulationClusteringType.Localhost:
                            default:
                                {
                                    client.UseLocalhostClustering();
                                    break;
                                }
                        }

                    }
                    else
                    {
                        client.UseLocalhostClustering();
                    }
                });
            }
        }

        public static void AddNI2SClustering(this IServiceCollection services)
        {
            services.AddScoped<IClusteringService, ClusteringService>();
            services.AddScoped<INodeMappingService, NodeMappingService>();

            services.AddSingleton<ClusterManager>();
        }

        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddNI2SMiniProfiler(this IServiceCollection services)
        {
            //whether database is already installed
            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            var nodeSettings = Singleton<NI2SSettings>.Instance;
            if (nodeSettings.Get<CommonConfig>().MiniProfilerEnabled)
            {
                services.AddMiniProfiler(miniProfilerOptions =>
                {
                    //use memory cache provider for storing each result
                    ((MemoryCacheStorage)miniProfilerOptions.Storage).CacheDuration = TimeSpan.FromMinutes(nodeSettings.Get<CacheConfig>().DefaultCacheTime);

                    //determine who can access the MiniProfiler results
                    miniProfilerOptions.ResultsAuthorize = request => EngineContext.Current.Resolve<IPermissionService>().AuthorizeAsync(StandardPermissionProvider.AccessProfiling).Result;
                });
            }
        }

        /// <summary>
        /// Adds MiniProfiler timings for actions and views.
        /// </summary>
        /// <param name="services">The services collection to configure.</param>
        /// <param name="configureOptions">An <see cref="Action{MiniProfilerOptions}"/> to configure options for MiniProfiler.</param>
        private static IMiniProfilerBuilder AddMiniProfiler(this IServiceCollection services, Action<MiniProfilerOptions> configureOptions = null)
        {
            services.AddMemoryCache(); // Unconditionally register an IMemoryCache since it's the most common and default case
            services.AddSingleton<IConfigureOptions<MiniProfilerOptions>, MiniProfilerOptionsDefaults>();
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            // Set background statics
            services.Configure<MiniProfilerOptions>(o => MiniProfiler.Configure(o));
            services.AddSingleton<DiagnosticInitializer>(); // For any IMiniProfilerDiagnosticListener registration

            services.AddSingleton<IMiniProfilerDiagnosticListener, MiniProfilerDiagnosticListener>(); // For view and action profiling

            return new MiniProfilerBuilder(services);
        }

    }


    ///// <summary>
    ///// Adds data protection services
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNI2SDataProtection(this IServiceCollection services)
    //{
    //    var ni2sSettings = Singleton<NI2SSettings>.Instance;
    //    if (ni2sSettings.Get<AzureBlobConfig>().Enabled && ni2sSettings.Get<AzureBlobConfig>().StoreDataProtectionKeys)
    //    {
    //        var blobServiceClient = new BlobServiceClient(ni2sSettings.Get<AzureBlobConfig>().ConnectionString);
    //        var blobContainerClient = blobServiceClient.GetBlobContainerClient(ni2sSettings.Get<AzureBlobConfig>().DataProtectionKeysContainerName);
    //        var blobClient = blobContainerClient.GetBlobClient(DataProtectionDefaults.AzureDataProtectionKeyFile);

    //        var dataProtectionBuilder = services.AddDataProtection().PersistKeysToAzureBlobStorage(blobClient);

    //        if (!ni2sSettings.Get<AzureBlobConfig>().DataProtectionKeysEncryptWithVault)
    //            return;

    //        var keyIdentifier = ni2sSettings.Get<AzureBlobConfig>().DataProtectionKeysVaultId;
    //        var credentialOptions = new DefaultAzureCredentialOptions();
    //        var tokenCredential = new DefaultAzureCredential(credentialOptions);

    //        dataProtectionBuilder.ProtectKeysWithAzureKeyVault(new Uri(keyIdentifier), tokenCredential);
    //    }
    //    else
    //    {
    //        var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(DataProtectionDefaults.DataProtectionKeysPath);
    //        var dataProtectionKeysFolder = new DirectoryInfo(dataProtectionKeysPath);

    //        //configure the data protection system to persist keys to the specified directory
    //        services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
    //    }
    //}



    ///// <summary>
    ///// Adds authentication service
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNodeAuthentication(this IServiceCollection services)
    //{
    //    //set default authentication schemes
    //    var authenticationBuilder = services.AddAuthentication(options =>
    //    {
    //        options.DefaultChallengeScheme = AuthenticationServicesDefaults.AuthenticationScheme;
    //        options.DefaultScheme = AuthenticationServicesDefaults.AuthenticationScheme;
    //        options.DefaultSignInScheme = AuthenticationServicesDefaults.ExternalAuthenticationScheme;
    //    });

    //    //add main cookie authentication
    //    authenticationBuilder.AddCookie(AuthenticationServicesDefaults.AuthenticationScheme, options =>
    //    {
    //        options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.AuthenticationCookie}";
    //        options.Cookie.HttpOnly = true;
    //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //        options.LoginPath = AuthenticationServicesDefaults.LoginPath;
    //        options.AccessDeniedPath = AuthenticationServicesDefaults.AccessDeniedPath;
    //    });

    //    //add external authentication
    //    authenticationBuilder.AddCookie(AuthenticationServicesDefaults.ExternalAuthenticationScheme, options =>
    //    {
    //        options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.ExternalAuthenticationCookie}";
    //        options.Cookie.HttpOnly = true;
    //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //        options.LoginPath = AuthenticationServicesDefaults.LoginPath;
    //        options.AccessDeniedPath = AuthenticationServicesDefaults.AccessDeniedPath;
    //    });

    //    //add wallet authentication
    //    authenticationBuilder.AddCookie(AuthenticationServicesDefaults.WalletAuthenticationScheme, options =>
    //    {
    //        options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.WalletAuthenticationCookie}";
    //        options.Cookie.HttpOnly = true;
    //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //        options.LoginPath = AuthenticationServicesDefaults.LoginPath;
    //        options.AccessDeniedPath = AuthenticationServicesDefaults.AccessDeniedPath;
    //    });

    //    //register and configure external authentication modules now
    //    var typeFinder = Singleton<ITypeFinder>.Instance;
    //    var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
    //    var externalAuthInstances = externalAuthConfigurations
    //        .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x));

    //    foreach (var instance in externalAuthInstances)
    //        instance.Configure(authenticationBuilder);

    //    var walletAuthConfigurations = typeFinder.FindClassesOfType<IWalletAuthenticationRegistrar>();
    //    var walletAuthInstances = walletAuthConfigurations
    //        .Select(x => (IWalletAuthenticationRegistrar)Activator.CreateInstance(x));

    //    foreach (var instance in walletAuthInstances)
    //        instance.Configure(authenticationBuilder);

    //}

    ///// <summary>
    ///// Add and configure MVC for the application
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    ///// <returns>A builder for configuring MVC services</returns>
    //public static IMvcBuilder AddNodeMvc(this IServiceCollection services)
    //{
    //    //add basic MVC feature
    //    var mvcBuilder = services.AddControllersWithViews();

    //    mvcBuilder.AddRazorRuntimeCompilation();

    //    var nodeSettings = Singleton<NodeSettings>.Instance;
    //    if (nodeSettings.Get<CommonConfig>().UseSessionStateTempDataProvider)
    //    {
    //        //use session-based temp data provider
    //        mvcBuilder.AddSessionStateTempDataProvider();
    //    }
    //    else
    //    {
    //        //use cookie-based temp data provider
    //        mvcBuilder.AddCookieTempDataProvider(options =>
    //        {
    //            options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.TempDataCookie}";
    //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //        });
    //    }

    //    services.AddRazorPages();

    //    //MVC now serializes JSON with camel case names by default, use this code to avoid it
    //    mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

    //    //set some options
    //    mvcBuilder.AddMvcOptions(options =>
    //    {
    //        //we'll use this until https://github.com/dotnet/aspnetcore/issues/6566 is solved 
    //        options.ModelBinderProviders.Insert(0, new InvariantNumberModelBinderProvider());
    //        options.ModelBinderProviders.Insert(1, new CustomPropertiesModelBinderProvider());
    //        //add custom display metadata provider 
    //        options.ModelMetadataDetailsProviders.Add(new NodeMetadataProvider());

    //        //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
    //        //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
    //        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => NodeValidationDefaults.NotNullValidationLocaleName);
    //    });

    //    //add fluent validation
    //    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

    //    //register all available validators from DragonCorp assemblies
    //    var assemblies = mvcBuilder.PartManager.ApplicationParts
    //        .OfType<AssemblyPart>()
    //        .Where(part => part.Name.StartsWith("DragonCorp", StringComparison.InvariantCultureIgnoreCase))
    //        .Select(part => part.Assembly);
    //    services.AddValidatorsFromAssemblies(assemblies);

    //    //register controllers as services, it'll allow to override them
    //    mvcBuilder.AddControllersAsServices();

    //    return mvcBuilder;
    //}

    ///// <summary>
    ///// Register custom RedirectResultExecutor
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNodeRedirectResultExecutor(this IServiceCollection services)
    //{
    //    //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
    //    services.AddScoped<IActionResultExecutor<RedirectResult>, NodeRedirectResultExecutor>();
    //}

    ///// <summary>
    ///// Add and configure WebMarkupMin service
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNodeWebMarkupMin(this IServiceCollection services)
    //{
    //    //check whether database is installed
    //    if (!DataSettingsManager.IsDatabaseInstalled())
    //        return;

    //    services
    //        .AddWebMarkupMin(options =>
    //        {
    //            options.AllowMinificationInDevelopmentEnvironment = true;
    //            options.AllowCompressionInDevelopmentEnvironment = true;
    //            options.DisableMinification = !EngineContext.Current.Resolve<CommonSettings>().EnableHtmlMinification;
    //            options.DisableCompression = true;
    //            options.DisablePoweredByHttpHeaders = true;
    //        })
    //        .AddHtmlMinification(options =>
    //        {
    //            options.MinificationSettings.AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.KeepQuotes;

    //            options.CssMinifierFactory = new NUglifyCssMinifierFactory();
    //            options.JsMinifierFactory = new NUglifyJsMinifierFactory();
    //        })
    //        .AddXmlMinification(options =>
    //        {
    //            var settings = options.MinificationSettings;
    //            settings.RenderEmptyTagsWithSpace = true;
    //            settings.CollapseTagsWithoutContent = true;
    //        });
    //}

    ///// <summary>
    ///// Adds WebOptimizer to the specified <see cref="IServiceCollection"/> and enables CSS and JavaScript minification.
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNodeWebOptimizer(this IServiceCollection services)
    //{
    //    var nodeSettings = Singleton<NodeSettings>.Instance;
    //    var cssBundling = nodeSettings.Get<WebOptimizerConfig>().EnableCssBundling;
    //    var jsBundling = nodeSettings.Get<WebOptimizerConfig>().EnableJavaScriptBundling;

    //    //add minification & bundling
    //    var cssSettings = new CssBundlingSettings
    //    {
    //        FingerprintUrls = false,
    //        Minify = cssBundling
    //    };

    //    var codeSettings = new CodeBundlingSettings
    //    {
    //        Minify = jsBundling,
    //        AdjustRelativePaths = false //disable this feature because it breaks function names that have "Url(" at the end
    //    };

    //    services.AddWebOptimizer(null, cssSettings, codeSettings);
    //}

    ///// <summary>
    ///// Add and configure default HTTP clients
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNodeHttpClients(this IServiceCollection services)
    //{
    //    //default client
    //    services.AddHttpClient(HttpDefaults.DefaultHttpClient).WithProxy();

    //    //client to request current node
    //    services.AddHttpClient<ServerHttpClient>();

    //    //client to request dragonCorp official site
    //    services.AddHttpClient<ServerHttpClient>().WithProxy();

    //    //client to request reCAPTCHA service
    //    services.AddHttpClient<CaptchaHttpClient>().WithProxy();
    //}

}
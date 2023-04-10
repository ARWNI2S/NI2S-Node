using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Core;
using NI2S.Node.Core.Configuration;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Hosting.Builder;
using System;
using System.Linq;
using System.Net;

namespace NI2S.Node.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure base application settings
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for node engine and services</param>
        /* 041 */
        public static void ConfigureEngineSettings(this IServiceCollection services,
            NodeEngineBuilder builder)
        {
            //let the operating system decide what TLS protocol version to use
            //see dummys://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            //create default file provider
            CommonHelper.DefaultFileProvider = new NodeFileProvider(builder.Environment);

            //register type finder
            var typeFinder = new NodeEngineTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

            var appSettings = NodeSettingsHelper.SaveNodeSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for node engine and services</param>
        /* 050 */
        public static void ConfigureEngineServices(this IServiceCollection services,
            NodeEngineBuilder builder)
        {
            //add accessor to DummyContext
            services.AddDummyContextAccessor();

            //initialize plugins
            //var mvcCoreBuilder = services.AddMvcCore();
            var pluginConfig = new PluginConfig();
            builder.Configuration.GetSection(nameof(PluginConfig)).Bind(pluginConfig, options => options.BindNonPublicProperties = true);
            //mvcCoreBuilder.PartManager.InitializePlugins(pluginConfig);

            //create engine and configure service provider
            var engine = EngineContext.Create();

            engine.ConfigureServices(services, builder.Configuration);
        }

        /// <summary>
        /// Register DummyContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDummyContextAccessor(this IServiceCollection services)
        {
            //services.AddSingleton<IDummyContextAccessor, DummyContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddAntiForgery(this IServiceCollection services)
        //{
        //    //override cookie name
        //    services.AddAntiforgery(options =>
        //    {
        //        options.Cookie.Name = $"{NI2SCookieDefaults.Prefix}{NI2SCookieDefaults.AntiforgeryCookie}";
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //    });
        //}

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddDummySession(this IServiceCollection services)
        //{
        //    services.AddSession(options =>
        //    {
        //        options.Cookie.Name = $"{NI2SCookieDefaults.Prefix}{NI2SCookieDefaults.SessionCookie}";
        //        options.Cookie.DummyOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //    });
        //}

        /// <summary>
        /// Adds services required for themes support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddThemes(this IServiceCollection services)
        //{
        //    if (!DataSettingsManager.IsDatabaseInstalled())
        //        return;

        //    //themes support
        //    services.Configure<RazorViewEngineOptions>(options =>
        //    {
        //        options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
        //    });
        //}

        /// <summary>
        /// Adds services required for distributed cache
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCache(this IServiceCollection services)
        {
            //    var appSettings = Singleton<AppSettings>.Instance;
            //    var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

            //    if (!distributedCacheConfig.Enabled)
            //        return;

            //    switch (distributedCacheConfig.DistributedCacheType)
            //    {
            //        case DistributedCacheType.Memory:
            //            services.AddDistributedMemoryCache();
            //            break;

            //        case DistributedCacheType.SqlServer:
            //            services.AddDistributedSqlServerCache(options =>
            //            {
            //                options.ConnectionString = distributedCacheConfig.ConnectionString;
            //                options.SchemaName = distributedCacheConfig.SchemaName;
            //                options.TableName = distributedCacheConfig.TableName;
            //            });
            //            break;

            //        case DistributedCacheType.Redis:
            //            services.AddStackExchangeRedisCache(options =>
            //            {
            //                options.Configuration = distributedCacheConfig.ConnectionString;
            //                options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
            //            });
            //            break;

            //        case DistributedCacheType.RedisSynchronizedMemory:
            //            services.AddStackExchangeRedisCache(options =>
            //            {
            //                options.Configuration = distributedCacheConfig.ConnectionString;
            //                options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
            //            });
            //            break;
            //    }
        }

        /// <summary>
        /// Adds data protection services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SDataProtection(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    if (appSettings.Get<AzureBlobConfig>().Enabled && appSettings.Get<AzureBlobConfig>().StoreDataProtectionKeys)
        //    {
        //        var blobServiceClient = new BlobServiceClient(appSettings.Get<AzureBlobConfig>().ConnectionString);
        //        var blobContainerClient = blobServiceClient.GetBlobContainerClient(appSettings.Get<AzureBlobConfig>().DataProtectionKeysContainerName);
        //        var blobClient = blobContainerClient.GetBlobClient(NI2SDataProtectionDefaults.AzureDataProtectionKeyFile);

        //        var dataProtectionBuilder = services.AddDataProtection().PersistKeysToAzureBlobStorage(blobClient);

        //        if (!appSettings.Get<AzureBlobConfig>().DataProtectionKeysEncryptWithVault)
        //            return;

        //        var keyIdentifier = appSettings.Get<AzureBlobConfig>().DataProtectionKeysVaultId;
        //        var credentialOptions = new DefaultAzureCredentialOptions();
        //        var tokenCredential = new DefaultAzureCredential(credentialOptions);

        //        dataProtectionBuilder.ProtectKeysWithAzureKeyVault(new Uri(keyIdentifier), tokenCredential);
        //    }
        //    else
        //    {
        //        var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(NI2SDataProtectionDefaults.DataProtectionKeysPath);
        //        var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

        //        //configure the data protection system to persist keys to the specified directory
        //        services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
        //    }
        //}

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SAuthentication(this IServiceCollection services)
        //{
        //    //set default authentication schemes
        //    var authenticationBuilder = services.AddAuthentication(options =>
        //    {
        //        options.DefaultChallengeScheme = NI2SAuthenticationDefaults.AuthenticationScheme;
        //        options.DefaultScheme = NI2SAuthenticationDefaults.AuthenticationScheme;
        //        options.DefaultSignInScheme = NI2SAuthenticationDefaults.ExternalAuthenticationScheme;
        //    });

        //    //add main cookie authentication
        //    authenticationBuilder.AddCookie(NI2SAuthenticationDefaults.AuthenticationScheme, options =>
        //    {
        //        options.Cookie.Name = $"{NI2SCookieDefaults.Prefix}{NI2SCookieDefaults.AuthenticationCookie}";
        //        options.Cookie.DummyOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        options.LoginPath = NI2SAuthenticationDefaults.LoginPath;
        //        options.AccessDeniedPath = NI2SAuthenticationDefaults.AccessDeniedPath;
        //    });

        //    //add external authentication
        //    authenticationBuilder.AddCookie(NI2SAuthenticationDefaults.ExternalAuthenticationScheme, options =>
        //    {
        //        options.Cookie.Name = $"{NI2SCookieDefaults.Prefix}{NI2SCookieDefaults.ExternalAuthenticationCookie}";
        //        options.Cookie.DummyOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        options.LoginPath = NI2SAuthenticationDefaults.LoginPath;
        //        options.AccessDeniedPath = NI2SAuthenticationDefaults.AccessDeniedPath;
        //    });

        //    //register and configure external authentication plugins now
        //    var typeFinder = Singleton<ITypeFinder>.Instance;
        //    var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
        //    var externalAuthInstances = externalAuthConfigurations
        //        .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x));

        //    foreach (var instance in externalAuthInstances)
        //        instance.Configure(authenticationBuilder);
        //}

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        //public static IMvcBuilder AddNI2SMvc(this IServiceCollection services)
        //{
        //    //add basic MVC feature
        //    var mvcBuilder = services.AddControllersWithViews();

        //    mvcBuilder.AddRazorRuntimeCompilation();

        //    var appSettings = Singleton<AppSettings>.Instance;
        //    if (appSettings.Get<CommonConfig>().UseSessionStateTempDataProvider)
        //    {
        //        //use session-based temp data provider
        //        mvcBuilder.AddSessionStateTempDataProvider();
        //    }
        //    else
        //    {
        //        //use cookie-based temp data provider
        //        mvcBuilder.AddCookieTempDataProvider(options =>
        //        {
        //            options.Cookie.Name = $"{NI2SCookieDefaults.Prefix}{NI2SCookieDefaults.TempDataCookie}";
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        });
        //    }

        //    services.AddRazorPages();

        //    //MVC now serializes JSON with camel case names by default, use this code to avoid it
        //    mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

        //    //set some options
        //    mvcBuilder.AddMvcOptions(options =>
        //    {
        //        //we'll use this until dummys://github.com/dotnet/aspnetcore/issues/6566 is solved 
        //        options.ModelBinderProviders.Insert(0, new InvariantNumberModelBinderProvider());
        //        options.ModelBinderProviders.Insert(1, new CustomPropertiesModelBinderProvider());
        //        //add custom display metadata provider 
        //        options.ModelMetadataDetailsProviders.Add(new NI2SMetadataProvider());

        //        //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
        //        //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
        //        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => NI2SValidationDefaults.NotNullValidationLocaleName);
        //    });

        //    //add fluent validation
        //    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        //    //register all available validators from NI2S assemblies
        //    var assemblies = mvcBuilder.PartManager.ApplicationParts
        //        .OfType<AssemblyPart>()
        //        .Where(part => part.Name.StartsWith("NI2S", StringComparison.InvariantCultureIgnoreCase))
        //        .Select(part => part.Assembly);
        //    services.AddValidatorsFromAssemblies(assemblies);

        //    //register controllers as services, it'll allow to override them
        //    mvcBuilder.AddControllersAsServices();

        //    return mvcBuilder;
        //}

        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SRedirectResultExecutor(this IServiceCollection services)
        //{
        //    //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
        //    services.AddScoped<IActionResultExecutor<RedirectResult>, NI2SRedirectResultExecutor>();
        //}

        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SMiniProfiler(this IServiceCollection services)
        //{
        //    //whether database is already installed
        //    if (!DataSettingsManager.IsDatabaseInstalled())
        //        return;

        //    var appSettings = Singleton<AppSettings>.Instance;
        //    if (appSettings.Get<CommonConfig>().MiniProfilerEnabled)
        //    {
        //        services.AddMiniProfiler(miniProfilerOptions =>
        //        {
        //            //use memory cache provider for storing each result
        //            ((MemoryCacheStorage)miniProfilerOptions.Storage).CacheDuration = TimeSpan.FromMinutes(appSettings.Get<CacheConfig>().DefaultCacheTime);

        //            //determine who can access the MiniProfiler results
        //            miniProfilerOptions.ResultsAuthorize = request => EngineContext.Current.Resolve<IPermissionService>().AuthorizeAsync(StandardPermissionProvider.AccessProfiling).Result;
        //        });
        //    }
        //}

        /// <summary>
        /// Add and configure NodeMarkupMin service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SNodeMarkupMin(this IServiceCollection services)
        //{
        //    //check whether database is installed
        //    if (!DataSettingsManager.IsDatabaseInstalled())
        //        return;

        //    services
        //        .AddNodeMarkupMin(options =>
        //        {
        //            options.AllowMinificationInDevelopmentEnvironment = true;
        //            options.AllowCompressionInDevelopmentEnvironment = true;
        //            options.DisableMinification = !EngineContext.Current.Resolve<CommonSettings>().EnableHtmlMinification;
        //            options.DisableCompression = true;
        //            options.DisablePoweredByDummyHeaders = true;
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

        /// <summary>
        /// Adds NodeOptimizer to the specified <see cref="IServiceCollection"/> and enables CSS and JavaScript minification.
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SNodeOptimizer(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    var cssBundling = appSettings.Get<NodeOptimizerConfig>().EnableCssBundling;
        //    var jsBundling = appSettings.Get<NodeOptimizerConfig>().EnableJavaScriptBundling;

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

        //    services.AddNodeOptimizer(null, cssSettings, codeSettings);
        //}

        /// <summary>
        /// Add and configure default HTTP clients
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        //public static void AddNI2SDummyClients(this IServiceCollection services)
        //{
        //    //default client
        //    services.AddDummyClient(NI2SDummyDefaults.DefaultDummyClient).WithProxy();

        //    //client to request current store
        //    services.AddDummyClient<StoreDummyClient>();

        //    //client to request nopCommerce official site
        //    services.AddDummyClient<NI2SDummyClient>().WithProxy();

        //    //client to request reCAPTCHA service
        //    services.AddDummyClient<CaptchaDummyClient>().WithProxy();
        //}
    }
}

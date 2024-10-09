﻿using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace ARWNI2S.Node.Hosting.Extensions
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
            var typeFinder = new AppDomainTypeFinder();
            //var typeFinder = new WebAppTypeFinder();
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

            var appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="context">A builder for web applications and services</param>
        public static void ConfigureApplicationServices(this IServiceCollection services,
            HostBuilderContext context)
        {
            //add accessor to HttpContext
            //services.AddHttpContextAccessor();

            //initialize modules
            var moduleConfig = new ModuleConfig();
            context.Configuration.GetSection(nameof(ModuleConfig)).Bind(moduleConfig, options => options.BindNonPublicProperties = true);
            NodeModuleManager.InitializeModules(moduleConfig);

            //create engine and configure service provider
            var engine = EngineContext.Create();

            engine.ConfigureServices(services, context.Configuration);
        }

        ///// <summary>
        ///// Adds services required for distributed cache
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddDistributedCache(this IServiceCollection services)
        //{
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
        //}

        ///// <summary>
        ///// Adds data protection services
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddServerDataProtection(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    if (appSettings.Get<AzureBlobConfig>().Enabled && appSettings.Get<AzureBlobConfig>().StoreDataProtectionKeys)
        //    {
        //        var blobServiceClient = new BlobServiceClient(appSettings.Get<AzureBlobConfig>().ConnectionString);
        //        var blobContainerClient = blobServiceClient.GetBlobContainerClient(appSettings.Get<AzureBlobConfig>().DataProtectionKeysContainerName);
        //        var blobClient = blobContainerClient.GetBlobClient(DataProtectionDefaults.AzureDataProtectionKeyFile);

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
        //public static void AddServerAuthentication(this IServiceCollection services)
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
        //public static IMvcBuilder AddServerMvc(this IServiceCollection services)
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
        //        options.ModelMetadataDetailsProviders.Add(new ServerMetadataProvider());

        //        //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
        //        //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
        //        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => ServerValidationDefaults.NotNullValidationLocaleName);
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
        //public static void AddServerRedirectResultExecutor(this IServiceCollection services)
        //{
        //    //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
        //    services.AddScoped<IActionResultExecutor<RedirectResult>, ServerRedirectResultExecutor>();
        //}

        ///// <summary>
        ///// Add and configure MiniProfiler service
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddServerMiniProfiler(this IServiceCollection services)
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

        ///// <summary>
        ///// Add and configure WebMarkupMin service
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddServerWebMarkupMin(this IServiceCollection services)
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
        //public static void AddServerWebOptimizer(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    var cssBundling = appSettings.Get<WebOptimizerConfig>().EnableCssBundling;
        //    var jsBundling = appSettings.Get<WebOptimizerConfig>().EnableJavaScriptBundling;

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
        //public static void AddServerHttpClients(this IServiceCollection services)
        //{
        //    //default client
        //    services.AddHttpClient(HttpDefaults.DefaultHttpClient).WithProxy();

        //    //client to request current server
        //    services.AddHttpClient<ServerHttpClient>();

        //    //client to request dragonCorp official site
        //    services.AddHttpClient<ServerHttpClient>().WithProxy();

        //    //client to request reCAPTCHA service
        //    services.AddHttpClient<CaptchaHttpClient>().WithProxy();
        //}

    }

}

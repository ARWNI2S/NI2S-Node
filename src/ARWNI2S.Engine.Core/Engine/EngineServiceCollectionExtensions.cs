using ARWNI2S.Core.Infrastructure;
using ARWNI2S.Engine;
using ARWNI2S.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ARWNI2S.Core.Engine
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class EngineServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a default file provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddNI2SFileProvider(this IServiceCollection services, INiisHostEnvironment hostEnvironment)
        {
            services.TryAddSingleton<INiisFileProvider, NI2SFileProvider>();
        }

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
        /// Register NI2SContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddNI2SContextAccessor(this IServiceCollection services)
        {
            services.TryAddSingleton<INiisContextAccessor, DefaultContextAccessor>();
        }

        ///// <summary>
        ///// Adds services required for anti-forgery support
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddAntiForgery(this IServiceCollection services)
        //{
        //    //override cookie name
        //    services.AddAntiforgery(options =>
        //    {
        //        options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.AntiforgeryCookie}";
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //    });
        //}

        ///// <summary>
        ///// Adds services required for application session state
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddHttpSession(this IServiceCollection services)
        //{
        //    services.AddSession(options =>
        //    {
        //        options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.SessionCookie}";
        //        options.Cookie.HttpOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //    });
        //}

        ///// <summary>
        ///// Adds services required for themes support
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
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
        //public static void AddNopDataProtection(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    if (appSettings.Get<AzureBlobConfig>().Enabled && appSettings.Get<AzureBlobConfig>().StoreDataProtectionKeys)
        //    {
        //        var blobServiceClient = new BlobServiceClient(appSettings.Get<AzureBlobConfig>().ConnectionString);
        //        var blobContainerClient = blobServiceClient.GetBlobContainerClient(appSettings.Get<AzureBlobConfig>().DataProtectionKeysContainerName);
        //        var blobClient = blobContainerClient.GetBlobClient(NopDataProtectionDefaults.AzureDataProtectionKeyFile);

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
        //        var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(NopDataProtectionDefaults.DataProtectionKeysPath);
        //        var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

        //        //configure the data protection system to persist keys to the specified directory
        //        services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
        //    }
        //}

        ///// <summary>
        ///// Adds authentication service
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddNopAuthentication(this IServiceCollection services)
        //{
        //    //set default authentication schemes
        //    var authenticationBuilder = services.AddAuthentication(options =>
        //    {
        //        options.DefaultChallengeScheme = NopAuthenticationDefaults.AuthenticationScheme;
        //        options.DefaultScheme = NopAuthenticationDefaults.AuthenticationScheme;
        //        options.DefaultSignInScheme = NopAuthenticationDefaults.ExternalAuthenticationScheme;
        //    });

        //    //add main cookie authentication
        //    authenticationBuilder.AddCookie(NopAuthenticationDefaults.AuthenticationScheme, options =>
        //    {
        //        options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.AuthenticationCookie}";
        //        options.Cookie.HttpOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        options.LoginPath = NopAuthenticationDefaults.LoginPath;
        //        options.AccessDeniedPath = NopAuthenticationDefaults.AccessDeniedPath;
        //    });

        //    //add external authentication
        //    authenticationBuilder.AddCookie(NopAuthenticationDefaults.ExternalAuthenticationScheme, options =>
        //    {
        //        options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.ExternalAuthenticationCookie}";
        //        options.Cookie.HttpOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        options.LoginPath = NopAuthenticationDefaults.LoginPath;
        //        options.AccessDeniedPath = NopAuthenticationDefaults.AccessDeniedPath;
        //    });

        //    //register and configure external authentication plugins now
        //    var typeFinder = Singleton<ITypeFinder>.Instance;
        //    var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
        //    var externalAuthInstances = externalAuthConfigurations
        //        .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x));

        //    foreach (var instance in externalAuthInstances)
        //        instance.Configure(authenticationBuilder);
        //}

        ///// <summary>
        ///// Add and configure MVC for the application
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        ///// <returns>A builder for configuring MVC services</returns>
        //public static IMvcBuilder AddNopMvc(this IServiceCollection services)
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
        //            options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.TempDataCookie}";
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        });
        //    }

        //    services.AddRazorPages();

        //    //MVC now serializes JSON with camel case names by default, use this code to avoid it
        //    mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

        //    //set some options
        //    mvcBuilder.AddMvcOptions(options =>
        //    {
        //        options.ModelBinderProviders.Insert(1, new NopModelBinderProvider());
        //        //add custom display metadata provider 
        //        options.ModelMetadataDetailsProviders.Add(new NopMetadataProvider());

        //        //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
        //        //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
        //        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => NopValidationDefaults.NotNullValidationLocaleName);
        //    });

        //    //add fluent validation
        //    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        //    //register all available validators from Nop assemblies
        //    var assemblies = mvcBuilder.PartManager.ApplicationParts
        //        .OfType<AssemblyPart>()
        //        .Where(part => part.Name.StartsWith("Nop", StringComparison.InvariantCultureIgnoreCase))
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
        //public static void AddNopRedirectResultExecutor(this IServiceCollection services)
        //{
        //    //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
        //    services.AddScoped<IActionResultExecutor<RedirectResult>, NopRedirectResultExecutor>();
        //}

        ///// <summary>
        ///// Add and configure WebMarkupMin service
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddNopWebMarkupMin(this IServiceCollection services)
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
        //public static void AddNopWebOptimizer(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    var woConfig = appSettings.Get<WebOptimizerConfig>();

        //    if (!woConfig.EnableCssBundling && !woConfig.EnableJavaScriptBundling)
        //    {
        //        services.AddScoped<INopAssetHelper, NopDefaultAssetHelper>();
        //        return;
        //    }

        //    //add minification & bundling
        //    var cssSettings = new CssBundlingSettings
        //    {
        //        FingerprintUrls = false,
        //        Minify = woConfig.EnableCssBundling
        //    };

        //    var codeSettings = new CodeBundlingSettings
        //    {
        //        Minify = woConfig.EnableJavaScriptBundling,
        //        AdjustRelativePaths = false //disable this feature because it breaks function names that have "Url(" at the end
        //    };

        //    services.AddWebOptimizer(null, cssSettings, codeSettings);
        //    services.AddScoped<INopAssetHelper, NopAssetHelper>();
        //}

        ///// <summary>
        ///// Add and configure default HTTP clients
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddNopHttpClients(this IServiceCollection services)
        //{
        //    //default client
        //    services.AddHttpClient(NopHttpDefaults.DefaultHttpClient).WithProxy();

        //    //client to request current store
        //    services.AddHttpClient<StoreHttpClient>();

        //    //client to request nopCommerce official site
        //    services.AddHttpClient<NopHttpClient>().WithProxy();

        //    //client to request reCAPTCHA service
        //    services.AddHttpClient<CaptchaHttpClient>().WithProxy();
        //}
    }
}

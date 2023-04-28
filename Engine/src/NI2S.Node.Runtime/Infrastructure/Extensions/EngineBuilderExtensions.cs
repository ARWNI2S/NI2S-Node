﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Hosting;
using NI2S.Node.Core.Configuration;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Builder;
using System.Reflection;
using System.Threading.Tasks;

namespace NI2S.Node.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IEngineBuilder
    /// </summary>
    public static class EngineBuilderExtensions
    {
        /// <summary>
        /// Configure the engine HTTP request pipeline
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        /* 004 - ... -> nodeEngine.ConfigureEventHandler() */
        public static void ConfigureEventHandler(this IEngineBuilder engine)
        {
            EngineContext.Current.ConfigureEventHandler(engine);
        }

        public static void StartEngine(this IEngineBuilder _)
        {
        }

        /* 005 - ... -> nodeEngine.StartEngineAsync() */
        public static async Task StartEngineAsync(this IEngineBuilder _)
        {
            var engine = EngineContext.Current;

            //further actions are performed only when the database is installed
            // TODO: apply required DataSettingsManager.
            //if (DataSettingsManager.IsDatabaseInstalled())
            //{
            //    //log engine start
            //    await engine.Resolve<ILogger>().InformationAsync("Engine started");

            //    //install and update plugins
            //    var pluginService = engine.Resolve<IPluginService>();
            //    await pluginService.InstallPluginsAsync();
            //    await pluginService.UpdatePluginsAsync();

            //    //update nopCommerce core and db
            //    var migrationManager = engine.Resolve<IMigrationManager>();
            //    var assembly = Assembly.GetAssembly(typeof(EngineBuilderExtensions));
            //    migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
            //    assembly = Assembly.GetAssembly(typeof(IMigrationManager));
            //    migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);

            //    var taskScheduler = engine.Resolve<ITaskScheduler>();
            //    await taskScheduler.InitializeAsync();
            //    taskScheduler.StartScheduler();
            //}
        }

        /// <summary>
        /// Add exception handling
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SExceptionHandler(this IEngineBuilder engine)
        {
            var appSettings = EngineContext.Current.Resolve<NodeSettings>();
            var nodeHostEnvironment = EngineContext.Current.Resolve<INodeHostEnvironment>();
            var useDetailedExceptionPage = appSettings.Get<CommonConfig>().DisplayFullErrorStack || nodeHostEnvironment.IsDevelopment();
            if (useDetailedExceptionPage)
            {
                //get detailed exceptions for developing and testing purposes
                // TODO: apply required DataSettingsManager.
                //engine.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                // TODO: apply required DataSettingsManager.
                //engine.UseExceptionHandler("/Error/Error");
            }

            //TODO: log errors
            //engine.UseExceptionHandler(handler =>
            //{
            //    handler.Run(async context =>
            //    {
            //        var exception = context.Modules.Get<IExceptionHandlerModule>()?.Error;
            //        if (exception == null)
            //            return;

            //        try
            //        {
            //            //check whether database is installed
            //            if (DataSettingsManager.IsDatabaseInstalled())
            //            {
            //                //get current customer
            //                var currentCustomer = await EngineContext.Current.Resolve<IWorkContext>().GetCurrentCustomerAsync();

            //                //log error
            //                await EngineContext.Current.Resolve<ILogger>().ErrorAsync(exception.Message, exception, currentCustomer);
            //            }
            //        }
            //        finally
            //        {
            //            //rethrow the exception to show the error page
            //            ExceptionDispatchInfo.Throw(exception);
            //        }
            //    });
            //});
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 404 status code that do not have a body
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UsePageNotFound(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //engine.UseStatusCodePages(async context =>
            //{
            //    //handle 404 Not Found
            //    if (context.DummyContext.Response.StatusCode == StatusCodes.Status404NotFound)
            //    {
            //        var webHelper = EngineContext.Current.Resolve<INodeHelper>();
            //        if (!webHelper.IsStaticResource())
            //        {
            //            //get original path and query
            //            var originalPath = context.DummyContext.Message.Path;
            //            var originalQueryString = context.DummyContext.Message.QueryString;

            //            if (DataSettingsManager.IsDatabaseInstalled())
            //            {
            //                var commonSettings = EngineContext.Current.Resolve<CommonSettings>();

            //                if (commonSettings.Log404Errors)
            //                {
            //                    var logger = EngineContext.Current.Resolve<ILogger>();
            //                    var workContext = EngineContext.Current.Resolve<IWorkContext>();

            //                    await logger.ErrorAsync($"Error 404. The requested page ({originalPath}) was not found",
            //                        customer: await workContext.GetCurrentCustomerAsync());
            //                }
            //            }

            //            try
            //            {
            //                //get new path
            //                var pageNotFoundPath = "/page-not-found";
            //                //re-execute request with new path
            //                context.DummyContext.Response.Redirect(context.DummyContext.Message.PathBase + pageNotFoundPath);
            //            }
            //            finally
            //            {
            //                //return original path to request
            //                context.DummyContext.Message.QueryString = originalQueryString;
            //                context.DummyContext.Message.Path = originalPath;
            //            }
            //        }
            //    }
            //});
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 400 status code (bad request)
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseBadMessageResult(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //engine.UseStatusCodePages(async context =>
            //{
            //    //handle 404 (Bad request)
            //    if (context.DummyContext.Response.StatusCode == StatusCodes.Status400BadMessage)
            //    {
            //        var logger = EngineContext.Current.Resolve<ILogger>();
            //        var workContext = EngineContext.Current.Resolve<IWorkContext>();
            //        await logger.ErrorAsync("Error 400. Bad request", null, customer: await workContext.GetCurrentCustomerAsync());
            //    }
            //});
        }

        /// <summary>
        /// Configure middleware for dynamically compressing HTTP responses
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SResponseCompression(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //if (!DataSettingsManager.IsDatabaseInstalled())
            //    return;

            ////whether to use compression(gzip by default)
            //if (EngineContext.Current.Resolve<CommonSettings>().UseResponseCompression)
            //    engine.UseResponseCompression();
        }

        /// <summary>
        /// Adds NodeOptimizer to the <see cref="IEngineBuilder"/> request execution pipeline
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SNodeOptimizer(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //var fileProvider = EngineContext.Current.Resolve<INI2SFileProvider>();
            //var nodeHostEnvironment = EngineContext.Current.Resolve<INodeHostEnvironment>();

            //engine.UseNodeOptimizer(nodeHostEnvironment, new[]
            //{
            //    new FileProviderOptions
            //    {
            //        MessagePath =  new PathString("/Plugins"),
            //        FileProvider = new PhysicalFileProvider(fileProvider.MapPath(@"Plugins"))
            //    },
            //    new FileProviderOptions
            //    {
            //        MessagePath =  new PathString("/Themes"),
            //        FileProvider = new PhysicalFileProvider(fileProvider.MapPath(@"Themes"))
            //    }
            //});
        }

        /// <summary>
        /// Configure static file serving
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SStaticFiles(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //var fileProvider = EngineContext.Current.Resolve<INI2SFileProvider>();
            //var appSettings = EngineContext.Current.Resolve<AppSettings>();

            //void staticFileResponse(StaticFileResponseContext context)
            //{
            //    if (!string.IsNullOrEmpty(appSettings.Get<CommonConfig>().StaticFilesCacheControl))
            //        context.Context.Response.Headers.Append(HeaderNames.CacheControl, appSettings.Get<CommonConfig>().StaticFilesCacheControl);
            //}

            ////add handling if sitemaps 
            //engine.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(NI2SSeoDefaults.SitemapXmlDirectory)),
            //    MessagePath = new PathString($"/{NI2SSeoDefaults.SitemapXmlDirectory}"),
            //    OnPrepareResponse = context =>
            //    {
            //        if (!DataSettingsManager.IsDatabaseInstalled() ||
            //            !EngineContext.Current.Resolve<SitemapXmlSettings>().SitemapXmlEnabled)
            //        {
            //            context.Context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //            context.Context.Response.ContentLength = 0;
            //            context.Context.Response.Body = Stream.Null;
            //        }
            //    }
            //});

            ////common static files
            //engine.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = staticFileResponse });

            ////themes static files
            //engine.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(fileProvider.MapPath(@"Themes")),
            //    MessagePath = new PathString("/Themes"),
            //    OnPrepareResponse = staticFileResponse
            //});

            ////plugins static files
            //var staticFileOptions = new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(fileProvider.MapPath(@"Plugins")),
            //    MessagePath = new PathString("/Plugins"),
            //    OnPrepareResponse = staticFileResponse
            //};

            ////exclude files in blacklist
            //if (!string.IsNullOrEmpty(appSettings.Get<CommonConfig>().PluginStaticFileExtensionsBlacklist))
            //{
            //    var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

            //    foreach (var ext in appSettings.Get<CommonConfig>().PluginStaticFileExtensionsBlacklist
            //        .Split(';', ',')
            //        .Select(e => e.Trim().ToLowerInvariant())
            //        .Select(e => $"{(e.StartsWith(".") ? string.Empty : ".")}{e}")
            //        .Where(fileExtensionContentTypeProvider.Mappings.ContainsKey))
            //    {
            //        fileExtensionContentTypeProvider.Mappings.Remove(ext);
            //    }

            //    staticFileOptions.ContentTypeProvider = fileExtensionContentTypeProvider;
            //}

            //engine.UseStaticFiles(staticFileOptions);

            ////add support for backups
            //var provider = new FileExtensionContentTypeProvider
            //{
            //    Mappings = { [".bak"] = MimeTypes.EngineOctetStream }
            //};

            //engine.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(NI2SCommonDefaults.DbBackupsPath)),
            //    MessagePath = new PathString("/db_backups"),
            //    ContentTypeProvider = provider,
            //    OnPrepareResponse = context =>
            //    {
            //        if (!DataSettingsManager.IsDatabaseInstalled() ||
            //            !EngineContext.Current.Resolve<IPermissionService>().AuthorizeAsync(StandardPermissionProvider.ManageMaintenance).Result)
            //        {
            //            context.Context.Response.StatusCode = StatusCodes.Status404NotFound;
            //            context.Context.Response.ContentLength = 0;
            //            context.Context.Response.Body = Stream.Null;
            //        }
            //    }
            //});

            ////add support for webmanifest files
            //provider.Mappings[".webmanifest"] = MimeTypes.EngineManifestJson;

            //engine.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath("icons")),
            //    MessagePath = "/icons",
            //    ContentTypeProvider = provider
            //});

            //if (DataSettingsManager.IsDatabaseInstalled())
            //{
            //    engine.UseStaticFiles(new StaticFileOptions
            //    {
            //        FileProvider = EngineContext.Current.Resolve<IRoxyFilemanFileProvider>(),
            //        MessagePath = new PathString(NI2SRoxyFilemanDefaults.DefaultRootDirectory),
            //        OnPrepareResponse = staticFileResponse
            //    });
            //}

            //if (appSettings.Get<CommonConfig>().ServeUnknownFileTypes)
            //{
            //    engine.UseStaticFiles(new StaticFileOptions
            //    {
            //        FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(".well-known")),
            //        MessagePath = new PathString("/.well-known"),
            //        ServeUnknownFileTypes = true,
            //    });
            //}
        }

        /// <summary>
        /// Configure middleware checking whether requested page is keep alive page
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseKeepAlive(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //engine.UseMiddleware<KeepAliveMiddleware>();
        }

        /// <summary>
        /// Configure middleware checking whether database is installed
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseInstallUrl(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //engine.UseMiddleware<InstallUrlMiddleware>();
        }

        /// <summary>
        /// Adds the authentication middleware, which enables authentication capabilities.
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SAuthentication(this IEngineBuilder engine)
        {
            //check whether database is installed
            // TODO: apply required DataSettingsManager.
            //if (!DataSettingsManager.IsDatabaseInstalled())
            //    return;

            //engine.UseMiddleware<AuthenticationMiddleware>();
        }

        /// <summary>
        /// Configure PDF
        /// </summary>
        public static void UseNI2SPdf(this IEngineBuilder _)
        {
            // TODO: apply required DataSettingsManager.
            //if (!DataSettingsManager.IsDatabaseInstalled())
            //    return;

            //var fileProvider = EngineContext.Current.Resolve<INI2SFileProvider>();
            //var fontPaths = fileProvider.EnumerateFiles(fileProvider.MapPath("~/App_Data/Pdf/"), "*.ttf") ?? Enumerable.Empty<string>();

            ////write placeholder characters instead of unavailable glyphs for both debug/ release configurations
            //QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

            //foreach (var fp in fontPaths)
            //{
            //    FontManager.RegisterFont(File.OpenRead(fp));
            //}
        }

        /// <summary>
        /// Configure the request localization feature
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SMessageLocalization(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //engine.UseMessageLocalization(options =>
            //{
            //    if (!DataSettingsManager.IsDatabaseInstalled())
            //        return;

            //    //prepare supported cultures
            //    var cultures = EngineContext.Current.Resolve<ILanguageService>().GetAllLanguages()
            //        .OrderBy(language => language.DisplayOrder)
            //        .Select(language => new CultureInfo(language.LanguageCulture)).ToList();
            //    options.SupportedCultures = cultures;
            //    options.SupportedUICultures = cultures;
            //    options.DefaultMessageCulture = new MessageCulture(cultures.FirstOrDefault() ?? new CultureInfo(NI2SCommonDefaults.DefaultLanguageCulture));
            //    options.ApplyCurrentCultureToResponseHeaders = true;

            //    //configure culture providers
            //    options.AddInitialMessageCultureProvider(new NI2SSeoUrlCultureProvider());
            //    var cookieMessageCultureProvider = options.MessageCultureProviders.OfType<CookieMessageCultureProvider>().FirstOrDefault();
            //    if (cookieMessageCultureProvider is not null)
            //        cookieMessageCultureProvider.CookieName = $"{NI2SCookieDefaults.Prefix}{NI2SCookieDefaults.CultureCookie}";
            //});
        }

        /// <summary>
        /// Configure Endpoints routing
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SEndpoints(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //Execute the endpoint selected by the routing middleware
            //engine.UseEndpoints(endpoints =>
            //{
            //    //register all routes
            //    EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(endpoints);
            //});
        }

        /// <summary>
        /// Configure applying forwarded headers to their matching fields on the current request.
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SProxy(this IEngineBuilder engine)
        {
            // TODO: apply required DataSettingsManager.
            //var appSettings = EngineContext.Current.Resolve<AppSettings>();

            //if (appSettings.Get<HostingConfig>().UseProxy)
            //{
            //    var options = new ForwardedHeadersOptions
            //    {
            //        ForwardedHeaders = ForwardedHeaders.All,
            //        // IIS already serves as a reverse proxy and will add X-Forwarded headers to all requests,
            //        // so we need to increase this limit, otherwise, passed forwarding headers will be ignored.
            //        ForwardLimit = 2
            //    };

            //    if (!string.IsNullOrEmpty(appSettings.Get<HostingConfig>().ForwardedForHeaderName))
            //        options.ForwardedForHeaderName = appSettings.Get<HostingConfig>().ForwardedForHeaderName;

            //    if (!string.IsNullOrEmpty(appSettings.Get<HostingConfig>().ForwardedProtoHeaderName))
            //        options.ForwardedProtoHeaderName = appSettings.Get<HostingConfig>().ForwardedProtoHeaderName;

            //    if (!string.IsNullOrEmpty(appSettings.Get<HostingConfig>().KnownProxies))
            //    {
            //        foreach (var strIp in appSettings.Get<HostingConfig>().KnownProxies.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            //        {
            //            if (IPAddress.TryParse(strIp, out var ip))
            //                options.KnownProxies.Add(ip);
            //        }

            //        if (options.KnownProxies.Count > 1)
            //            options.ForwardLimit = null; //disable the limit, because KnownProxies is configured
            //    }

            //    //configure forwarding
            //    engine.UseForwardedHeaders(options);
            //}
        }

        /// <summary>
        /// Configure NodeMarkupMin
        /// </summary>
        /// <param name="engine">Builder for configuring an engine's request pipeline</param>
        public static void UseNI2SNodeMarkupMin(this IEngineBuilder engine)
        {
            //check whether database is installed
            // TODO: apply required DataSettingsManager.
            //if (!DataSettingsManager.IsDatabaseInstalled())
            //    return;

            //engine.UseNodeMarkupMin();
        }
    }
}
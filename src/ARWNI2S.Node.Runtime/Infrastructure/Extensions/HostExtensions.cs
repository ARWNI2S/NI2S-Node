using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Migrations;
using ARWNI2S.Node.Services.Installation;
using ARWNI2S.Node.Services.Logging;
using ARWNI2S.Node.Services.Plugins;
using ARWNI2S.Node.Services.ScheduleTasks;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ARWNI2S.Runtime.Infrastructure.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        /// Configure the node NI2S engine
        /// </summary>
        /// <param name="application">Builder for configuring a node's NI2S engine</param>
        public static void ConfigureEngine(this IHost application)
        {
            EngineContext.Current.ConfigureEngine(application);
        }

        /// <summary>
        /// Starts the engine
        /// </summary>
        /// <param name="_">unused</param>
        /// <returns>async task</returns>
        public static async Task StartEngineAsync(this IHost _)
        {
            var engine = EngineContext.Current;

            if (!DataSettingsManager.IsDatabaseInstalled())
            {
                await engine.Resolve<IDatabaseInstaller>().InstallDatabaseAsync();
            }

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //log node start
                await engine.Resolve<ILogService>().InformationAsync("Node started");

                //install and update modules
                var moduleService = engine.Resolve<IModuleService>();
                await moduleService.InstallModulesAsync();
                await moduleService.UpdateModulesAsync();

                //update dragonCorp core and db
                var migrationManager = engine.Resolve<IMigrationManager>();
                var assembly = Assembly.GetAssembly(typeof(HostExtensions));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
                assembly = Assembly.GetAssembly(typeof(IMigrationManager));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);

                var taskScheduler = engine.Resolve<ITaskScheduler>();
                await taskScheduler.InitializeAsync();
                taskScheduler.StartScheduler();
            }
        }

        /// <summary>
        /// Add exception handling
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseNI2SExceptionHandler(this IHost application)
        {
            //TODO: EXCEPTION HANDLER!!!

            var ni2sSettings = EngineContext.Current.Resolve<NI2SSettings>();
            var hostEnvironment = EngineContext.Current.Resolve<IHostEnvironment>();
            var useDetailedExceptions = ni2sSettings.Get<CommonConfig>().DisplayFullErrorStack || hostEnvironment.IsDevelopment();
            if (useDetailedExceptions)
            {
                //get detailed exceptions for developing and testing purposes
                //application.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                //application.UseExceptionHandler("/Error/Error");
            }

            //log errors
            //application.UseExceptionHandler(handler =>
            //{
            //    handler.Run(async context =>
            //    {
            //        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            //        if (exception == null)
            //            return;

            //        try
            //        {
            //            //check whether database is installed
            //            if (DataSettingsManager.IsDatabaseInstalled())
            //            {
            //                //get current user
            //                var currentUser = await EngineContext.Current.Resolve<IWorkContext>().GetCurrentUserAsync();

            //                //log error
            //                await EngineContext.Current.Resolve<ILogService>().ErrorAsync(exception.Message, exception, currentUser);
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
        /// Configure applying forwarded headers to their matching fields on the current request.
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseNodeProxy(this IHost application)
        {
            //TODO: NODE PROXY/VPN/ETC!!!

            var ni2sSettings = EngineContext.Current.Resolve<NI2SSettings>();

            if (ni2sSettings.Get<HostingConfig>().UseProxy)
            {
                //var options = new ForwardedHeadersOptions
                //{
                //    ForwardedHeaders = ForwardedHeaders.All,
                //    // IIS already serves as a reverse proxy and will add X-Forwarded headers to all requests,
                //    // so we need to increase this limit, otherwise, passed forwarding headers will be ignored.
                //    ForwardLimit = 2
                //};

                //if (!string.IsNullOrEmpty(ni2sSettings.Get<HostingConfig>().ForwardedForHeaderName))
                //    options.ForwardedForHeaderName = ni2sSettings.Get<HostingConfig>().ForwardedForHeaderName;

                //if (!string.IsNullOrEmpty(ni2sSettings.Get<HostingConfig>().ForwardedProtoHeaderName))
                //    options.ForwardedProtoHeaderName = ni2sSettings.Get<HostingConfig>().ForwardedProtoHeaderName;

                //if (!string.IsNullOrEmpty(ni2sSettings.Get<HostingConfig>().KnownProxies))
                //{
                //    foreach (var strIp in ni2sSettings.Get<HostingConfig>().KnownProxies.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                //    {
                //        if (IPAddress.TryParse(strIp, out var ip))
                //            options.KnownProxies.Add(ip);
                //    }

                //    if (options.KnownProxies.Count > 1)
                //        options.ForwardLimit = null; //disable the limit, because KnownProxies is configured
                //}

                ////configure forwarding
                //application.UseForwardedHeaders(options);
            }
        }

        public static void UseNI2SClustering(this IHost application)
        {
        }
    }
}

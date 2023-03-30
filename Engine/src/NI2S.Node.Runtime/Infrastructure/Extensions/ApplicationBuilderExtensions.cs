namespace NI2S.Node.Runtime.Infrastructure.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        //public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        //{
        //    //EngineContext.Current.ConfigureRequestPipeline(application);
        //}

        //public static async Task StartEngineAsync(this IApplicationBuilder _)
        //{
        //var engine = EngineContext.Current;

        ////further actions are performed only when the database is installed
        //if (DataSettingsManager.IsDatabaseInstalled())
        //{
        //    //log application start
        //    await engine.Resolve<ILogger>().InformationAsync("Application started");

        //    //install and update plugins
        //    var pluginService = engine.Resolve<IPluginService>();
        //    await pluginService.InstallPluginsAsync();
        //    await pluginService.UpdatePluginsAsync();

        //    //update nopCommerce core and db
        //    var migrationManager = engine.Resolve<IMigrationManager>();
        //    var assembly = Assembly.GetAssembly(typeof(ApplicationBuilderExtensions));
        //    migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
        //    assembly = Assembly.GetAssembly(typeof(IMigrationManager));
        //    migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);

        //    var taskScheduler = engine.Resolve<ITaskScheduler>();
        //    await taskScheduler.InitializeAsync();
        //    taskScheduler.StartScheduler();
        //}
        //}

    }
}

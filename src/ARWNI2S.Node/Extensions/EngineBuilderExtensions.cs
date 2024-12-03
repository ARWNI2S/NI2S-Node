using ARWNI2S.Data;
using ARWNI2S.Data.Migrations;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Services.Logging;
using ARWNI2S.Services.Plugins;
using ARWNI2S.Services.Scheduling;
using System.Reflection;

namespace ARWNI2S.Node.Extensions
{
    public static class EngineBuilderExtensions
    {
        public static void ConfigureLocalNodeRuntime(this IEngineBuilder _)
        {

        }

        /// <summary>
        /// Starts the engine
        /// </summary>
        /// <param name="_">unused</param>
        /// <returns>async task</returns>
        public static async Task StartEngineAsync(this IEngineBuilder _)
        {
            var engine = NI2SEngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //install and update plugins
                var pluginService = engine.Resolve<IPluginService>();
                await pluginService.InstallPluginsAsync();
                await pluginService.UpdatePluginsAsync();

                //update dragonCorp core and db
                var migrationManager = engine.Resolve<IMigrationManager>();
                var assembly = Assembly.GetAssembly(typeof(EngineBuilderExtensions));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
                assembly = Assembly.GetAssembly(typeof(IMigrationManager));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);

                ////start engine lifecycle
                //var engineLifecycle = engine.Resolve<EngineLifecycle>();

                //log node start
                await engine.Resolve<ILogService>().InformationAsync("Node started");

                var taskScheduler = engine.Resolve<IClusterTaskScheduler>();
                await taskScheduler.InitializeAsync();
                taskScheduler.StartScheduler();
            }
        }
    }
}

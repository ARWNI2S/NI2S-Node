// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Hosting;
using System.Threading.Tasks;

namespace NI2S.Node.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IEngineBuilder
    /// </summary>
    public static class NodeEngineExtensions
    {
        public static async Task StartEngineAsync(this NodeEngine _)
        {
            var engine = EngineContext.Current;

            ////further actions are performed only when the database is installed
            //if (DataSettingsManager.IsDatabaseInstalled())
            //{
            //    //log application start
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
    }
}

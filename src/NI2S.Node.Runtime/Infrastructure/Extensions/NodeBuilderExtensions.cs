using NI2S.Node.Hosting.Builder;
using System.Threading.Tasks;

namespace NI2S.Node.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of INodeBuilder
    /// </summary>
    public static class NodeBuilderExtensions
    {
        /// <summary>
        /// Configure the node HTTP request pipeline
        /// </summary>
        /// <param name="node">Builder for configuring an node's request pipeline</param>
        public static void ConfigureRequestPipeline(this INodeBuilder node)
        {
            EngineContext.Current.ConfigureRequestPipeline(node);
        }

        public static async Task StartEngineAsync(this INodeBuilder _)
        {
            var engine = EngineContext.Current;

            //further actions are performed only when the database is installed
            //if (DataSettingsManager.IsDatabaseInstalled())
            //{
            //    //log node start
            //    await engine.Resolve<ILogger>().InformationAsync("Application started");

            //    //install and update plugins
            //    var pluginService = engine.Resolve<IPluginService>();
            //    await pluginService.InstallPluginsAsync();
            //    await pluginService.UpdatePluginsAsync();

            //    //update nopCommerce core and db
            //    var migrationManager = engine.Resolve<IMigrationManager>();
            //    var assembly = Assembly.GetAssembly(typeof(NodeBuilderExtensions));
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

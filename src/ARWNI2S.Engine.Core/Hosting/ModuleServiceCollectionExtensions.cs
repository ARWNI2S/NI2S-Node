using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Hosting
{
    public static class NodeModuleServiceCollectionExtensions
    {
        public static void AddModulesCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEngineModuleManager, EngineModuleManager>();


            //var dataSources = new ObservableCollection<IModuleDataSource>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ModuleOptions>, ConfigureModuleOptions>(
            //serviceProvider => new ConfigureModuleOptions(dataSources)));

            //// Allow global access to the list of modules.
            //services.TryAddSingleton<IModuleDataSource>(s =>
            //{
            //    // Call internal ctor and pass global collection
            //    return new CompositeModuleDataSource(dataSources);
            //});

            ConfigureModules(/*dataSources, */services, configuration);
        }


        public static void AddModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddModulesCore(configuration);
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ModuleOptions>, ModuleConstraintSetup>());
        }

        private static void ConfigureModules(/*ICollection<ModuleDataSource> dataSources, */IServiceCollection services, IConfiguration configuration)
        {
            //find startup configurations provided by locally installed assemblies
            var typeFinder = services.GetOrCreateTypeFinder();
            var modules = typeFinder.FindClassesOfType<IConfigureEngine>();

            //create and sort instances of startup configurations
            var instances = modules
                .Select(startup => (IConfigureEngine)Activator.CreateInstance(startup))
                .Where(startup => startup != null)
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
            {
                try
                {
                    instance.ConfigureServices(services, configuration);

                    //if (instance is IModule module)
                    //    //add to the global list of modules
                    //    dataSources.Add(module.DataSource);
                }
                catch (Exception ex)
                {
                    throw new ModuleLoadException($"An error occurred during the configuration of the engine module '{instance.GetType().Name}'.", ex);
                }

            }
        }
    }
}
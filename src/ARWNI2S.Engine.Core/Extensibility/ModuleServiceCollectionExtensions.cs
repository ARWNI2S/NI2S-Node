// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Extensibility.Internals;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace ARWNI2S.Engine.Extensibility
{
    internal class ModulesMarkerService { }

    internal static class ModuleServiceCollectionExtensions
    {
        internal static void AddModulesCore(this IServiceCollection services, IConfiguration configuration)
        {

            var dataSources = new ObservableCollection<IModuleDataSource>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ModuleBuilderOptions>, ConfigureModuleBuilderOptions>(
                    serviceProvider => new ConfigureModuleBuilderOptions(dataSources)));

            // Allow global access to the list of modules.
            services.TryAddSingleton<IModuleDataSource>(sp =>
            {
                // Call internal ctor and pass global collection
                return new CompositeModuleDataSource(dataSources);
            });

            ConfigureModules(dataSources, services, configuration);

            services.AddSingleton<ModulesMarkerService>();
        }


        internal static void AddModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddModulesCore(configuration);
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ModuleBuilderOptions>, ModuleConstraintSetup>());
        }

        private static void ConfigureModules(ObservableCollection<IModuleDataSource> dataSources, IServiceCollection services, IConfiguration configuration)
        {
            //find startup configurations provided by locally installed assemblies
            var typeFinder = services.GetOrCreateTypeFinder();
            var configurations = typeFinder.FindClassesOfType<IConfigureEngine>();

            //create and sort instances of startup configurations
            var instances = configurations
                .Select(startup => (IConfigureEngine)Activator.CreateInstance(startup))
                .Where(startup => startup != null)
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
            {
                try
                {
                    instance.ConfigureServices(services, configuration);

                    if (instance is IBuildModuleSupported buildableModule)
                    {
                        //add to the global list of modules
                        if (buildableModule.DataSource != null)
                            dataSources.Add(buildableModule.DataSource);
                        else
                            dataSources.Add(new EmptyDataSource());
                    }

                }
                catch (Exception ex)
                {
                    throw new ModuleLoadException($"An error occurred during the configuration of the engine module '{instance.GetType().Name}'.", ex);
                }
            }
        }
    }
}
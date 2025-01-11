using ARWNI2S.Cluster.Builder;
using ARWNI2S.Cluster.Extensibility;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Node.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Extensibility
{
    internal static class ModuleEngineBuilderExtensions
    {
        internal static IEngineBuilder UseModules(this IEngineBuilder engine)
        {
            ArgumentNullException.ThrowIfNull(engine);

            VerifyModulesServicesAreRegistered(engine);

            IClusterNodeBuilder clusterNodeBuilder;
            if (engine.Properties.TryGetValue(NI2SHostingDefaults.GlobalNodeBuilderKey, out var obj))
            {
                clusterNodeBuilder = (IClusterNodeBuilder)obj!;
                // Let interested parties know if UseRouting() was called while a global route builder was set
                engine.Properties[NI2SHostingDefaults.NodeBuilderKey] = clusterNodeBuilder;
            }
            else
            {
                clusterNodeBuilder = new DefaultClusterNodeBuilder(engine);
                engine.Properties[NI2SHostingDefaults.NodeBuilderKey] = clusterNodeBuilder;
            }

            // Add UseRouting function to properties so that middleware that can't reference UseRouting directly can call UseRouting via this property
            // This is part of the global endpoint route builder concept
            engine.Properties.TryAdd(NI2SHostingDefaults.UseModulesKey, (object)UseModules);

            return engine.UseNodeModules(clusterNodeBuilder);
        }

        private static void VerifyModulesServicesAreRegistered(IEngineBuilder engine)
        {
            // Verify if AddModules was done before calling UseNodeModules
            // We use the RoutingMarkerService to make sure if all the services were added.
            if (engine.EngineServices.GetService(typeof(ModulesMarkerService)) == null)
            {
                throw new InvalidOperationException(string.Format("Unable to find services, initialization skipped to call {0} {1} while executing ConfigureServices(...) ",
                    nameof(IServiceCollection),
                    nameof(ModuleServiceCollectionExtensions.AddModules)));
            }
        }
    }
}

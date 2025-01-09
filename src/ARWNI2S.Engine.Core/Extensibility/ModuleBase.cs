using ARWNI2S.Engine.Builder;
using ARWNI2S.Environment;
using ARWNI2S.Extensibility;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Extensibility
{
    public abstract class ModuleBase : IModule
    {
        /// <summary>
        /// Gets or sets the module system name.
        /// </summary>
        public abstract string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the module friendly name.
        /// </summary>
        public abstract string DisplayName { get; set; }

        /// <summary>
        /// Gets the order of the module.
        /// </summary>
        public virtual int Order => NI2SLifecycleStage.RuntimeInitialize;

        public virtual IList<string> ModuleDependencies { get; } = [];

        /// <summary>
        /// Gets the global type finder.
        /// </summary>
        protected readonly ITypeFinder TypeFinder = Singleton<ITypeFinder>.Instance;

        /// <summary>
        /// Add and configure any of the module services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Host configuration</param>
        public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added module
        /// </summary>
        /// <param name="engineBuilder">The engine builde</param>
        public virtual void ConfigureEngine(IEngineBuilder engineBuilder)
        {
            var module = engineBuilder.EngineServices.GetKeyedService<IModule>(SystemName);
            engineBuilder.EngineServices.GetRequiredService<IModuleManager>().Register(module);
        }
    }
}

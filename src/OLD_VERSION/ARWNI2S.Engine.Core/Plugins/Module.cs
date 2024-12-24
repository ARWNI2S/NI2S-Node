using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Plugins
{
    public class Module : IEngineModule
    {
        /// <summary>
        /// Add and configure any of the engine services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //plugins
            services.AddScoped<IPluginService, PluginService>();
            services.AddScoped<PluginDiscovery>();

            //plugin managers
            services.AddScoped(typeof(IPluginManager<>), typeof(PluginManager<>));
        }

        /// <summary>
        /// Configure the using of added components
        /// </summary>
        /// <param name="engine">Builder for configuring a node's NI2S engine</param>
        public void ConfigureEngine(IEngineBuilder engine)
        {

        }

        ///// <summary>
        ///// Gets order of this startup configuration implementation
        ///// </summary>
        //public int Order => InitStage.DbInit;
    }
}

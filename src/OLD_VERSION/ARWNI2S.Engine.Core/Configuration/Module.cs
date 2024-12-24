using ARWNI2S.Data;
using ARWNI2S.Engine.Clustering;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Configuration
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
            services.AddScoped<ISettingService, SettingService>();

            //register all settings
            var settings = Singleton<ITypeFinder>.Instance.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
            {
                services.AddScoped(setting, serviceProvider =>
                {
                    var storeId = DataSettingsManager.IsDatabaseInstalled()
                        ? serviceProvider.GetRequiredService<IClusterContext>().GetCurrentNode()?.Id ?? 0
                        : 0;

                    return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, storeId).Result;
                });
            }

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

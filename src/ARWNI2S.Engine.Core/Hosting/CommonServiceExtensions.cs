using ARWNI2S.Configuration;
using ARWNI2S.Engine.Configuration;
using ARWNI2S.Environment;
using ARWNI2S.Node.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Hosting
{
    public static class CommonServiceExtensions
    {
        public static ITypeFinder GetOrCreateTypeFinder(this IServiceCollection services)
        {
            if (Singleton<ITypeFinder>.Instance == null)
            {
                //register type finder
                Singleton<ITypeFinder>.Instance = new NI2STypeFinder();
                services.AddSingleton(sp => Singleton<ITypeFinder>.Instance);
            }
            return Singleton<ITypeFinder>.Instance;
        }

        public static void ConfigureFileProvider(this IServiceCollection services, INiisHostEnvironment hostingEnvironment)
        {
            NI2SFileProvider.Default = new NI2SFileProvider(hostingEnvironment);
            services.AddScoped<INiisFileProvider, NI2SFileProvider>();
        }

        public static NodeSettings ConfigureNodeSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //add configuration parameters
            var configurations = services.GetOrCreateTypeFinder()
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

            return NI2SSettingsHelper.SaveNodeSettings(configurations, NI2SFileProvider.Default, false);
        }
    }
}

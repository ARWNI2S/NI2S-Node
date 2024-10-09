using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Hosting;
using ARWNI2S.Node.Hosting.Extensions;
using Autofac.Extensions.DependencyInjection;

namespace ARWNI2S.Node
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = NodeHostBuilder.Create(args);

            // Configurar los archivos de configuración
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environment = hostingContext.HostingEnvironment;
                config.AddJsonFile(ConfigurationDefaults.AppSettingsFilePath, optional: true, reloadOnChange: true);

                if (!string.IsNullOrEmpty(environment.EnvironmentName))
                {
                    var path = string.Format(ConfigurationDefaults.AppSettingsEnvironmentFilePath, environment.EnvironmentName);
                    config.AddJsonFile(path, optional: true, reloadOnChange: true);
                }

                config.AddEnvironmentVariables();
            });

            // Configurar servicios de la aplicación y ajustes
            builder.ConfigureServices((context, services) =>
            {
                // Load application settings
                services.ConfigureApplicationSettings(context);

                var appSettings = Singleton<AppSettings>.Instance;
                var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

                if (useAutofac)
                    builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                else
                    builder.UseDefaultServiceProvider(options =>
                    {
                        //we don't validate the scopes, since at the app start and the initial configuration we need 
                        //to resolve some services (registered as "scoped") through the root container
                        options.ValidateScopes = false;
                        options.ValidateOnBuild = true;
                    });

                services.ConfigureApplicationServices(context);
            });

            // Construir y ejecutar el host
            var host = builder.Build();
            host.Run();
        }
    }
}
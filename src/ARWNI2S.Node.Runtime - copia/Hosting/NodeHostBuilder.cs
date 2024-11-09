using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Runtime.Hosting.Extensions;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Hosting
{
    public sealed class NodeHostBuilder : HostBuilder/*NodeServerHostBuilder<StringPackageInfo>*/, IHostBuilder
    {
        //private NodeHostBuilder(string[] args)
        //    : base(args) { }

        //public override IHost Build()
        //{
        //    return base.Build();
        //}

        //protected override void RegisterBasicServices(HostBuilderContext builderContext, IServiceCollection servicesInHost, IServiceCollection services)
        //{
        //    base.RegisterBasicServices(builderContext, servicesInHost, services);
        //}

        //protected override void RegisterDefaultHostedService(IServiceCollection servicesInHost)
        //{
        //    base.RegisterDefaultHostedService(servicesInHost);
        //}

        //protected override void RegisterDefaultServices(HostBuilderContext builderContext, IServiceCollection servicesInHost, IServiceCollection services)
        //{
        //    services.TryAddSingleton<INodeContext, LocalNodeContext>();
        //    services.TryAddSingleton<EngineLifecycle>();
        //    services.TryAddFromExisting<IEngineLifecycleSubject, EngineLifecycle>();
        //    services.TryAddFromExisting<IEngineLifecycle, EngineLifecycle>();

        //    base.RegisterDefaultServices(builderContext, servicesInHost, services);
        //}


        public static IHost CreateRuntimeHost(string[] args)
        {
            var host = Create(args).Build();

            host.ConfigureEngine();

            return host;
        }

        public static async Task<IHost> CreateRuntimeHostAsync(string[] args, bool startEngine = false)
        {
            var host = CreateRuntimeHost(args);

            if (startEngine)
                await host.StartEngineAsync();

            return host;
        }

        public static IHostBuilder Create(string[] args)
        {
            NodeHostBuilder builder = new(/*args*/);

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environment = hostingContext.HostingEnvironment;
                config.AddJsonFile(ConfigurationDefaults.NodeSettingsFilePath, optional: true, reloadOnChange: true);

                if (!string.IsNullOrEmpty(environment.EnvironmentName))
                {
                    var path = string.Format(ConfigurationDefaults.NodeSettingsEnvironmentFilePath, environment.EnvironmentName);
                    config.AddJsonFile(path, optional: true, reloadOnChange: true);
                }

                config.AddEnvironmentVariables();
            });

            //builder.ConfigureNodeServerHost();

            // Configurar servicios de la aplicación y ajustes
            builder.ConfigureServices((context, services) =>
            {
                // Load application settings
                services.ConfigureApplicationSettings(context);

                var ni2sSettings = Singleton<NI2SSettings>.Instance;
                var useAutofac = ni2sSettings.Get<CommonConfig>().UseAutofac;

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

            return builder;
        }
    }
}

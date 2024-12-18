using ARWNI2S.Node.Hosting.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internals
{
    internal abstract class NodeHostBuilderBase : INiisHostBuilder//TODO:, ISupportsUseDefaultServiceProvider remove or add?
    {
        private protected readonly IHostBuilder _builder;
        private protected readonly IConfiguration _config;

        public NodeHostBuilderBase(IHostBuilder builder, NI2SHostBuilderOptions options)
        {
            _builder = builder;
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection();

            if (!options.SuppressEnvironmentConfiguration)
            {
                configBuilder.AddEnvironmentVariables(prefix: "ARWNI2S_");
            }

            _config = configBuilder.Build();
        }

        public INiisHost Build()
        {
            throw new NotSupportedException($"Building this implementation of {nameof(INiisHostBuilder)} is not supported.");
        }

        public INiisHostBuilder ConfigureNI2SConfiguration(Action<NI2SHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _builder.ConfigureAppConfiguration((context, builder) =>
            {
                var niishostBuilderContext = GetNI2SHostBuilderContext(context);
                configureDelegate(niishostBuilderContext, builder);
            });

            return this;
        }

        public INiisHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        public INiisHostBuilder ConfigureServices(Action<NI2SHostBuilderContext, IServiceCollection> configureServices)
        {
            _builder.ConfigureServices((context, builder) =>
            {
                var niishostBuilderContext = GetNI2SHostBuilderContext(context);
                configureServices(niishostBuilderContext, builder);
            });

            return this;
        }

        public INiisHostBuilder UseDefaultServiceProvider(Action<NI2SHostBuilderContext, ServiceProviderOptions> configure)
        {
            _builder.UseServiceProviderFactory(context =>
            {
                var niisHostBuilderContext = GetNI2SHostBuilderContext(context);
                var options = new ServiceProviderOptions();
                configure(niisHostBuilderContext, options);
                return new DefaultServiceProviderFactory(options);
            });

            return this;
        }

        protected NI2SHostBuilderContext GetNI2SHostBuilderContext(HostBuilderContext context)
        {
            if (!context.Properties.TryGetValue(typeof(NI2SHostBuilderContext), out var contextVal))
            {
                // Use _config as a fallback for NI2SHostingOptions in case the chained source was removed from the hosting IConfigurationBuilder.
                var options = new NI2SHostingOptions(context.Configuration, fallbackConfiguration: _config, environment: context.HostingEnvironment);
                var niisHostBuilderContext = new NI2SHostBuilderContext
                {
                    Configuration = context.Configuration,
                    HostingEnvironment = new HostingEnvironment(),
                };
                niisHostBuilderContext.HostingEnvironment.Initialize(context.HostingEnvironment.ContentRootPath, options, baseEnvironment: context.HostingEnvironment);
                context.Properties[typeof(NI2SHostBuilderContext)] = niisHostBuilderContext;
                context.Properties[typeof(NI2SHostingOptions)] = options;
                return niisHostBuilderContext;
            }

            // Refresh config, it's periodically updated/replaced
            var niisHostContext = (NI2SHostBuilderContext)contextVal;
            niisHostContext.Configuration = context.Configuration;
            return niisHostContext;
        }

        //HACK
        //protected NI2SSettings InitializeNodeEngineSettings(HostBuilderContext context, NI2SHostBuilderContext niisContext, IServiceCollection services)
        //{
        //    //create default file provider
        //    CommonHelper.DefaultFileProvider ??= new NI2SFileProvider(niisContext.HostingEnvironment);

        //    if (!context.Properties.TryGetValue(typeof(NI2SSettings), out var settingsVal))
        //    {
        //        //add configuration parameters
        //        var configurations = services.GetOrCreateTypeFinder()
        //            .FindClassesOfType<IConfig>()
        //            .Select(configType => (IConfig)Activator.CreateInstance(configType))
        //            .ToList();

        //        foreach (var config in configurations)
        //            context.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

        //        settingsVal = NI2SSettingsHelper.SaveNI2SSettings(configurations, CommonHelper.DefaultFileProvider, false);
        //        context.Properties[typeof(NI2SSettings)] = settingsVal;
        //    }

        //    var niisSettings = (NI2SSettings)settingsVal;
        //    return niisSettings;
        //}

        public string GetSetting(string key)
        {
            return _config[key];
        }

        public INiisHostBuilder UseSetting(string key, string value)
        {
            _config[key] = value;
            return this;
        }
    }
}

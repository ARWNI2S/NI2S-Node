using ARWNI2S.Hosting.LocalAssets;
using ARWNI2S.Hosting.Node;

namespace ARWNI2S.Hosting
{
    public static class NI2SHostBuilderExtensions
    {
        public static INiisHostBuilder Configure(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, IEngineBuilder> configureApp)
        {
            Action<NI2SHostBuilderContext, IEngineBuilder> configureApp2 = configureApp;
            ArgumentNullException.ThrowIfNull(configureApp2, "configureApp");
            if (hostBuilder is GenericNI2SHostBuilder genericBuilderp)
            {
                return genericBuilderp.Configure(configureApp2);
            }
            string name = configureApp2.GetMethodInfo().DeclaringType.Assembly.GetName().Name;
            hostBuilder.UseSetting(NI2SHostDefaults.ApplicationKey, name);
            return hostBuilder.ConfigureServices(delegate (NI2SHostBuilderContext context, IServiceCollection services)
            {
                //services.AddSingleton((Func<IServiceProvider, IStartup>)((IServiceProvider sp) => new DelegateStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), delegate (IEngineBuilder app)
                //{
                //    configureApp2(context, app);
                //})));
            });
        }

        public static INiisHostBuilder UseDefaultServiceProvider(this INiisHostBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        {
            Action<ServiceProviderOptions> configure2 = configure;
            return hostBuilder.UseDefaultServiceProvider(delegate (NI2SHostBuilderContext context, ServiceProviderOptions options)
            {
                configure2(options);
            });
        }

        public static INiisHostBuilder UseDefaultServiceProvider(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, ServiceProviderOptions> configure)
        {
            Action<NI2SHostBuilderContext, ServiceProviderOptions> configure2 = configure;
            //if (hostBuilder is ISupportsUseDefaultServiceProvider supportsUseDefaultServiceProvider)
            //{
            //    return supportsUseDefaultServiceProvider.UseDefaultServiceProvider(configure2);
            //}
            return hostBuilder.ConfigureServices(delegate (NI2SHostBuilderContext context, IServiceCollection services)
            {
                ServiceProviderOptions serviceProviderOptions = new ServiceProviderOptions();
                configure2(context, serviceProviderOptions);
                services.Replace(ServiceDescriptor.Singleton((IServiceProviderFactory<IServiceCollection>)new DefaultServiceProviderFactory(serviceProviderOptions)));
            });
        }

        public static INiisHostBuilder ConfigureAppConfiguration(this INiisHostBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            Action<IConfigurationBuilder> configureDelegate2 = configureDelegate;
            return hostBuilder.ConfigureNI2SConfiguration(delegate (NI2SHostBuilderContext context, IConfigurationBuilder builder)
            {
                configureDelegate2(builder);
            });
        }

        public static INiisHostBuilder ConfigureLogging(this INiisHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            Action<ILoggingBuilder> configureLogging2 = configureLogging;
            return hostBuilder.ConfigureServices(delegate (IServiceCollection collection)
            {
                collection.AddLogging(configureLogging2);
            });
        }

        public static INiisHostBuilder ConfigureLogging(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, ILoggingBuilder> configureLogging)
        {
            Action<NI2SHostBuilderContext, ILoggingBuilder> configureLogging2 = configureLogging;
            return hostBuilder.ConfigureServices(delegate (NI2SHostBuilderContext context, IServiceCollection collection)
            {
                collection.AddLogging(delegate (ILoggingBuilder builder)
                {
                    configureLogging2(context, builder);
                });
            });
        }

        public static INiisHostBuilder UseLocalAssets(this INiisHostBuilder builder)
        {
            builder.ConfigureNI2SConfiguration(delegate (NI2SHostBuilderContext context, IConfigurationBuilder configBuilder)
            {
                LocalAssetsLoader.UseLocalAssets(context.HostingEnvironment, context.Configuration);
            });
            return builder;
        }
    }
}

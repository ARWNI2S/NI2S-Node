using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internal
{
    /// <summary>
    /// Extension methods for configuring the <see cref="IHostBuilder" />.
    /// </summary>
    internal static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures a <see cref="IHostBuilder" /> with defaults for hosting a web app. This should be called
        /// before application specific configuration to avoid it overwriting provided services, configuration sources,
        /// environments, content root, etc.
        /// </summary>
        /// <remarks>
        /// The following defaults are applied to the <see cref="IHostBuilder"/>:
        /// <list type="bullet">
        ///     <item><description>use Kestrel as the web server and configure it using the application's configuration providers</description></item>
        ///     <item><description>configure <see cref="INiisHostEnvironment.NodeRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
        ///     <item><description>adds the HostFiltering middleware</description></item>
        ///     <item><description>adds the ForwardedHeaders middleware if ARWNI2S_FORWARDEDHEADERS_ENABLED=true,</description></item>
        ///     <item><description>enable IIS integration</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        internal static IHostBuilder ConfigureNI2SHostingDefaults(this IHostBuilder builder, Action<INiisHostBuilder> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            return builder.ConfigureNI2SHost(niisHostBuilder =>
            {
                DefaultNodeServices.ConfigureNI2SDefaults(niisHostBuilder);
                configure(niisHostBuilder);
            });
        }

        private static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Action<INiisHostBuilder> configure, Func<IHostBuilder, INiisHostBuilder> createNI2SHostBuilder = null)
        {
            ArgumentNullException.ThrowIfNull(configure, nameof(configure));
            createNI2SHostBuilder ??= (hostBuilder => new NI2SHostBuilder(hostBuilder));
            configure(createNI2SHostBuilder(builder));
            builder.ConfigureServices((HostBuilderContext context, IServiceCollection services) => //7 LAST
            {
                // Lifecycle
                services.AddSingleton<NI2SHostService>();
                services.AddSingleton<IHostedLifecycleService>(provider => provider.GetRequiredService<NI2SHostService>()); ;
                services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<NI2SHostService>());
            });
            return builder;
        }

        //public static IHostBuilder ConfigureNodeServiceProvider(this IHostBuilder builder)
        //{
        //    //ArgumentNullException.ThrowIfNull(configure);

        //    //return builder.ConfigureNI2SHost(niisHostBuilder =>
        //    //{
        //    //    DefaultNodeServices.ConfigureNI2SDefaults(niisHostBuilder);
        //    //    configure(niisHostBuilder);
        //    //});
        //}

        //private void ApplyServiceProviderFactory(HostApplicationBuilder hostApplicationBuilder)
        //{
        //    var useAutofac = Settings.Get<CommonConfig>().UseAutofac;

        //    if (useAutofac)
        //        UseServiceProviderFactory(new AutofacServiceProviderFactory());
        //    else
        //        this.UseDefaultServiceProvider(options =>
        //        {
        //            //we don't validate the scopes, since at the app start and the initial configuration we need 
        //            //to resolve some services (registered as "scoped") through the root container
        //            options.ValidateScopes = false;
        //            options.ValidateOnBuild = true;
        //        });


        //    void ConfigureContainerBuilderAdapter(object containerBuilder)
        //    {
        //        foreach (var action in _configureContainerActions)
        //        {
        //            action(_context, containerBuilder);
        //        }
        //    }

        //    hostApplicationBuilder.ConfigureContainer(_serviceProviderFactory, ConfigureContainerBuilderAdapter);
        //}

    }
}

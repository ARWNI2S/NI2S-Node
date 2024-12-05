using ARWNI2S.Engine.Builder;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting.Infrastructure;
using ARWNI2S.Node.Hosting.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ARWNI2S.Node.Hosting.Extensions
{
    public static class NodeHostBuilderExtensions
    {
        public static INodeHostBuilder Configure(this INodeHostBuilder hostBuilder, Action<IEngineBuilder> configureApp)
        {
            Action<IEngineBuilder> configureApp2 = configureApp;
            ArgumentNullException.ThrowIfNull(configureApp2, "configureApp");
            if (hostBuilder is ISupportsNodeStartup supportsStartup)
            {
                return supportsStartup.Configure(configureApp2);
            }
            string name = configureApp2.GetMethodInfo().DeclaringType.Assembly.GetName().Name;
            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, name);
            return hostBuilder.ConfigureServices(delegate (NodeHostBuilderContext context, IServiceCollection services)
            {
                services.AddSingleton((Func<IServiceProvider, INodeStartup>)((IServiceProvider sp) => new DelegateStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), delegate (IEngineBuilder app)
                {
                    configureApp2(app);
                })));
            });
        }

        public static INodeHostBuilder Configure(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, IEngineBuilder> configureApp)
        {
            Action<NodeHostBuilderContext, IEngineBuilder> configureApp2 = configureApp;
            ArgumentNullException.ThrowIfNull(configureApp2, "configureApp");
            if (hostBuilder is ISupportsNodeStartup supportsStartup)
            {
                return supportsStartup.Configure(configureApp2);
            }
            string name = configureApp2.GetMethodInfo().DeclaringType.Assembly.GetName().Name;
            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, name);
            return hostBuilder.ConfigureServices(delegate (NodeHostBuilderContext context, IServiceCollection services)
            {
                services.AddSingleton((Func<IServiceProvider, INodeStartup>)((IServiceProvider sp) => new DelegateStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), delegate (IEngineBuilder app)
                {
                    configureApp2(context, app);
                })));
            });
        }

        public static INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(this INodeHostBuilder hostBuilder, Func<NodeHostBuilderContext, TStartup> startupFactory) where TStartup : class
        {
            Func<NodeHostBuilderContext, TStartup> startupFactory2 = startupFactory;
            ArgumentNullException.ThrowIfNull(startupFactory2, "startupFactory");
            if (hostBuilder is ISupportsNodeStartup supportsStartup)
            {
                return supportsStartup.UseStartup(startupFactory2);
            }
            string name = startupFactory2.GetMethodInfo().DeclaringType.Assembly.GetName().Name;
            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, name);
            return hostBuilder.ConfigureServices(delegate (NodeHostBuilderContext context, IServiceCollection services)
            {
                services.AddSingleton(typeof(INodeStartup), GetStartupInstance);
                [UnconditionalSuppressMessage("Trimmer", "IL2072", Justification = "Startup type created by factory can't be determined statically.")]
                object GetStartupInstance(IServiceProvider serviceProvider)
                {
                    TStartup val = startupFactory2(context) ?? throw new InvalidOperationException("The specified factory returned null startup instance.");
                    IHostEnvironment requiredService = serviceProvider.GetRequiredService<IHostEnvironment>();
                    if (val is INodeStartup result)
                    {
                        return result;
                    }
                    return new ConventionBasedStartup(StartupLoader.LoadMethods(serviceProvider, val.GetType(), requiredService.EnvironmentName, val));
                }
            });
        }

        public static INodeHostBuilder UseStartup(this INodeHostBuilder hostBuilder, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods)] Type startupType)
        {
            Type startupType2 = startupType;
            ArgumentNullException.ThrowIfNull(startupType2, "startupType");
            if (hostBuilder is ISupportsNodeStartup supportsStartup)
            {
                return supportsStartup.UseStartup(startupType2);
            }
            string name = startupType2.Assembly.GetName().Name;
            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, name);
            return hostBuilder.ConfigureServices(delegate (IServiceCollection services)
            {
                if (typeof(INodeStartup).IsAssignableFrom(startupType2))
                {
                    services.AddSingleton(typeof(INodeStartup), startupType2);
                }
                else
                {
                    services.AddSingleton(typeof(INodeStartup), delegate (IServiceProvider sp)
                    {
                        IHostEnvironment requiredService = sp.GetRequiredService<IHostEnvironment>();
                        return new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType2, requiredService.EnvironmentName));
                    });
                }
            });
        }

        public static INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(this INodeHostBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }

        public static INodeHostBuilder UseDefaultServiceProvider(this INodeHostBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        {
            Action<ServiceProviderOptions> configure2 = configure;
            return hostBuilder.UseDefaultServiceProvider(delegate (NodeHostBuilderContext context, ServiceProviderOptions options)
            {
                configure2(options);
            });
        }

        public static INodeHostBuilder UseDefaultServiceProvider(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, ServiceProviderOptions> configure)
        {
            Action<NodeHostBuilderContext, ServiceProviderOptions> configure2 = configure;
            if (hostBuilder is ISupportsUseDefaultServiceProvider supportsUseDefaultServiceProvider)
            {
                return supportsUseDefaultServiceProvider.UseDefaultServiceProvider(configure2);
            }
            return hostBuilder.ConfigureServices(delegate (NodeHostBuilderContext context, IServiceCollection services)
            {
                ServiceProviderOptions serviceProviderOptions = new ServiceProviderOptions();
                configure2(context, serviceProviderOptions);
                services.Replace(ServiceDescriptor.Singleton((IServiceProviderFactory<IServiceCollection>)new DefaultServiceProviderFactory(serviceProviderOptions)));
            });
        }

        public static INodeHostBuilder ConfigureAppConfiguration(this INodeHostBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            Action<IConfigurationBuilder> configureDelegate2 = configureDelegate;
            return hostBuilder.ConfigureAppConfiguration(delegate (NodeHostBuilderContext context, IConfigurationBuilder builder)
            {
                configureDelegate2(builder);
            });
        }

        public static INodeHostBuilder ConfigureLogging(this INodeHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            Action<ILoggingBuilder> configureLogging2 = configureLogging;
            return hostBuilder.ConfigureServices(delegate (IServiceCollection collection)
            {
                collection.AddLogging(configureLogging2);
            });
        }

        public static INodeHostBuilder ConfigureLogging(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, ILoggingBuilder> configureLogging)
        {
            Action<NodeHostBuilderContext, ILoggingBuilder> configureLogging2 = configureLogging;
            return hostBuilder.ConfigureServices(delegate (NodeHostBuilderContext context, IServiceCollection collection)
            {
                collection.AddLogging(delegate (ILoggingBuilder builder)
                {
                    configureLogging2(context, builder);
                });
            });
        }

        public static INodeHostBuilder UseLocalAssets(this INodeHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(delegate (NodeHostBuilderContext context, IConfigurationBuilder configBuilder)
            {
                NodeAssetsLoader.UseLocalAssets(context.HostingEnvironment, context.Configuration);
            });
            return builder;
        }
    }
}

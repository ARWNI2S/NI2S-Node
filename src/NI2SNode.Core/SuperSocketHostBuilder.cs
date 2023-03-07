using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;
using NI2S.Node.Server;

namespace NI2S.Node
{
    public class SuperSocketHostBuilder<TReceivePackage> : HostBuilderAdapter<SuperSocketHostBuilder<TReceivePackage>>, INodeHostBuilder<TReceivePackage>, IHostBuilder
    {
        private Func<HostBuilderContext, IConfiguration, IConfiguration>? _serverOptionsReader;

        protected List<Action<HostBuilderContext, IServiceCollection>> ConfigureServicesActions { get; private set; } = new List<Action<HostBuilderContext, IServiceCollection>>();

        protected List<Action<HostBuilderContext, IServiceCollection>> ConfigureSupplementServicesActions = new();

        public SuperSocketHostBuilder(IHostBuilder hostBuilder)
            : base(hostBuilder)
        {

        }

        public SuperSocketHostBuilder(string[]? args = null)
            : base(args)
        {

        }

        private void ConfigureHostBuilder()
        {
            HostBuilder.ConfigureServices((ctx, services) =>
            {
                RegisterBasicServices(ctx, services, services);
            }).ConfigureServices((ctx, services) =>
            {
                foreach (var action in ConfigureServicesActions)
                {
                    action(ctx, services);
                }

                foreach (var action in ConfigureSupplementServicesActions)
                {
                    action(ctx, services);
                }
            }).ConfigureServices((ctx, services) =>
            {
                RegisterDefaultServices(ctx, services, services);
            });
        }

        void IMinimalApiHostBuilder.ConfigureHostBuilder()
        {
            ConfigureHostBuilder();
        }

        public override IHost Build()
        {
            ConfigureHostBuilder();
            return HostBuilder.Build();
        }

        public INodeHostBuilder<TReceivePackage> ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            ConfigureSupplementServicesActions.Add(configureDelegate);
            return this;
        }

        INodeHostBuilder INodeHostBuilder.ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return ConfigureSupplementServices(configureDelegate);
        }

        protected virtual void RegisterBasicServices(HostBuilderContext builderContext, IServiceCollection servicesInHost, IServiceCollection services)
        {
            var serverOptionReader = _serverOptionsReader ?? ((ctx, config) =>
                {
                    return config;
                });
            services.AddOptions();

            var config = builderContext.Configuration.GetSection("serverOptions");
            var serverConfig = serverOptionReader(builderContext, config);

            services.Configure<ServerOptions>(serverConfig);
        }

        protected virtual void RegisterDefaultServices(HostBuilderContext builderContext, IServiceCollection servicesInHost, IServiceCollection services)
        {
            // if the package type is StringPackageInfo
            if (typeof(TReceivePackage) == typeof(StringPackageInfo))
            {
                services.TryAdd(ServiceDescriptor.Singleton<IPackageDecoder<StringPackageInfo>, DefaultStringPackageDecoder>());
            }

            services.TryAdd(ServiceDescriptor.Singleton<IPackageEncoder<string>, DefaultStringEncoderForDI>());
            services.TryAdd(ServiceDescriptor.Singleton<ISessionFactory, DefaultSessionFactory>());

            // if no host service was defined, just use the default one
            if (!CheckIfExistHostedService(services))
            {
                RegisterDefaultHostedService(servicesInHost);
            }
        }

        protected virtual bool CheckIfExistHostedService(IServiceCollection services)
        {
            return services.Any(s => s.ServiceType == typeof(IHostedService)
                && typeof(SuperSocketService<TReceivePackage>).IsAssignableFrom(GetImplementationType(s)));
        }

        private static Type? GetImplementationType(ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor.ImplementationType != null)
                return serviceDescriptor.ImplementationType;

            if (serviceDescriptor.ImplementationInstance != null)
                return serviceDescriptor.ImplementationInstance.GetType();

            if (serviceDescriptor.ImplementationFactory != null)
            {
                var typeArguments = serviceDescriptor.ImplementationFactory.GetType().GenericTypeArguments;

                if (typeArguments.Length == 2)
                    return typeArguments[1];
            }

            return null;
        }

        protected virtual void RegisterDefaultHostedService(IServiceCollection servicesInHost)
        {
            RegisterHostedService<SuperSocketService<TReceivePackage>>(servicesInHost);
        }

        protected virtual void RegisterHostedService<THostedService>(IServiceCollection servicesInHost)
            where THostedService : class, IHostedService
        {
            servicesInHost.AddSingleton<THostedService, THostedService>();
            servicesInHost.AddSingleton(s => (INodeInfo)s.GetService<THostedService>()!);
            servicesInHost.AddHostedService(s => s.GetService<THostedService>()!);
        }

        public INodeHostBuilder<TReceivePackage> ConfigureServerOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader)
        {
            _serverOptionsReader = serverOptionsReader;
            return this;
        }

        INodeHostBuilder<TReceivePackage> INodeHostBuilder<TReceivePackage>.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return ConfigureServices(configureDelegate);
        }

        public override SuperSocketHostBuilder<TReceivePackage> ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            ConfigureServicesActions.Add(configureDelegate);
            return this;
        }

        public virtual INodeHostBuilder<TReceivePackage> UsePipelineFilter<TPipelineFilter>()
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            return this.ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<IPipelineFilterFactory<TReceivePackage>, DefaultPipelineFilterFactory<TReceivePackage, TPipelineFilter>>();
            });
        }

        public virtual INodeHostBuilder<TReceivePackage> UsePipelineFilterFactory<TPipelineFilterFactory>()
            where TPipelineFilterFactory : class, IPipelineFilterFactory<TReceivePackage>
        {
            return this.ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<IPipelineFilterFactory<TReceivePackage>, TPipelineFilterFactory>();
            });
        }

        public virtual INodeHostBuilder<TReceivePackage> UseSession<TSession>()
            where TSession : ISession
        {
            return this.UseSessionFactory<GenericSessionFactory<TSession>>();
        }

        public virtual INodeHostBuilder<TReceivePackage> UseSessionFactory<TSessionFactory>()
            where TSessionFactory : class, ISessionFactory
        {
            return this.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<ISessionFactory, TSessionFactory>();
                }
            );
        }

        public virtual INodeHostBuilder<TReceivePackage> UseHostedService<THostedService>()
            where THostedService : class, IHostedService
        {
            if (!typeof(SuperSocketService<TReceivePackage>).IsAssignableFrom(typeof(THostedService)))
            {
                throw new ArgumentException($"The type parameter should be subclass of {nameof(SuperSocketService<TReceivePackage>)}", nameof(THostedService));
            }

            return this.ConfigureServices((ctx, services) =>
            {
                RegisterHostedService<THostedService>(services);
            });
        }


        public virtual INodeHostBuilder<TReceivePackage> UsePackageDecoder<TPackageDecoder>()
            where TPackageDecoder : class, IPackageDecoder<TReceivePackage>
        {
            return this.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IPackageDecoder<TReceivePackage>, TPackageDecoder>();
                }
            );
        }

        public virtual INodeHostBuilder<TReceivePackage> UseMiddleware<TMiddleware>()
            where TMiddleware : class, IMiddleware
        {
            return this.ConfigureServices((ctx, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>());
            });
        }

        public INodeHostBuilder<TReceivePackage> UsePackageHandlingScheduler<TPackageHandlingScheduler>()
            where TPackageHandlingScheduler : class, IPackageHandlingScheduler<TReceivePackage>
        {
            return this.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IPackageHandlingScheduler<TReceivePackage>, TPackageHandlingScheduler>();
                }
            );
        }

        public INodeHostBuilder<TReceivePackage> UsePackageHandlingContextAccessor()
        {
            return this.ConfigureServices(
                 (hostCtx, services) =>
                 {
                     services.AddSingleton<IPackageHandlingContextAccessor<TReceivePackage>, PackageHandlingContextAccessor<TReceivePackage>>();
                 }
             );
        }
    }

    public static class SuperSocketHostBuilder
    {
        public static INodeHostBuilder<TReceivePackage> Create<TReceivePackage>(string[]? args = null)
             where TReceivePackage : class
        {
            return new SuperSocketHostBuilder<TReceivePackage>(args);
        }

        public static INodeHostBuilder<TReceivePackage> Create<TReceivePackage, TPipelineFilter>(string[]? args = null)
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            return new SuperSocketHostBuilder<TReceivePackage>(args)
                .UsePipelineFilter<TPipelineFilter>();
        }
    }
}

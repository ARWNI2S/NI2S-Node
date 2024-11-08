using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Connection;
using ARWNI2S.Engine.Network.Connections;
using ARWNI2S.Engine.Network.Host;
using ARWNI2S.Engine.Network.Middleware;
using ARWNI2S.Engine.Network.Protocol;
using ARWNI2S.Engine.Network.Protocol.Factory;
using ARWNI2S.Engine.Network.Session;
using ARWNI2S.Infrastructure.Network.Protocol;
using ARWNI2S.Runtime.Hosting.Extensions;
using ARWNI2S.Runtime.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Hosting
{
    public class NodeServerHostBuilder<TReceivePackage> : HostBuilderAdapter<NodeServerHostBuilder<TReceivePackage>>, INodeServerHostBuilder<TReceivePackage>, IHostBuilder
    {
        private Func<HostBuilderContext, IConfiguration, IConfiguration> _serverOptionsReader;

        protected List<Action<HostBuilderContext, IServiceCollection>> ConfigureServicesActions { get; private set; } = [];

        protected List<Action<HostBuilderContext, IServiceCollection>> ConfigureSupplementServicesActions = [];

        public NodeServerHostBuilder(IHostBuilder hostBuilder)
            : base(hostBuilder)
        {

        }

        public NodeServerHostBuilder()
            : this(args: null)
        {

        }

        public NodeServerHostBuilder(string[] args)
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

        public INodeServerHostBuilder<TReceivePackage> ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            ConfigureSupplementServicesActions.Add(configureDelegate);
            return this;
        }

        INodeServerHostBuilder INodeServerHostBuilder.ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return ConfigureSupplementServices(configureDelegate);
        }

        protected virtual void RegisterBasicServices(HostBuilderContext builderContext, IServiceCollection servicesInHost, IServiceCollection services)
        {
            var serverOptionReader = _serverOptionsReader;

            if (serverOptionReader == null)
            {
                serverOptionReader = (ctx, config) =>
                {
                    return config;
                };
            }

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
            services.TryAdd(ServiceDescriptor.Singleton<IConnectionListenerFactory, TcpConnectionListenerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(new SocketOptionsSetter(socket => { })));
            services.TryAdd(ServiceDescriptor.Singleton<IConnectionFactoryBuilder, ConnectionFactoryBuilder>());
            services.TryAdd(ServiceDescriptor.Singleton<IConnectionStreamInitializersFactory, DefaultConnectionStreamInitializersFactory>());

            // if no host service was defined, just use the default one
            if (!CheckIfExistHostedService(services))
            {
                RegisterDefaultHostedService(servicesInHost);
            }
        }

        protected virtual bool CheckIfExistHostedService(IServiceCollection services)
        {
            return services.Any(s => s.ServiceType == typeof(IHostedService)
                && typeof(NodeServerService<TReceivePackage>).IsAssignableFrom(GetImplementationType(s)));
        }

        private Type GetImplementationType(ServiceDescriptor serviceDescriptor)
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
            RegisterHostedService<NodeServerService<TReceivePackage>>(servicesInHost);
        }

        protected virtual void RegisterHostedService<THostedService>(IServiceCollection servicesInHost)
            where THostedService : class, IHostedService
        {
            servicesInHost.AddSingleton<THostedService, THostedService>();
            servicesInHost.AddSingleton(s => s.GetService<THostedService>() as IServerInfo);
            servicesInHost.AddHostedService(s => s.GetService<THostedService>());
        }

        public INodeServerHostBuilder<TReceivePackage> ConfigureServerOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader)
        {
            _serverOptionsReader = serverOptionsReader;
            return this;
        }

        INodeServerHostBuilder<TReceivePackage> INodeServerHostBuilder<TReceivePackage>.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return ConfigureServices(configureDelegate);
        }

        public override NodeServerHostBuilder<TReceivePackage> ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            ConfigureServicesActions.Add(configureDelegate);
            return this;
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UsePipelineFilter<TPipelineFilter>()
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            return ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<IPipelineFilterFactory<TReceivePackage>, DefaultPipelineFilterFactory<TReceivePackage, TPipelineFilter>>();
                services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IPipelineFilterFactory<TReceivePackage>>() as IPipelineFilterFactory);
            });
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UsePipelineFilterFactory<TPipelineFilterFactory>()
            where TPipelineFilterFactory : class, IPipelineFilterFactory<TReceivePackage>
        {
            return ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<IPipelineFilterFactory<TReceivePackage>, TPipelineFilterFactory>();
                services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IPipelineFilterFactory<TReceivePackage>>() as IPipelineFilterFactory);
            });
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UseSession<TSession>()
            where TSession : INodeSession
        {
            return UseSessionFactory<GenericSessionFactory<TSession>>();
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UseSessionFactory<TSessionFactory>()
            where TSessionFactory : class, ISessionFactory
        {
            return ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<ISessionFactory, TSessionFactory>();
                }
            );
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UseHostedService<THostedService>()
            where THostedService : class, IHostedService
        {
            if (!typeof(NodeServerService<TReceivePackage>).IsAssignableFrom(typeof(THostedService)))
            {
                throw new ArgumentException($"The type parameter should be subclass of {nameof(NodeServerService<TReceivePackage>)}", nameof(THostedService));
            }

            return ConfigureServices((ctx, services) =>
            {
                RegisterHostedService<THostedService>(services);
            });
        }


        public virtual INodeServerHostBuilder<TReceivePackage> UsePackageDecoder<TPackageDecoder>()
            where TPackageDecoder : class, IPackageDecoder<TReceivePackage>
        {
            return ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IPackageDecoder<TReceivePackage>, TPackageDecoder>();
                }
            );
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UsePackageEncoder<TPackageEncoder>()
            where TPackageEncoder : class, IPackageEncoder<TReceivePackage>
        {
            return ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IPackageEncoder<TReceivePackage>, TPackageEncoder>();
                }
            );
        }

        public virtual INodeServerHostBuilder<TReceivePackage> UseMiddleware<TMiddleware>()
            where TMiddleware : class, IMiddleware
        {
            return ConfigureServices((ctx, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>());
            });
        }

        public INodeServerHostBuilder<TReceivePackage> UsePackageHandlingScheduler<TPackageHandlingScheduler>()
            where TPackageHandlingScheduler : class, IPackageHandlingScheduler<TReceivePackage>
        {
            return ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IPackageHandlingScheduler<TReceivePackage>, TPackageHandlingScheduler>();
                }
            );
        }

        public INodeServerHostBuilder<TReceivePackage> UsePackageHandlingContextAccessor()
        {
            return ConfigureServices(
                 (hostCtx, services) =>
                 {
                     services.AddSingleton<IPackageHandlingContextAccessor<TReceivePackage>, PackageHandlingContextAccessor<TReceivePackage>>();
                 }
             );
        }

        public INodeServerHostBuilder<TReceivePackage> UseGZip()
        {
            return (this as INodeServerHostBuilder).UseGZip() as INodeServerHostBuilder<TReceivePackage>;
        }
    }

    public static class NodeServerHostBuilder
    {
        public static INodeServerHostBuilder<TReceivePackage> Create<TReceivePackage>()
            where TReceivePackage : class
        {
            return Create<TReceivePackage>(args: null);
        }

        public static INodeServerHostBuilder<TReceivePackage> Create<TReceivePackage>(string[] args)
        {
            return new NodeServerHostBuilder<TReceivePackage>(args);
        }

        public static INodeServerHostBuilder<TReceivePackage> Create<TReceivePackage, TPipelineFilter>()
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            return Create<TReceivePackage, TPipelineFilter>(args: null);
        }

        public static INodeServerHostBuilder<TReceivePackage> Create<TReceivePackage, TPipelineFilter>(string[] args)
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            return new NodeServerHostBuilder<TReceivePackage>(args)
                .UsePipelineFilter<TPipelineFilter>();
        }
    }
}

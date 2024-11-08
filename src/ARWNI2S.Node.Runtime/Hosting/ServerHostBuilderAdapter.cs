using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Host;
using ARWNI2S.Runtime.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Hosting
{
    public class ServerHostBuilderAdapter<TReceivePackage> : NodeServerHostBuilder<TReceivePackage>, IServerHostBuilderAdapter
    {
        private IHostBuilder _hostBuilder;

        private IServiceCollection _currentServices = new ServiceCollection();

        private IServiceProvider _serviceProvider;

        private IServiceProvider _hostServiceProvider;

        private Func<HostBuilderContext, IServiceCollection, IServiceProvider> _serviceProviderBuilder = null;

        private List<IConfigureContainerAdapter> _configureContainerActions = [];

        public ServerHostBuilderAdapter(IHostBuilder hostBuilder)
            : base(hostBuilder)
        {
            _hostBuilder = hostBuilder;
        }

        void IServerHostBuilderAdapter.ConfigureServer(HostBuilderContext context, IServiceCollection hostServices)
        {
            ConfigureServer(context, hostServices);
        }

        protected void ConfigureServer(HostBuilderContext context, IServiceCollection hostServices)
        {
            var services = _currentServices;

            CopyGlobalServices(hostServices, services);

            RegisterBasicServices(context, hostServices, services);

            foreach (var configureServicesAction in ConfigureServicesActions)
            {
                configureServicesAction(context, services);
            }

            foreach (var configureServicesAction in ConfigureSupplementServicesActions)
            {
                configureServicesAction(context, services);
            }

            RegisterDefaultServices(context, hostServices, services);

            if (_serviceProviderBuilder == null)
            {
                var serviceFactory = new DefaultServiceProviderFactory();
                var containerBuilder = serviceFactory.CreateBuilder(services);
                ConfigureContainerBuilder(context, containerBuilder);
                _serviceProvider = serviceFactory.CreateServiceProvider(containerBuilder);
            }
            else
            {
                _serviceProvider = _serviceProviderBuilder(context, services);
            }
        }

        private void ConfigureContainerBuilder(HostBuilderContext context, object containerBuilder)
        {
            foreach (IConfigureContainerAdapter containerAction in _configureContainerActions)
                containerAction.ConfigureContainer(context, containerBuilder);
        }

        private void CopyGlobalServices(IServiceCollection hostServices, IServiceCollection services)
        {
            foreach (var sd in hostServices)
            {
                if (sd.ServiceType == typeof(IHostedService))
                    continue;

                CopyGlobalServiceDescriptor(hostServices, services, sd);
            }
        }

        private void CopyGlobalServiceDescriptor(IServiceCollection hostServices, IServiceCollection services, ServiceDescriptor sd)
        {
            if (sd.ImplementationInstance != null)
            {
                services.Add(new ServiceDescriptor(sd.ServiceType, sd.ImplementationInstance));
            }
            else if (sd.ImplementationFactory != null)
            {
                services.Add(new ServiceDescriptor(sd.ServiceType, (sp) => GetServiceFromHost(sd.ImplementationFactory), sd.Lifetime));
            }
            else if (sd.ImplementationType != null)
            {
                if (!sd.ServiceType.IsGenericTypeDefinition && sd.Lifetime == ServiceLifetime.Singleton)
                    services.Add(new ServiceDescriptor(sd.ServiceType, (sp) => _hostServiceProvider.GetService(sd.ServiceType), ServiceLifetime.Singleton));
                else
                    services.Add(sd);
            }
        }

        private object GetServiceFromHost(Func<IServiceProvider, object> factory)
        {
            return factory(_hostServiceProvider);
        }

        void IServerHostBuilderAdapter.ConfigureServiceProvider(IServiceProvider hostServiceProvider)
        {
            _hostServiceProvider = hostServiceProvider;
        }

        protected void RegisterHostedService<THostedService>()
            where THostedService : class, IHostedService
        {
            HostBuilder.ConfigureServices((context, services) =>
            {
                RegisterHostedService<THostedService>(services);
            });
        }

        protected override void RegisterHostedService<THostedService>(IServiceCollection servicesInHost)
        {
            _currentServices.AddSingleton<IHostedService, THostedService>();
            _currentServices.AddSingleton(s => s.GetService<IHostedService>() as IServerInfo);
            servicesInHost.AddHostedService(s => GetHostedService<THostedService>());
        }

        protected override void RegisterDefaultHostedService(IServiceCollection servicesInHost)
        {
            RegisterHostedService<NodeServerService<TReceivePackage>>(servicesInHost);
        }

        private THostedService GetHostedService<THostedService>()
        {
            return (THostedService)_serviceProvider.GetService<IHostedService>();
        }

        public override INodeServerHostBuilder<TReceivePackage> UseHostedService<THostedService>()
        {
            RegisterHostedService<THostedService>();
            return this;
        }

        public override IHost Build()
        {
            throw new NotSupportedException();
        }

        public override NodeServerHostBuilder<TReceivePackage> ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            _configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate));
            return this;
        }

        public override NodeServerHostBuilder<TReceivePackage> UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            _serviceProviderBuilder = (context, services) =>
            {
                var containerBuilder = factory.CreateBuilder(services);
                ConfigureContainerBuilder(context, containerBuilder);
                return factory.CreateServiceProvider(containerBuilder);
            };
            return this;
        }

        public override NodeServerHostBuilder<TReceivePackage> UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            _serviceProviderBuilder = (context, services) =>
            {
                var serviceProviderFactory = factory(context);
                var containerBuilder = serviceProviderFactory.CreateBuilder(services);
                ConfigureContainerBuilder(context, containerBuilder);
                return serviceProviderFactory.CreateServiceProvider(containerBuilder);
            };
            return this;
        }
    }
}
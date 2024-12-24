using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed class BootstrapHostBuilder : IHostBuilder
    {
        private readonly HostApplicationBuilder _builder;

        private readonly List<Action<IConfigurationBuilder>> _configureHostActions = [];
        private readonly List<Action<HostBuilderContext, IConfigurationBuilder>> _configureAppActions = [];
        private readonly List<Action<HostBuilderContext, IServiceCollection>> _configureServicesActions = [];
        private IServiceProviderFactory<object> _serviceProviderFactory;

        public BootstrapHostBuilder(HostApplicationBuilder builder)
        {
            _builder = builder;

            foreach (var descriptor in _builder.Services)
            {
                if (descriptor.ServiceType == typeof(HostBuilderContext))
                {
                    Context = (HostBuilderContext)descriptor.ImplementationInstance!;
                    break;
                }
            }

            if (Context is null)
            {
                throw new InvalidOperationException($"{nameof(HostBuilderContext)} must exist in the {nameof(IServiceCollection)}");
            }
        }

        public IDictionary<object, object> Properties => Context.Properties;

        public HostBuilderContext Context { get; }

        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureHostActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureAppActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHost Build()
        {
            // ConfigureNI2SHostDefaults should never call this.
            throw new InvalidOperationException();
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            // ConfigureNI2SHostDefaults should never call this.
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        {
            ArgumentNullException.ThrowIfNull(factory);

            _serviceProviderFactory = new ServiceProviderFactoryAdapter<TContainerBuilder>(factory);
            return this;
        }

        /// <inheritdoc />
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        {
            return UseServiceProviderFactory(factory(Context));
        }

        public ServiceDescriptor RunDefaultCallbacks()
        {
            foreach (var configureHostAction in _configureHostActions)
            {
                configureHostAction(_builder.Configuration);
            }

            // ConfigureAppConfiguration cannot modify the host configuration because doing so could
            // change the environment, content root and application name which is not allowed at this stage.
            foreach (var configureAppAction in _configureAppActions)
            {
                configureAppAction(Context, _builder.Configuration);
            }

            foreach (var configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(Context, _builder.Services);
            }

            ServiceDescriptor genericNI2SHostServiceDescriptor = null;

            for (int i = _builder.Services.Count - 1; i >= 0; i--)
            {
                var descriptor = _builder.Services[i];
                if (descriptor.ServiceType == typeof(IHostedService))
                {
                    Debug.Assert(descriptor.ImplementationType?.Name == "NI2SHostService");

                    genericNI2SHostServiceDescriptor = descriptor;
                    _builder.Services.RemoveAt(i);
                    break;
                }
            }

            return genericNI2SHostServiceDescriptor ?? throw new InvalidOperationException($"NI2SHostService must exist in the {nameof(IServiceCollection)}");
        }

        private sealed class ServiceProviderFactoryAdapter<TContainerBuilder> : IServiceProviderFactory<object> where TContainerBuilder : notnull
        {
            private readonly IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;

            public ServiceProviderFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
            {
                _serviceProviderFactory = serviceProviderFactory;
            }

            public object CreateBuilder(IServiceCollection services) => _serviceProviderFactory.CreateBuilder(services);
            public IServiceProvider CreateServiceProvider(object containerBuilder) => _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}

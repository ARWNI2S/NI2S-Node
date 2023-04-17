﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NI2S.Node.Hosting
{
    // This exists solely to bootstrap the configuration
    internal sealed class BootstrapHostBuilder : IHostBuilder
    {
        private readonly HostApplicationBuilder _builder;

        private readonly List<Action<IConfigurationBuilder>> _configureHostActions = new();
        private readonly List<Action<HostBuilderContext, IConfigurationBuilder>> _configureNodeActions = new();
        private readonly List<Action<HostBuilderContext, IServiceCollection>> _configureServicesActions = new();

        /* 001.1 - new NodeEngineHostBuilder(...) -> new BootstrapHostBuilder(...) */
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

        /* 001.2.1.2.1 */
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureHostActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /* 001.2.1.2.2 */
        /* 001.2.1.3.1.1.1 */
        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureNodeActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /* 001.2.1.2.3 */
        /* 001.2.1.3.1.2.1.1.2.1.1.1 */
        /* 001.2.1.3.1.2.1.2.1.1 */
        /* 001.2.1.3.1.2.2.1.1 */
        /* 001.2.1.3.2.1.1.2 */
        /* 001.2.1.4 */
        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHost Build()
        {
            // ConfigureNodeHostDefaults should never call this.
            throw new InvalidOperationException();
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            // ConfigureNodeHostDefaults should never call this.
            throw new InvalidOperationException();
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        {
            // ConfigureNodeHostDefaults should never call this.
            throw new InvalidOperationException();
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        {
            // ConfigureNodeHostDefaults should never call this.
            throw new InvalidOperationException();
        }

        /* 001.3 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() */
        public ServiceDescriptor RunDefaultCallbacks()
        {
            foreach (var configureHostAction in _configureHostActions)
            {
                configureHostAction(_builder.Configuration);
            }

            // ConfigureAppConfiguration cannot modify the host configuration because doing so could
            // change the environment, content root and application name which is not allowed at this stage.
            foreach (var configureAppAction in _configureNodeActions)
            {
                configureAppAction(Context, _builder.Configuration);
            }

            foreach (var configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(Context, _builder.Services);
            }

            ServiceDescriptor genericNodeHostServiceDescriptor = null;

            for (int i = _builder.Services.Count - 1; i >= 0; i--)
            {
                var descriptor = _builder.Services[i];
                if (descriptor.ServiceType == typeof(IHostedService))
                {
                    Debug.Assert(descriptor.ImplementationType?.Name == "NodeHostService");

                    genericNodeHostServiceDescriptor = descriptor;
                    _builder.Services.RemoveAt(i);
                    break;
                }
            }

            return genericNodeHostServiceDescriptor ?? throw new InvalidOperationException($"GenericNodeHostedService must exist in the {nameof(IServiceCollection)}");
        }
    }
}
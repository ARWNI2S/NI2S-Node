﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Hosting.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A non-buildable <see cref="IHostBuilder"/> for <see cref="NodeEngineHostBuilder"/>.
    /// Use <see cref="NodeEngineHostBuilder.Build"/> to build the <see cref="NodeEngineHostBuilder"/>.
    /// </summary>
    public sealed class ConfigureHostBuilder : IHostBuilder, ISupportsConfigureNodeHost
    {
        private readonly ConfigurationManager _configuration;
        private readonly IServiceCollection _services;
        private readonly HostBuilderContext _context;

        private readonly List<Action<HostBuilderContext, object>> _configureContainerActions = new();
        private IServiceProviderFactory<object> _serviceProviderFactory;

        internal ConfigureHostBuilder(
            HostBuilderContext context,
            ConfigurationManager configuration,
            IServiceCollection services)
        {
            _configuration = configuration;
            _services = services;
            _context = context;
        }

        public IDictionary<string, object> Properties => _context.Properties.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);

        /// <inheritdoc />
        IDictionary<object, object> IHostBuilder.Properties => _context.Properties;

        IHost IHostBuilder.Build()
        {
            throw new NotSupportedException($"Call {nameof(NodeEngineHostBuilder)}.{nameof(NodeEngineHostBuilder.Build)}() instead.");
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);
            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            if (configureDelegate is null)
            {
                throw new ArgumentNullException(nameof(configureDelegate));
            }

            _configureContainerActions.Add((context, containerBuilder) => configureDelegate(context, (TContainerBuilder)containerBuilder));

            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            var previousApplicationName = _configuration[HostDefaults.ApplicationKey];
            // Use the real content root so we can compare paths
            var previousContentRoot = PathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[HostDefaults.ContentRootKey];
            var previousEnvironment = _configuration[HostDefaults.EnvironmentKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_configuration);

            // Disallow changing any host settings this late in the cycle, the reasoning is that we've already loaded the default configuration
            // and done other things based on environment name, engine name or content root.
            if (!string.Equals(previousApplicationName, _configuration[HostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The engine name changed from \"{previousApplicationName}\" to \"{_configuration[HostDefaults.ApplicationKey]}\". Changing the host configuration using EngineBuilder.Host is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }

            if (!string.Equals(previousContentRootConfig, _configuration[HostDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, PathResolver.ResolvePath(_configuration[HostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{PathResolver.ResolvePath(_configuration[HostDefaults.ContentRootKey])}\". Changing the host configuration using EngineBuilder.Host is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }

            if (!string.Equals(previousEnvironment, _configuration[HostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[HostDefaults.EnvironmentKey]}\". Changing the host configuration using EngineBuilder.Host is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }

            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _services);
            return this;
        }

        /// <inheritdoc />
        /* 002.2 - ConfigureNodeEngineBuilder(...) -> builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()) */
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _serviceProviderFactory = new ServiceProviderFactoryAdapter<TContainerBuilder>(factory);
            return this;
        }

        /// <inheritdoc />
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        {
            return UseServiceProviderFactory(factory(_context));
        }

        IHostBuilder ISupportsConfigureNodeHost.ConfigureNodeHost(Action<INodeHostBuilder> configure, Action<NodeHostBuilderOptions> configureOptions)
        {
            throw new NotSupportedException("ConfigureNodeHost() is not supported by EngineBuilder.Host. Use the NodeEngine returned by EngineBuilder.Build() instead.");
        }

        /* 003.1 - ... -> .Build() -> Host.ApplyServiceProviderFactory(...) */
        internal void ApplyServiceProviderFactory(HostApplicationBuilder hostEngineBuilder)
        {
            if (_serviceProviderFactory is null)
            {
                // No custom factory. Avoid calling hostEngineBuilder.ConfigureContainer() which might override default validation options.
                // If there were any callbacks supplied to ConfigureHostBuilder.ConfigureContainer(), call those with the IServiceCollection.
                foreach (var action in _configureContainerActions)
                {
                    action(_context, _services);
                }

                return;
            }

            void ConfigureContainerBuilderAdapter(object containerBuilder)
            {
                /* 003.2.1 - ... -> .Build() -> Host.ApplyServiceProviderFactory(...) -> new NodeEngineHost(_hostApplicationBuilder.Build()) 
                             -> _serviceProviderFactory.CreateBuilder(services) */
                foreach (var action in _configureContainerActions)
                {
                    action(_context, containerBuilder);
                }
            }

            hostEngineBuilder.ConfigureContainer(_serviceProviderFactory, ConfigureContainerBuilderAdapter);
        }

        private sealed class ServiceProviderFactoryAdapter<TContainerBuilder> : IServiceProviderFactory<object> where TContainerBuilder : notnull
        {
            private readonly IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;

            public ServiceProviderFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
            {
                _serviceProviderFactory = serviceProviderFactory;
            }

            /* 003.2 - ... -> .Build() -> Host.ApplyServiceProviderFactory(...) -> new NodeEngineHost(_hostApplicationBuilder.Build()) */
            public object CreateBuilder(IServiceCollection services) => _serviceProviderFactory.CreateBuilder(services);
            /* 003.3 - ... -> .Build() -> Host.ApplyServiceProviderFactory(...) -> new NodeEngineHost(_hostApplicationBuilder.Build()) */
            public IServiceProvider CreateServiceProvider(object containerBuilder) => _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}
﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration;
using NI2S.Node.Hosting.Infrastructure;
using NI2S.Node.Infrastructure;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A non-buildable <see cref="IHostBuilder"/> for <see cref="NI2SNodeBuilder"/>.
    /// Use <see cref="NI2SNodeBuilder.Build"/> to build the <see cref="NI2SNodeBuilder"/>.
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

        /// <inheritdoc />
        public IDictionary<object, object> Properties => _context.Properties;

        IHost IHostBuilder.Build()
        {
            throw new NotSupportedException($"Call {nameof(NI2SNodeBuilder)}.{nameof(NI2SNodeBuilder.Build)}() instead.");
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
            ArgumentNullException.ThrowIfNull(configureDelegate);

            _configureContainerActions.Add((context, containerBuilder) => configureDelegate(context, (TContainerBuilder)containerBuilder));

            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            var previousApplicationName = _configuration[HostDefaults.ApplicationKey];
            // Use the real content root so we can compare paths
            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[HostDefaults.ContentRootKey];
            var previousEnvironment = _configuration[HostDefaults.EnvironmentKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_configuration);

            // Disallow changing any host settings this late in the cycle, the reasoning is that we've already loaded the default configuration
            // and done other things based on environment name, application name or content root.
            if (!string.Equals(previousApplicationName, _configuration[HostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The application name changed from \"{previousApplicationName}\" to \"{_configuration[HostDefaults.ApplicationKey]}\". Changing the host configuration using NI2SNodeBuilder.Host is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }

            if (!string.Equals(previousContentRootConfig, _configuration[HostDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(_configuration[HostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(_configuration[HostDefaults.ContentRootKey])}\". Changing the host configuration using NI2SNodeBuilder.Host is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }

            if (!string.Equals(previousEnvironment, _configuration[HostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[HostDefaults.EnvironmentKey]}\". Changing the host configuration using NI2SNodeBuilder.Host is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
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
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        {
            ArgumentNullException.ThrowIfNull(factory);

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
            throw new NotSupportedException("ConfigureNodeHost() is not supported by NI2SNodeBuilder.Host. Use the NI2SNode returned by NI2SNodeBuilder.Build() instead.");
        }

        internal void ApplyServiceProviderFactory(HostApplicationBuilder hostApplicationBuilder)
        {
            if (_serviceProviderFactory is null)
            {
                // No custom factory. Avoid calling hostApplicationBuilder.ConfigureContainer() which might override default validation options.
                // If there were any callbacks supplied to ConfigureHostBuilder.ConfigureContainer(), call those with the IServiceCollection.
                foreach (var action in _configureContainerActions)
                {
                    action(_context, _services);
                }

                return;
            }

            void ConfigureContainerBuilderAdapter(object containerBuilder)
            {
                foreach (var action in _configureContainerActions)
                {
                    action(_context, containerBuilder);
                }
            }

            hostApplicationBuilder.ConfigureContainer(_serviceProviderFactory, ConfigureContainerBuilderAdapter);
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

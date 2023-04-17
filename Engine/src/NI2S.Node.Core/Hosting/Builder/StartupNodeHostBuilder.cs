﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Infrastructure;
using NI2S.Node.Hosting.Internal;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Hosting.Builder
{
    // We use this type to capture calls to the INodeHostBuilder so the we can properly order calls to
    // to GenericHostNodeHostBuilder.
    internal sealed class StartupNodeHostBuilder : INodeHostBuilder, ISupportsStartup, ISupportsUseDefaultServiceProvider
    {
        private readonly GenericNodeHostBuilder _builder;
        private Action<NodeHostBuilderContext, IConfigurationBuilder> _configureConfiguration;
        private Action<NodeHostBuilderContext, IServiceCollection> _configureServices;

        /* 001.3.1.1.2 */
        public StartupNodeHostBuilder(GenericNodeHostBuilder builder)
        {
            _builder = builder;
        }

        public INodeHost Build()
        {
            throw new NotSupportedException($"Building this implementation of {nameof(INodeHostBuilder)} is not supported.");
        }

        public INodeHostBuilder ConfigureNodeConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureConfiguration += configureDelegate;
            return this;
        }

        public INodeHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        public INodeHostBuilder ConfigureServices(Action<NodeHostBuilderContext, IServiceCollection> configureServices)
        {
            _configureServices += configureServices;
            return this;
        }

        public string GetSetting(string key) => _builder.GetSetting(key);

        public INodeHostBuilder UseSetting(string key, string value)
        {
            _builder.UseSetting(key, value);
            return this;
        }

        /* 001.3.4.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() -> configureServicesAction(...)
                       -> _hostingStartupNodeHostBuilder?.ConfigureServices(nodehostContext, services) */
        public void ConfigureServices(NodeHostBuilderContext context, IServiceCollection services)
        {
            _configureServices?.Invoke(context, services);
        }

        /* 001.3.2.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() -> configureAppAction(Context, _builder.Configuration) 
                      -> GetNodeHostBuilderContext(context) -> _hostingStartupNodeHostBuilder.ConfigureAppConfiguration(...) */
        public void ConfigureAppConfiguration(NodeHostBuilderContext context, IConfigurationBuilder builder)
        {
            _configureConfiguration?.Invoke(context, builder);
        }

        public INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure)
        {
            return _builder.UseDefaultServiceProvider(configure);
        }

        public INodeHostBuilder Configure(Action<INodeEngineBuilder> configure)
        {
            return _builder.Configure(configure);
        }

        public INodeHostBuilder Configure(Action<NodeHostBuilderContext, INodeEngineBuilder> configure)
        {
            return _builder.Configure(configure);
        }

        public INodeHostBuilder UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            return _builder.UseStartup(startupType);
        }

        // Note: This method isn't 100% compatible with trimming. It is possible for the factory to return a derived type from TStartup.
        // RequiresUnreferencedCode isn't on this method because the majority of people won't do that.
        public INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory)
        {
            return _builder.UseStartup(startupFactory);
        }
    }
}
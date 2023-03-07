using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Server
{
    public abstract class HostBuilderAdapter<THostBuilder> : IHostBuilder
        where THostBuilder : HostBuilderAdapter<THostBuilder>
    {
        protected IHostBuilder HostBuilder { get; private set; }

        public HostBuilderAdapter()
            : this(args: null)
        {

        }

        public HostBuilderAdapter(string[]? args)
            : this(Host.CreateDefaultBuilder(args))
        {

        }

        public HostBuilderAdapter(IHostBuilder hostBuilder)
        {
            HostBuilder = hostBuilder;
        }

        public IDictionary<object, object> Properties => HostBuilder.Properties;

        public virtual IHost Build()
        {
            return HostBuilder.Build();
        }

        IHostBuilder IHostBuilder.ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return ConfigureAppConfiguration(configureDelegate);
        }

        public virtual THostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            HostBuilder.ConfigureAppConfiguration(configureDelegate);
            return (THostBuilder)this;
        }

        IHostBuilder IHostBuilder.ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            return ConfigureContainer(configureDelegate);
        }

        public virtual THostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            HostBuilder.ConfigureContainer(configureDelegate);
            return (THostBuilder)this;
        }

        IHostBuilder IHostBuilder.ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            return ConfigureHostConfiguration(configureDelegate);
        }

        public THostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            HostBuilder.ConfigureHostConfiguration(configureDelegate);
            return (THostBuilder)this;
        }

        IHostBuilder IHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return ConfigureServices(configureDelegate);
        }

        public virtual THostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            HostBuilder.ConfigureServices(configureDelegate);
            return (THostBuilder)this;
        }

        IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            return UseServiceProviderFactory(factory);
        }

        public virtual THostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        {
            HostBuilder.UseServiceProviderFactory(factory);
            return (THostBuilder)this;
        }

        IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            return UseServiceProviderFactory(factory);
        }

        public virtual THostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        {
            HostBuilder.UseServiceProviderFactory(factory);
            return (THostBuilder)this;
        }
    }
}
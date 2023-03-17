using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Hosting
{

    /// <summary>
    /// A builder for web applications and services.
    /// </summary>
    public sealed class NI2SNodeHostBuilder : HostBuilderAdapter<NI2SNodeHostBuilder>, INodeHostBuilder, IHostBuilder
    {
        #region static class

        public static NI2SNodeHostBuilder CreateDefaultBuilder(string[]? args = null)
        {
            return new NI2SNodeHostBuilder(args);
        }

        #endregion

        //private Func<HostBuilderContext, IConfiguration, IConfiguration> _optionsReader;

        // TODO: SE USA??
        private readonly List<Action<HostBuilderContext, IServiceCollection>> _configureServicesActions = new();

        // TODO: SE USA??
        private readonly List<Action<HostBuilderContext, IServiceCollection>> _configureSupplementServicesActions = new();

        #region Constructor

        /// <inheritdoc/>
        public NI2SNodeHostBuilder(IHostBuilder hostBuilder)
            : base(hostBuilder)
        {

        }

        /// <inheritdoc/>
        public NI2SNodeHostBuilder(string[]? args = null)
            : base(args)
        {

        }

        #endregion

        /// <inheritdoc/>
        public override IHost Build()
        {
            ConfigureHostBuilder();
            return base.Build();
        }

        #region IMinimalApiHostBuilder Implementation

        private void ConfigureHostBuilder()
        {
            ConfigureServices((ctx, services) =>
            {
                //RegisterBasicServices(ctx, services, services);
            }).ConfigureServices((ctx, services) =>
            {
                foreach (var action in _configureServicesActions)
                {
                    action(ctx, services);
                }

                foreach (var action in _configureSupplementServicesActions)
                {
                    action(ctx, services);
                }
            }).ConfigureServices((ctx, services) =>
            {
                //RegisterDefaultServices(ctx, services, services);
            });
        }

        /// <inheritdoc/>
        void IMinimalApiHostBuilder.ConfigureHostBuilder() => ConfigureHostBuilder();

        #endregion


        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IServiceCollection"/> that will be used
        /// to construct the <see cref="IServiceProvider"/>.</param>
        /// <returns>The same instance of the <see cref="THostBuilder"/> for chaining.</returns>
        public override NI2SNodeHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return base.ConfigureServices(configureDelegate);
        }

        public NI2SNodeHostBuilder UseSomeService<TSomeService>()
            where TSomeService : class
        {
            return ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<TSomeService>();
            });
        }





        INodeHostBuilder INodeHostBuilder.ConfigureServerOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        INodeHostBuilder INodeHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
            => ConfigureServices(configureDelegate);

        INodeHostBuilder INodeHostBuilder.UseHostedService<THostedService>()
        {
            throw new NotImplementedException();
        }

        INodeHostBuilder INodeHostBuilder.UseSomeService<TSomeService>()
                   where TSomeService : class
     => UseSomeService<TSomeService>();
    }
}

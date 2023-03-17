using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// The NI2S Node host used to configure the node clustering services.
    /// </summary>
    public sealed class NI2SNodeHost //: IHost, INodeHostBuilder/*, IClusterNodeBuilder*/, IAsyncDisposable
    {
        //    private readonly IHost _host;

        //    #region Static Class

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="NI2SNodeHost"/> class with preconfigured defaults.
        //    /// </summary>
        //    /// <param name="args">Command line arguments</param>
        //    /// <returns>The <see cref="NI2SNodeHost"/>.</returns>
        //    public static NI2SNodeHost Create(string[] args = null) =>
        //        new NI2SNodeHostBuilder(new() { Args = args }).Build();

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="NI2SNodeHostBuilder"/> class with preconfigured defaults.
        //    /// </summary>
        //    /// <returns>The <see cref="NI2SNodeHostBuilder"/>.</returns>
        //    public static NI2SNodeHostBuilder CreateBuilder() =>
        //        new(new());

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="NI2SNodeHostBuilder"/> class with preconfigured defaults.
        //    /// </summary>
        //    /// <param name="args">Command line arguments</param>
        //    /// <returns>The <see cref="NI2SNodeHostBuilder"/>.</returns>
        //    public static NI2SNodeHostBuilder CreateBuilder(string[] args) =>
        //        new(new() { Args = args });

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="NI2SNodeHostBuilder"/> class with preconfigured defaults.
        //    /// </summary>
        //    /// <param name="options">The <see cref="NI2SNodeHostOptions"/> to configure the <see cref="NI2SNodeHostBuilder"/>.</param>
        //    /// <returns>The <see cref="NI2SNodeHostBuilder"/>.</returns>
        //    public static NI2SNodeHostBuilder CreateBuilder(NI2SNodeHostOptions options) =>
        //        new(options);

        //    #endregion

        //    public NI2SNodeHost()
        //        : this(args: null)
        //    {

        //    }

        //    public NI2SNodeHost(string[] args)
        //    {

        //    }

        //    #region Private members

        //    private IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private IHost Build()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    #endregion

        //    #region IHost Implementation

        //    /// <summary>
        //    /// The NI2S node's configured services.
        //    /// </summary>
        //    public IServiceProvider Services => _host.Services;

        //    IDictionary<object, object> IHostBuilder.Properties => throw new NotImplementedException();

        //    public Task StartAsync(CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Task StopAsync(CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    #endregion

        //    #region IMinimalApiHostBuilder Implementation

        //    public void ConfigureHostBuilder()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    #endregion

        //    #region INodeHostBuilder Implementation

        //    public INodeHostBuilder ConfigureServerOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    INodeHostBuilder INodeHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UseMiddleware<TMiddleware>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UsePipelineFilter<TPipelineFilter>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UsePipelineFilterFactory<TPipelineFilterFactory>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UseHostedService<THostedService>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UsePackageDecoder<TPackageDecoder>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UsePackageHandlingScheduler<TPackageHandlingScheduler>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UseSessionFactory<TSessionFactory>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UseSession<TSession>()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public INodeHostBuilder UsePackageHandlingContextAccessor()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    #endregion

        //    #region IHostBuilder Implementation

        //    /// <inheritdoc/>
        //    IHostBuilder IHostBuilder.ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        //        => ConfigureHostConfiguration(configureDelegate);

        //    /// <inheritdoc/>
        //    IHostBuilder IHostBuilder.ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        //        => ConfigureAppConfiguration(configureDelegate);

        //    /// <inheritdoc/>
        //    IHostBuilder IHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        //        => ConfigureServices(configureDelegate);

        //    /// <inheritdoc/>
        //    IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        //        => UseServiceProviderFactory(factory);

        //    /// <inheritdoc/>
        //    IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        //        => UseServiceProviderFactory(factory);

        //    /// <inheritdoc/>
        //    IHostBuilder IHostBuilder.ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        //        => ConfigureContainer(configureDelegate);

        //    /// <inheritdoc/>
        //    IHost IHostBuilder.Build()
        //        => Build();

        //    #endregion

        //    #region IDisposable Implementation

        //    /// <summary>
        //    /// Disposes the node host.
        //    /// </summary>
        //    void IDisposable.Dispose() => _host.Dispose();

        //    #endregion

        //    #region IAsyncDisposable Implementation

        //    /// <summary>
        //    /// Disposes the node host.
        //    /// </summary>
        //    public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        //    #endregion

    }
}

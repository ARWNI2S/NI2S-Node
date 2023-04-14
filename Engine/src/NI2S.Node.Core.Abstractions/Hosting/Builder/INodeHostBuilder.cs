// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A builder for <see cref="INodeHost"/>.
    /// </summary>
    public interface INodeHostBuilder
    {
        /// <summary>
        /// Builds an <see cref="INodeHost"/> which hosts a web application.
        /// </summary>
        INodeHost Build();

        /// <summary>
        /// Adds a delegate for configuring the <see cref="IConfigurationBuilder"/> that will construct an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder" /> that will be used to construct an <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        /// <remarks>
        /// The <see cref="IConfiguration"/> and <see cref="ILoggerFactory"/> on the <see cref="NodeHostBuilderContext"/> are uninitialized at this stage.
        /// The <see cref="IConfigurationBuilder"/> is pre-populated with the settings of the <see cref="INodeHostBuilder"/>.
        /// </remarks>
        INodeHostBuilder ConfigureNodeConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate);

        /// <summary>
        /// Adds a delegate for configuring additional services for the host or web application. This may be called
        /// multiple times.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// Adds a delegate for configuring additional services for the host or web application. This may be called
        /// multiple times.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder ConfigureServices(Action<NodeHostBuilderContext, IServiceCollection> configureServices);

        /// <summary>
        /// Get the setting value from the configuration.
        /// </summary>
        /// <param name="key">The key of the setting to look up.</param>
        /// <returns>The value the setting currently contains.</returns>
        string GetSetting(string key);

        /// <summary>
        /// Add or replace a setting in the configuration.
        /// </summary>
        /// <param name="key">The key of the setting to add or replace.</param>
        /// <param name="value">The value of the setting to add or replace.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder UseSetting(string key, string value);
    }
}

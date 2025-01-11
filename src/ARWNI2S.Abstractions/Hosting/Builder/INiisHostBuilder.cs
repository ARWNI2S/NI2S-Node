// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Hosting.Builder
{
    /// <summary>
    /// A builder for <see cref="INiisHost"/>.
    /// </summary>
    public interface INiisHostBuilder
    {
        /// <summary>
        /// Builds an <see cref="INiisHost"/> which hosts a NI2S node engine.
        /// </summary>
        INiisHost Build();

        /// <summary>
        /// Adds a delegate for configuring the <see cref="IConfigurationBuilder"/> that will construct an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder" /> that will be used to construct an <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="INiisHostBuilder"/>.</returns>
        /// <remarks>
        /// The <see cref="IConfiguration"/> and <see cref="ILoggerFactory"/> on the <see cref="NI2SHostBuilderContext"/> are uninitialized at this stage.
        /// The <see cref="IConfigurationBuilder"/> is pre-populated with the settings of the <see cref="INiisHostBuilder"/>.
        /// </remarks>
        INiisHostBuilder ConfigureNI2SConfiguration(Action<NI2SHostBuilderContext, IConfigurationBuilder> configureDelegate);

        /// <summary>
        /// Adds a delegate for configuring additional services for the host or web application. This may be called
        /// multiple times.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="INiisHostBuilder"/>.</returns>
        INiisHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// Adds a delegate for configuring additional services for the host or web application. This may be called
        /// multiple times.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="INiisHostBuilder"/>.</returns>
        INiisHostBuilder ConfigureServices(Action<NI2SHostBuilderContext, IServiceCollection> configureServices);

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
        /// <returns>The <see cref="INiisHostBuilder"/>.</returns>
        INiisHostBuilder UseSetting(string key, string value);
    }
}
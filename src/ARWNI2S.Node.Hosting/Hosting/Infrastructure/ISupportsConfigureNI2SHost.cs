using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Infrastructure
{
    /// <summary>
    /// An interface implemented by INI2SHostBuilders that handle <see cref="Extensions.GenericHostNI2SHostBuilderExtensions.ConfigureNI2SHost(IHostBuilder, Action{INI2SHostBuilder})"/>
    /// directly.
    /// </summary>
    public interface ISupportsConfigureNI2SHost
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core web application.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="INI2SHostBuilder"/>.</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="NI2SHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        IHostBuilder ConfigureNI2SHost(Action<INI2SHostBuilder> configure, Action<NI2SHostBuilderOptions> configureOptions);
    }
}

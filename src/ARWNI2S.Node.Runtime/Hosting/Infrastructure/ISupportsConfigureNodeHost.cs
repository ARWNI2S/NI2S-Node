using ARWNI2S.Runtime.Builder;
using ARWNI2S.Runtime.Configuration.Options;
using ARWNI2S.Runtime.Hosting.Extensions;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Hosting.Infrastructure
{
    /// <summary>
    /// An interface implemented by INodeHostBuilders that handle <see cref="GenericHostNodeHostBuilderExtensions.ConfigureNodeHost(IHostBuilder, Action{INodeHostBuilder})"/>
    /// directly.
    /// </summary>
    public interface ISupportsConfigureNodeHost
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core node application.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="INodeHostBuilder"/>.</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="NodeHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        IHostBuilder ConfigureNodeHost(Action<INodeHostBuilder> configure, Action<NodeHostBuilderOptions> configureOptions);
    }
}
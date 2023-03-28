using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration;
using System;

namespace NI2S.Node.Hosting.Infrastructure
{
    /// <summary>
    /// An interface implemented by INodeHostBuilders that handle <see cref="GenericHostNodeHostBuilderExtensions.ConfigureNodeHost(IHostBuilder, Action{INodeHostBuilder})"/>
    /// directly.
    /// </summary>
    public interface ISupportsConfigureNodeHost
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core web node.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="INodeHostBuilder"/>.</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="NodeHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        IHostBuilder ConfigureNodeHost(Action<INodeHostBuilder> configure, Action<NodeHostBuilderOptions> configureOptions);
    }
}

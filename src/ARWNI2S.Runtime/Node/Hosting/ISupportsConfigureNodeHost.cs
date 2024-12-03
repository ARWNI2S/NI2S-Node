﻿using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting.Configuration;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting
{
    public interface ISupportsConfigureNodeHost
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core node enginelication.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="INodeHostBuilder"/>.</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="NodeHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        IHostBuilder ConfigureNodeHost(Action<INodeHostBuilder> configure, Action<NodeHostBuilderOptions> configureOptions);
    }
}
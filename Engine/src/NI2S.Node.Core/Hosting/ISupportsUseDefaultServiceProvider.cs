using Microsoft.Extensions.DependencyInjection;
using System;

namespace NI2S.Node.Hosting
{
    internal interface ISupportsUseDefaultServiceProvider
    {
        IWebHostBuilder UseDefaultServiceProvider(Action<WebHostBuilderContext, ServiceProviderOptions> configure);
    }
}
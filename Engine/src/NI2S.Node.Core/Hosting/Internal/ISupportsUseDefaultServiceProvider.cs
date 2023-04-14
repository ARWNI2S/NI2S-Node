// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Hosting.Internal
{
    internal interface ISupportsUseDefaultServiceProvider
    {
        INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure);
    }
}
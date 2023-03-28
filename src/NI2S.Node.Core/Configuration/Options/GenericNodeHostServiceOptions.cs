using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Configuration
{
    internal sealed class GenericNodeHostServiceOptions
    {
        public Action<INodeBuilder> ConfigureApplication { get; set; }

        public NodeHostOptions NodeHostOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException HostingStartupExceptions { get; set; }
    }
}

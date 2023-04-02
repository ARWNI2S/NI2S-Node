using NI2S.Node.Builder;
using System;

namespace NI2S.Node.Hosting
{
    internal sealed class GenericNodeHostServiceOptions
    {
        public Action<IEngineBuilder> ConfigureEngine { get; set; }

        public NodeHostOptions NodeHostOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException HostingStartupExceptions { get; set; }
    }
}
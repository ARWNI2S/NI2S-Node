// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Hosting.Builder;
using NI2S.Node.Hosting.Internal;
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
// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Options;
using NI2S.Node.Core;
using System;

namespace NI2S.Node.Engine
{
    internal sealed class NodeEngineOptionsSetup : IConfigureOptions<NodeEngineOptions>
    {
        private readonly IServiceProvider _services;

        public NodeEngineOptionsSetup(IServiceProvider services)
        {
            _services = services;
        }

        public void Configure(NodeEngineOptions options)
        {
            options.EngineServices = _services;
        }
    }
}

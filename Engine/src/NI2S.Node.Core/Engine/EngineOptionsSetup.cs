// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Options;
using NI2S.Node.Core;
using System;

namespace NI2S.Node.Engine
{
    internal sealed class EngineOptionsSetup : IConfigureOptions<EngineOptions>
    {
        private readonly IServiceProvider _services;

        public EngineOptionsSetup(IServiceProvider services)
        {
            _services = services;
        }

        public void Configure(EngineOptions options)
        {
            options.EngineServices = _services;
        }
    }
}

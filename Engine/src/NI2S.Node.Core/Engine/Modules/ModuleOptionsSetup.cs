// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.Options;
using System;

namespace NI2S.Node.Engine
{
    public sealed class ModuleOptionsSetup : IConfigureOptions<ModuleOptions>
    {
        private readonly IServiceProvider _services;

        public ModuleOptionsSetup(IServiceProvider services)
        {
            _services = services;
        }

        public void Configure(ModuleOptions options)
        {
            options.EngineServices = _services;
        }
    }
}

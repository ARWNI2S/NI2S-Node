using Microsoft.Extensions.Options;
using NI2S.Node.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node.Engine
{
    public sealed class EngineModulesOptionsSetup : IConfigureOptions<EngineModulesOptions>
    {
        private readonly IServiceProvider _services;

        public EngineModulesOptionsSetup(IServiceProvider services)
        {
            _services = services;
        }

        public void Configure(EngineModulesOptions options)
        {
            options.EngineServices = _services;
        }
    }
}

// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Engine;
using NI2S.Node.Builder;

namespace NI2S.Node.Core
{
    public sealed class NetworkModule : ModuleInfo, IEngineModule
    {
        public NetworkModule() : base(name: "Network", systemName: "CORE_NETWORK") { }

        public void Configure(IEngineBuilder engine)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }

        ModuleInfo IEngineModule.ModuleInfo => this;

    }
}

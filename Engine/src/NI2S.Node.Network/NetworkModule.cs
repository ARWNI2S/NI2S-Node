// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node.Core
{
    public sealed class NetworkModule : ModuleInfo, IEngineModule
    {
        public NetworkModule() : base(name: "Network", systemName: "CORE_NETWORK") { }

        public void Configure(INodeEngineBuilder engine)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }

        ModuleInfo IEngineModule.ModuleInfo => this;

    }
}

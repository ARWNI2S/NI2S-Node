// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node.Core
{
    public sealed class NetworkModule : EngineModule
    {
        public NetworkModule() : base(new ModuleDescriptor() { Author = "", }) { }

        public override void ConfigureModule(IEngineBuilder engine)
        {

        }

        public override void ConfigureModuleServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}

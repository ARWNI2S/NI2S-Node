using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node.Engine
{
    public abstract class EngineModule : IEngineModule
    {
        public ModuleDescriptor ModuleDescriptor { get; set; }
        ModuleDescriptorBase IEngineModule.ModuleDescriptor { get => ModuleDescriptor; set => ModuleDescriptor = (ModuleDescriptor)value; }

        public abstract void Configure(INodeEngineBuilder engine);

        public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}

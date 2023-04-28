// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node.Engine
{
    public abstract class EngineModule : IEngineModule
    {
        public ModuleDescriptor ModuleDescriptor { get; protected set; }

        ModuleDescriptorBase IEngineModule.ModuleDescriptor { get => ModuleDescriptor; set => ModuleDescriptor = (ModuleDescriptor)value; }

        protected EngineModule(ModuleDescriptor descriptor)
        {
            ModuleDescriptor = descriptor;
        }

        public abstract void ConfigureModule(IEngineBuilder engine);

        public abstract void ConfigureModuleServices(IServiceCollection services, IConfiguration configuration);
    }
}

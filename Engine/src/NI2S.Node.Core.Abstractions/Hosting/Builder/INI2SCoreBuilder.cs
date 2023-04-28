// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;

namespace NI2S.Node.Hosting.Builder
{
    public interface INI2SCoreBuilder
    {
        public IModuleManager ModuleManager { get; }

        public IServiceCollection Services { get; }
    }
}

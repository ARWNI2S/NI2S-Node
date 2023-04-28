// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;

namespace NI2S.Node.Hosting.Builder
{
    public interface INI2SBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where MVC services are configured.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Gets the <see cref="IModuleManager"/> where <see cref="IEngineModule"/>s
        /// are configured.
        /// </summary>
        IModuleManager ModuleManager { get; }
    }
}

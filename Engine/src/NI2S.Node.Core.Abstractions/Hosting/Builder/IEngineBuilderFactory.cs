// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Engine;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Provides an interface for implementing a factory that produces <see cref="IEngineBuilder"/> instances.
    /// </summary>
    public interface IEngineBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="IEngineBuilder" /> builder given a <paramref name="engineModules" />
        /// </summary>
        /// <param name="engineModules">An <see cref="IModuleCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="IEngineBuilder"/> configured with <paramref name="engineModules"/>.</returns>
        IEngineBuilder CreateBuilder(IModuleCollection engineModules);
    }
}

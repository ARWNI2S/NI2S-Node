// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node
{
    /// <summary>
    /// Represents platform specific configuration that will be applied to a <see cref="INodeHostBuilder"/> when building an <see cref="INodeHost"/>.
    /// </summary>
    public interface INodeStartup
    {
        /// <summary>
        /// Configure the <see cref="INodeHostBuilder"/>.
        /// </summary>
        /// <remarks>
        /// Configure is intended to be called before user code, allowing a user to overwrite any changes made.
        /// </remarks>
        /// <param name="builder"></param>
        void Configure(INodeHostBuilder builder);
    }
}
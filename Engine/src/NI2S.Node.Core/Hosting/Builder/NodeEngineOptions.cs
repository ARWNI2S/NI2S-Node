// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Options for configuring the behavior for <see cref="NodeEngine.CreateBuilder(NodeEngineOptions)"/>.
    /// </summary>
    public class NodeEngineOptions
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        public string[] Args { get; init; }

        /// <summary>
        /// The environment name.
        /// </summary>
        public string EnvironmentName { get; init; }

        /// <summary>
        /// The application name.
        /// </summary>
        public string ApplicationName { get; init; }

        /// <summary>
        /// The content root path.
        /// </summary>
        public string ContentRootPath { get; init; }

        /// <summary>
        /// The web root path.
        /// </summary>
        public string NodeRootPath { get; init; }
    }
}
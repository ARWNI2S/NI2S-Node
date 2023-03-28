using System;

namespace NI2S.Node.Configuration
{
    /// <summary>
    /// Options for configuring the behavior for <see cref="NI2SNode.CreateBuilder(NI2SNodeOptions)"/>.
    /// </summary>
    public class NI2SNodeOptions
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
        /// The node guid.
        /// </summary>
        public Guid NodeGuid { get; init; }

        /// <summary>
        /// The node assemby content root path.
        /// </summary>
        public string ContentRootPath { get; init; }

        /// <summary>
        /// The assets root path.
        /// </summary>
        public string AssetsRootPath { get; init; }
    }
}

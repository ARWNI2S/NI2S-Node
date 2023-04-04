// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Provides information about the hosting environment a niis node is running in.
    /// </summary>
    public interface INodeHostEnvironment : IHostEnvironment
    {
        //string AssetsPath { get; }

        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the node content files.
        /// </summary>
        string NodeRootPath { get; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="NodeRootPath"/>.
        /// </summary>
        IFileProvider NodeRootFileProvider { get; set; }
    }
}
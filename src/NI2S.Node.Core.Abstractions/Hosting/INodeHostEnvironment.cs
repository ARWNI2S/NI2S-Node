using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Provides information about the node hosting environment a niis engine is running in.
    /// </summary>
    public interface INodeHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the engine runtime assets content files.
        /// </summary>
        string AssetsRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="AssetsRootPath"/>.
        /// </summary>
        IFileProvider AssetsRootFileProvider { get; set; }
    }
}

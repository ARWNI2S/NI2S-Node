using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Provides information about the web hosting environment an application is running in.
    /// </summary>
    public interface INodeHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the web-servable application content files.
        /// </summary>
        string NodeRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="NodeRootPath"/>.
        /// </summary>
        IFileProvider NodeRootFileProvider { get; set; }

    }
}
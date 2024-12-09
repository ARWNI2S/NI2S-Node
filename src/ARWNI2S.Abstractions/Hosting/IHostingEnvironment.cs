using Microsoft.Extensions.FileProviders;

namespace ARWNI2S.Hosting
{
    /// <summary>
    /// Provides information about the node hosting environment an application is running in.
    /// <para>
    ///  This type is obsolete and will be removed in a future version.
    ///  The recommended alternative is ARWNI2S.Node.Hosting.INiisHostEnvironment.
    /// </para>
    /// </summary>
    [Obsolete("This type is obsolete and will be removed in a future version. The recommended alternative is ARWNI2S.Node.Hosting.INiisHostEnvironment.", error: false)]
    public interface IHostingEnvironment
    {
        /// <summary>
        /// Gets or sets the name of the environment. The host automatically sets this property to the value
        /// of the "ARWNI2S_ENVIRONMENT" environment variable, or "environment" as specified in any other configuration source.
        /// </summary>
        string EnvironmentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the application. This property is automatically set by the host to the assembly containing
        /// the application entry point.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the node-servable application content files.
        /// </summary>
        string NodeRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="NodeRootPath"/>.
        /// </summary>
        IFileProvider NodeRootFileProvider { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the application content files.
        /// </summary>
        string ContentRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="ContentRootPath"/>.
        /// </summary>
        IFileProvider ContentRootFileProvider { get; set; }
    }
}
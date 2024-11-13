using ARWNI2S.Node.Builder;

namespace ARWNI2S.Node.Hosting.Internal
{
    /// <summary>
    /// Represents platform specific configuration that will be applied to a <see cref="INodeHostBuilder"/> when building an <see cref="INodeHost"/>.
    /// </summary>
    public interface IHostingStartup
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
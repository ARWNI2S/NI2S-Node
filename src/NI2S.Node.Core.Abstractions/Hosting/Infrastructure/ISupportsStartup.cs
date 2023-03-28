using NI2S.Node.Hosting.Builder;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Hosting.Infrastructure
{
    /// <summary>
    /// An interface implemented by INodeHostBuilders that handle <see cref="NodeHostBuilderExtensions.Configure(INodeHostBuilder, Action{INodeBuilder})"/>,
    /// <see cref="NodeHostBuilderExtensions.UseStartup(INodeHostBuilder, Type)"/> and <see cref="NodeHostBuilderExtensions.UseStartup{TStartup}(INodeHostBuilder, Func{NodeHostBuilderContext, TStartup})"/>
    /// directly.
    /// </summary>
    public interface ISupportsStartup
    {
        /// <summary>
        /// Specify the startup method to be used to configure the web node.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="INodeBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder Configure(Action<INodeBuilder> configure);

        /// <summary>
        /// Specify the startup method to be used to configure the web node.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="INodeBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder Configure(Action<NodeHostBuilderContext, INodeBuilder> configure);

        /// <summary>
        /// Specify the startup type to be used by the web host.
        /// </summary>
        /// <param name="startupType">The <see cref="Type"/> to be used.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType);

        /// <summary>
        /// Specify a factory that creates the startup instance to be used by the web host.
        /// </summary>
        /// <param name="startupFactory">A delegate that specifies a factory for the startup class.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        /// <remarks>When using the IL linker, all public methods of <typeparamref name="TStartup"/> are preserved. This should match the Startup type directly (and not a base type).</remarks>
        INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory);
    }
}

using ARWNI2S.Engine.Builder;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Node.Hosting.Infrastructure
{
    /// <summary>
    /// An interface implemented by INodeHostBuilders that handle <see cref="NodeHostBuilderExtensions.Configure(INodeHostBuilder, Action{IEngineBuilder})"/>,
    /// <see cref="NodeHostBuilderExtensions.UseStartup(INodeHostBuilder, Type)"/> and <see cref="NodeHostBuilderExtensions.UseStartup{TStartup}(INodeHostBuilder, Func{NodeHostBuilderContext, TStartup})"/>
    /// directly.
    /// </summary>
    public interface ISupportsNodeStartup
    {
        /// <summary>
        /// Specify the startup method to be used to configure the node engine.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="IEngineBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder Configure(Action<IEngineBuilder> configure);

        /// <summary>
        /// Specify the startup method to be used to configure the node engine.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="IEngineBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder Configure(Action<NodeHostBuilderContext, IEngineBuilder> configure);

        /// <summary>
        /// Specify the startup type to be used by the node host.
        /// </summary>
        /// <param name="startupType">The <see cref="Type"/> to be used.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        INodeHostBuilder UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType);

        /// <summary>
        /// Specify a factory that creates the startup instance to be used by the node host.
        /// </summary>
        /// <param name="startupFactory">A delegate that specifies a factory for the startup class.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        /// <remarks>When in a trimmed engine, all public methods of <typeparamref name="TStartup"/> are preserved. This should match the Startup type directly (and not a base type).</remarks>
        INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory);
    }
}
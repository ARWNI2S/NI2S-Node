namespace ARWNI2S.Infrastructure.Engine.Features
{
    /// <summary>
    /// Provides acccess to the engine-scoped <see cref="IServiceProvider"/>.
    /// </summary>
    public interface IServiceProvidersFeature
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> scoped to the current frame.
        /// </summary>
        IServiceProvider EngineServices { get; set; }
    }
}
using ARWNI2S.Extensibility;

namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Encapsulates all specific information about an individual scoped context.
    /// </summary>
    public abstract class NiisContext
    {
        /// <summary>
        /// Gets or sets the <see cref="IModuleCollection"/> that provides access to the local node's module descriptors.
        /// </summary>
        public abstract IModuleCollection Modules { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the message's service container.
        /// </summary>
        public abstract IServiceProvider ServiceProvider { get; set; }
    }
}
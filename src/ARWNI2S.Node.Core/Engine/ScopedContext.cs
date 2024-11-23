namespace ARWNI2S.Node.Engine
{
    /// <summary>
    /// Encapsulates all specific information about an individual operation.
    /// </summary>
    public abstract class ScopedContext
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the message's service container.
        /// </summary>
        public abstract IServiceProvider ServiceProvider { get; set; }
    }
}
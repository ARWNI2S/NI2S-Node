// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Engine
{
    /// <summary>
    /// Provides methods to create and dispose of <see cref="WorkContext"/> instances.
    /// </summary>
    public interface IWorkContextFactory
    {
        //object WorkContextAccessor { get; }

        /// <summary>
        /// Creates an <see cref="WorkContext"/> instance for the specified set of HTTP modules.
        /// </summary>
        /// <param name="moduleCollection">The collection of HTTP modules to set on the created instance.</param>
        /// <returns>The <see cref="WorkContext"/> instance.</returns>
        IWorkContext Create(IModuleCollection moduleCollection);

        /// <summary>
        /// Releases resources held by the <see cref="WorkContext"/>.
        /// </summary>
        /// <param name="workContext">The <see cref="WorkContext"/> to dispose.</param>
        void Dispose(IWorkContext workContext);

        void Initialize(IWorkContext workContext, IModuleCollection moduleCollection);
    }
}

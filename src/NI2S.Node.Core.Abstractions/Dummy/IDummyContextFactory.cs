using NI2S.Node.Engine.Modules;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// Provides methods to create and dispose of <see cref="DummyContext"/> instances.
    /// </summary>
    public interface IDummyContextFactory
    {
        /// <summary>
        /// Creates an <see cref="DummyContext"/> instance for the specified set of HTTP features.
        /// </summary>
        /// <param name="featureCollection">The collection of HTTP features to set on the created instance.</param>
        /// <returns>The <see cref="DummyContext"/> instance.</returns>
        DummyContext Create(IDummyFeatureCollection featureCollection);

        /// <summary>
        /// Releases resources held by the <see cref="DummyContext"/>.
        /// </summary>
        /// <param name="httpContext">The <see cref="DummyContext"/> to dispose.</param>
        void Dispose(DummyContext httpContext);
    }
}

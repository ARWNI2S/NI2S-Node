using ARWNI2S.Infrastructure.EngineParts;

namespace ARWNI2S.Infrastructure.Engine.Features
{
    /// <summary>
    /// Marker interface for <see cref="IEngineFeatureProvider"/>
    /// implementations.
    /// </summary>
    public interface IEngineFeatureProvider
    {
    }

    /// <summary>
    /// A provider for a given <typeparamref name="TFeature"/> feature.
    /// </summary>
    /// <typeparam name="TFeature">The type of the feature.</typeparam>
    public interface IEngineFeatureProvider<TFeature> : IEngineFeatureProvider
    {
        /// <summary>
        /// Updates the <paramref name="feature"/> instance.
        /// </summary>
        /// <param name="parts">The list of <see cref="EnginePart"/> instances in the application.
        /// </param>
        /// <param name="feature">The feature instance to populate.</param>
        void PopulateFeature(IEnumerable<EnginePart> parts, TFeature feature);
    }
}

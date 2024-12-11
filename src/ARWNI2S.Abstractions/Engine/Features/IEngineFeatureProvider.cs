using ARWNI2S.Engine.Parts;

namespace ARWNI2S.Engine.Features
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
        /// <remarks>
        /// <see cref="EnginePart"/> instances in <paramref name="parts"/> appear in the same ordered sequence they
        /// are stored in <see cref="IApplicationPartManager.EngineParts"/>. This ordering may be used by the feature
        /// provider to make precedence decisions.
        /// </remarks>
        void PopulateFeature(IEnumerable<EnginePart> parts, TFeature feature);
    }
}
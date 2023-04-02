using System.Collections.Generic;

namespace NI2S.Node.Plugins
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
        /// <param name="parts">The list of <see cref="ApplicationPart"/> instances in the application.
        /// </param>
        /// <param name="feature">The feature instance to populate.</param>
        /// <remarks>
        /// <see cref="ApplicationPart"/> instances in <paramref name="parts"/> appear in the same ordered sequence they
        /// are stored in <see cref="ApplicationPartManager.ApplicationParts"/>. This ordering may be used by the feature
        /// provider to make precedence decisions.
        /// </remarks>
        void PopulateFeature(IEnumerable<ApplicationPart> parts, TFeature feature);
    }

}
using System;

namespace NI2S.Node.Engine.Modules
{
    /// <summary>
    /// Extension methods for getting feature from <see cref="IDummyFeatureCollection"/>
    /// </summary>
    public static class DummyFeatureCollectionExtensions
    {
        /// <summary>
        /// Retrives the requested feature from the collection.
        /// Throws an <see cref="InvalidOperationException"/> if the feature is not present.
        /// </summary>
        /// <param name="featureCollection">The <see cref="IDummyFeatureCollection"/>.</param>
        /// <typeparam name="TFeature">The feature key.</typeparam>
        /// <returns>The requested feature.</returns>
        public static TFeature GetRequiredFeature<TFeature>(this IDummyFeatureCollection featureCollection)
            where TFeature : notnull
        {
            if (featureCollection is null)
            {
                throw new ArgumentNullException(nameof(featureCollection));
            }

            return featureCollection.Get<TFeature>() ?? throw new InvalidOperationException($"Feature '{typeof(TFeature)}' is not present.");
        }

        /// <summary>
        /// Retrives the requested feature from the collection.
        /// Throws an <see cref="InvalidOperationException"/> if the feature is not present.
        /// </summary>
        /// <param name="featureCollection">feature collection</param>
        /// <param name="key">The feature key.</param>
        /// <returns>The requested feature.</returns>
        public static object GetRequiredFeature(this IDummyFeatureCollection featureCollection, Type key)
        {
            if (featureCollection is null)
            {
                throw new ArgumentNullException(nameof(featureCollection));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return featureCollection[key] ?? throw new InvalidOperationException($"Feature '{key}' is not present.");
        }
    }
}

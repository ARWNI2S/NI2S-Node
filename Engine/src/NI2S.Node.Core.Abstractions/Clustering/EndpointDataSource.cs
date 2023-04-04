using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Net;

namespace NI2S.Node.Clustering
{
    /// <summary>
    /// Provides a collection of <see cref="Endpoint"/> instances.
    /// </summary>
    public abstract class EndpointDataSource
    {
        /// <summary>
        /// Gets a <see cref="IChangeToken"/> used to signal invalidation of cached <see cref="Endpoint"/>
        /// instances.
        /// </summary>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public abstract IChangeToken GetChangeToken();

        /// <summary>
        /// Returns a read-only collection of <see cref="Endpoint"/> instances.
        /// </summary>
        public abstract IReadOnlyList<EndPoint> Endpoints { get; }
    }
}
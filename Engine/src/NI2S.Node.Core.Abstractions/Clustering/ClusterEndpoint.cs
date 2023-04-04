using System.Net;
using System;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// Represents an <see cref="Endpoint"/> that can be used in URL matching or URL generation.
    /// </summary>
    public sealed class ClusterEndpoint : EndPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterEndpoint"/> class.
        /// </summary>
        public ClusterEndpoint() : base() { }

        /// <summary>
        /// Gets the order value of endpoint.
        /// </summary>
        /// <remarks>
        /// The order value provides absolute control over the priority
        /// of an endpoint. Endpoints with a lower numeric value of order have higher priority.
        /// </remarks>
        public int Order { get; }
    }
}
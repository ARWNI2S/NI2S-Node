using NI2S.Node.Builder;
using NI2S.Node.Dummy;
using NI2S.Node.Routing.Patterns;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Routing
{
    /// <summary>
    /// Supports building a new <see cref="RouteEndpoint"/>.
    /// </summary>
    public sealed class RouteEndpointBuilder : EndpointBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="RoutePattern"/> associated with this endpoint.
        /// </summary>
        public RoutePattern RoutePattern { get; set; }

        /// <summary>
        /// Gets or sets the order assigned to the endpoint.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Constructs a new <see cref="RouteEndpointBuilder"/> instance.
        /// </summary>
        /// <param name="requestDelegate">The delegate used to process requests for the endpoint.</param>
        /// <param name="routePattern">The <see cref="RoutePattern"/> to use in URL matching.</param>
        /// <param name="order">The order assigned to the endpoint.</param>
        public RouteEndpointBuilder(
           RequestDelegate requestDelegate,
           RoutePattern routePattern,
           int order)
        {
            ArgumentNullException.ThrowIfNull(routePattern);

            RequestDelegate = requestDelegate;
            RoutePattern = routePattern;
            Order = order;
        }

        /// <inheritdoc />
        public override Endpoint Build()
        {
            if (RequestDelegate is null)
            {
                throw new InvalidOperationException($"{nameof(RequestDelegate)} must be specified to construct a {nameof(RouteEndpoint)}.");
            }

            return new RouteEndpoint(
                RequestDelegate,
                RoutePattern,
                Order,
                CreateMetadataCollection(Metadata),
                DisplayName);
        }

        private static EndpointMetadataCollection CreateMetadataCollection(IList<object> metadata)
        {
            if (metadata.Count > 0)
            {
                var hasCorsMetadata = false;
                IDummyMethodMetadata dummyMethodMetadata = null;

                // Before create the final collection we
                // need to update the IDummyMethodMetadata if
                // a CORS metadata is present
                for (var i = 0; i < metadata.Count; i++)
                {
                    // Not using else if since a metadata could have both
                    // interfaces.

                    if (metadata[i] is IDummyMethodMetadata methodMetadata)
                    {
                        // Storing only the last entry
                        // since the last metadata is the most significant.
                        dummyMethodMetadata = methodMetadata;
                    }

                    //if (!hasCorsMetadata && metadata[i] is ICorsMetadata)
                    //{
                    //    // IEnableCorsAttribute, IDisableCorsAttribute and ICorsPolicyMetadata
                    //    // are ICorsMetadata
                    //    hasCorsMetadata = true;
                    //}
                }

                if (hasCorsMetadata && dummyMethodMetadata is not null && !dummyMethodMetadata.AcceptCorsPreflight)
                {
                    // Since we found a CORS metadata we will update it
                    // to make sure the acceptCorsPreflight is set to true.
                    dummyMethodMetadata.AcceptCorsPreflight = true;
                }
            }

            return new EndpointMetadataCollection(metadata);
        }
    }
}

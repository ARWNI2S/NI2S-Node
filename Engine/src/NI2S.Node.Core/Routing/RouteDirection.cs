namespace NI2S.Node.Routing
{
    /// <summary>
    /// Indicates whether ASP.NET routing is processing a URL from an HTTP request or generating a URL.
    /// </summary>
    public enum RouteDirection
    {
        /// <summary>
        /// A URL from a client is being processed.
        /// </summary>
        IncomingRequest,

        /// <summary>
        /// A URL is being created based on the route definition.
        /// </summary>
        UrlGeneration,
    }
}
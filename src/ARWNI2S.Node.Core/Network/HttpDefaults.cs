﻿namespace ARWNI2S.Node.Core.Network
{
    /// <summary>
    /// Represents default values related to HTTP features
    /// </summary>
    public static partial class HttpDefaults
    {
        /// <summary>
        /// Gets the name of the default HTTP client
        /// </summary>
        public static string DefaultHttpClient => "default";

        /// <summary>
        /// Gets the name of a request item that stores the value that indicates whether the client is being redirected to a new location using POST
        /// </summary>
        public static string IsPostBeingDoneRequestItem => "ni2s.IsPOSTBeingDone";

        /// <summary>
        /// Gets the name of a request item that stores the value that indicates whether the request is being redirected by the generic route transformer
        /// </summary>
        public static string GenericRouteInternalRedirect => "ni2s.RedirectFromGenericPathRoute";
    }
}
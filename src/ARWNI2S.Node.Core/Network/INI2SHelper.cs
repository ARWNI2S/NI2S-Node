namespace ARWNI2S.Node.Core.Network
{
    public interface INI2SHelper
    {
        /// <summary>
        /// Get IP address from network context
        /// </summary>
        /// <returns>String of IP address</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// Gets node host location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL</param>
        /// <returns>Node host location</returns>
        string GetNodeHost(bool useSsl);

        /// <summary>
        /// Gets node location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL; pass null to determine automatically</param>
        /// <returns>Node location</returns>
        string GetNodeLocation(bool? useSsl = null);

        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the engine.
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        bool IsStaticAsset();

        /// <summary>
        /// Restart application domain
        /// </summary>
        void RestartAppDomain();

        /// <summary>
        /// Gets a value that indicates whether the client is being redirected to a new location
        /// </summary>
        bool IsBeingRedirected { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether the client is being redirected to a new location using POST
        /// </summary>
        bool IsPostBeingDone { get; set; }

        /// <summary>
        /// Gets current request protocol
        /// </summary>
        string GetCurrentProtocol();
    }
}

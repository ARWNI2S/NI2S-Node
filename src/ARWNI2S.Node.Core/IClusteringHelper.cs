namespace ARWNI2S.Node.Core
{
    public interface IClusteringHelper
    {
        /// <summary>
        /// Get IP address from the NI2S context
        /// </summary>
        /// <returns>String of IP address</returns>
        string GetCurrentIpAddress();

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
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        bool IsCurrentConnectionSecured();

        void RestartAppDomain();

    }
}

using ARWNI2S.Node.Core.Runtime;
using ARWNI2S.Node.Core.Runtime.Extensions;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Sockets;

namespace ARWNI2S.Node.Core
{
    /// <summary>
    /// Represents a node helper
    /// </summary>
    public partial class NodeHelper : INodeHelper
    {
        #region Fields  

        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IRuntimeContextAccessor _runtimeContextAccessor;
        private readonly Lazy<INodeContext> _nodeContext;

        #endregion

        #region Ctor

        public NodeHelper(IHostApplicationLifetime hostApplicationLifetime,
            IRuntimeContextAccessor runtimeContextAccessor,
            Lazy<INodeContext> nodeContext)
        {
            //_actionContextAccessor = actionContextAccessor;
            _hostApplicationLifetime = hostApplicationLifetime;
            _runtimeContextAccessor = runtimeContextAccessor;
            _nodeContext = nodeContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get IP address from HTTP context
        /// </summary>
        /// <returns>String of IP address</returns>
        public virtual string GetCurrentIpAddress()
        {
            // TODO: CHECK
            if (_runtimeContextAccessor.EngineContext?.Connection?.RemoteIpAddress is IPAddress remoteIp)
            {
                if (remoteIp.Equals(IPAddress.IPv6Loopback))
                    return IPAddress.Loopback.ToString();

                return remoteIp.MapToIPv4().ToString();
            }

            string hostName = GetNodeHost(IsCurrentConnectionSecured()); // Get the local host name
            // Get the IP addresses associated with the host
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
            string ipAddress = IPAddress.Loopback.ToString(); // Default to loopback

            if (ipAddresses.Length > 0)
            {
                foreach (IPAddress ip in ipAddresses)
                {
                    // Filter only IPv4 addresses and ignore loopback addresses
                    if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.Equals(IPAddress.Loopback))
                    {
                        ipAddress = ip.ToString();
                        break; // Return the first valid IPv4 address
                    }
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Gets node host location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL</param>
        /// <returns>Node host location</returns>
        public virtual string GetNodeHost(bool useSsl)
        {
            //try to get host from the request HOST header
            var host = _runtimeContextAccessor.EngineContext?.LocalHost;

            if (string.IsNullOrEmpty(host))
                return Dns.GetHostName();

            //add scheme to the URL
            var nodeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{host}";

            //ensure that host is ended with slash
            nodeHost = $"{nodeHost.TrimEnd('/')}/";

            return nodeHost;
        }

        /// <summary>
        /// Gets node location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL; pass null to determine automatically</param>
        /// <returns>Node location</returns>
        public virtual string GetNodeLocation(bool? useSsl = null)
        {
            var nodeLocation = string.Empty;

            //get node host
            var nodeHost = GetNodeHost(useSsl ?? IsCurrentConnectionSecured());
            if (!string.IsNullOrEmpty(nodeHost))
            {
                //add application path base if exists
                nodeLocation = _runtimeContextAccessor.EngineContext != null ? $"{nodeHost.TrimEnd('/')}{_runtimeContextAccessor.EngineContext.GetPathBase()}" : nodeHost;
            }

            //if host is empty (it is possible only when HttpContext is not available), use URL of a node entity configured in admin area
            if (string.IsNullOrEmpty(nodeHost))
                nodeLocation = _nodeContext.Value.GetCurrentNode()?.Hosts
                                ?? throw new NodeException("Current node cannot be loaded");

            //ensure that URL is ended with slash
            nodeLocation = $"{nodeLocation.TrimEnd('/')}/";

            return nodeLocation;
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            return _runtimeContextAccessor.EngineContext.IsSecureContext();
        }

        /// <summary>
        /// Restart application domain
        /// </summary>
        public virtual void RestartAppDomain()
        {
            _hostApplicationLifetime.StopApplication();
        }

        /// <summary>
        /// Gets current HTTP request protocol
        /// </summary>
        public virtual string GetCurrentRequestProtocol()
        {
            return IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

        #endregion
    }
}
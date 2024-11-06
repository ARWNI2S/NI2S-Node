using ARWNI2S.Infrastructure.Assets;
using ARWNI2S.Node.Core.Network;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace ARWNI2S.Node.Core
{
    /// <summary>
    /// Represents a node helper
    /// </summary>
    public partial class NI2SNetHelper : INI2SNetHelper
    {
        #region Fields  

        protected readonly IHostApplicationLifetime _hostApplicationLifetime;
        protected readonly INetworkContextAccessor _netContextAccessor;
        protected readonly Lazy<INodeContext> _nodeContext;

        #endregion

        #region Ctor

        public NI2SNetHelper(IHostApplicationLifetime hostApplicationLifetime,
            INetworkContextAccessor netContextAccessor,
            Lazy<INodeContext> nodeContext)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _netContextAccessor = netContextAccessor;
            _nodeContext = nodeContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether current HTTP request is available
        /// </summary>
        /// <returns>True if available; otherwise false</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (_netContextAccessor?.NetworkContext == null)
                return false;

            try
            {
                if (_netContextAccessor.NetworkContext?.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is IP address specified
        /// </summary>
        /// <param name="address">IP address</param>
        /// <returns>Result</returns>
        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            var rez = address != null && address.ToString() != IPAddress.IPv6Loopback.ToString();

            return rez;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get IP address from HTTP context
        /// </summary>
        /// <returns>String of IP address</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable() || _netContextAccessor.NetworkContext!.Connection.RemoteEndPoint is not IPEndPoint remoteIp)
                return string.Empty;

            return (remoteIp.Address.Equals(IPAddress.IPv6Loopback) ? IPAddress.Loopback : remoteIp.Address).ToString();
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;

            return _netContextAccessor.NetworkContext.Request.IsSecured;
        }

        /// <summary>
        /// Gets node host location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL</param>
        /// <returns>Node host location</returns>
        public virtual string GetNodeHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //try to get host from the request HOST header
            var hostHeader = _netContextAccessor.NetworkContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            //add scheme to the URL
            var nodeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

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
                nodeLocation = IsRequestAvailable() ? $"{nodeHost.TrimEnd('/')}{_netContextAccessor.NetworkContext.Request.PathBase}" : nodeHost;
            }

            //if host is empty (it is possible only when NetworkContext is not available), use URL of a node entity configured in admin area
            if (string.IsNullOrEmpty(nodeHost))
                nodeLocation = _nodeContext.Value.GetCurrentNode()?.Name
                                ?? throw new Exception("Current node cannot be loaded");

            //ensure that URL is ended with slash
            nodeLocation = $"{nodeLocation.TrimEnd('/')}/";

            return nodeLocation;
        }

        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        public virtual bool IsStaticAsset()
        {
            if (!IsRequestAvailable())
                return false;

            string path = _netContextAccessor.NetworkContext.Request.Path;

            //a little workaround. FileExtensionContentTypeProvider contains most of static file extensions. So we can use it
            //source: https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
            //if it can return content type, then it's a static file
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out var _);
        }

        /// <summary>
        /// Restart application domain
        /// </summary>
        public virtual void RestartAppDomain()
        {
            _hostApplicationLifetime.StopApplication();
        }

        /// <summary>
        /// Gets a value that indicates whether the client is being redirected to a new location
        /// </summary>
        public virtual bool IsBeingRedirected
        {
            get
            {
                var response = _netContextAccessor.NetworkContext.Response;
                //ASP.NET 4 style - return response.IsRequestBeingRedirected;
                int[] redirectionStatusCodes = [StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found];

                return redirectionStatusCodes.Contains(response.StatusCode);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the client is being redirected to a new location using POST
        /// </summary>
        public virtual bool IsPostBeingDone
        {
            get
            {
                if (_netContextAccessor.NetworkContext.Items[HttpDefaults.IsPostBeingDoneRequestItem] == null)
                    return false;

                return Convert.ToBoolean(_netContextAccessor.NetworkContext.Items[HttpDefaults.IsPostBeingDoneRequestItem]);
            }

            set => _netContextAccessor.NetworkContext.Items[HttpDefaults.IsPostBeingDoneRequestItem] = value;
        }

        /// <summary>
        /// Gets current HTTP request protocol
        /// </summary>
        public virtual string GetCurrentProtocol()
        {
            return IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

        ///// <summary>
        ///// Gets whether the specified HTTP request URI references the local host.
        ///// </summary>
        ///// <param name="req">HTTP request</param>
        ///// <returns>True, if HTTP request URI references to the local host</returns>
        //public virtual bool IsLocalRequest(HttpRequest req)
        //{
        //    //source: https://stackoverflow.com/a/41242493/7860424
        //    var connection = req.NetworkContext.Connection;
        //    if (IsIpAddressSet(connection.RemoteIpAddress))
        //    {
        //        //We have a remote address set up
        //        return IsIpAddressSet(connection.LocalIpAddress)
        //            //Is local is same as remote, then we are local
        //            ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
        //            //else we are remote if the remote IP address is not a loopback address
        //            : IPAddress.IsLoopback(connection.RemoteIpAddress);
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// Get the raw path and full query of request
        ///// </summary>
        ///// <param name="request">HTTP request</param>
        ///// <returns>Raw URL</returns>
        //public virtual string GetRawUrl(HttpRequest request)
        //{
        //    //first try to get the raw target from request feature
        //    //note: value has not been UrlDecoded
        //    var rawUrl = request.NetworkContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

        //    //or compose raw URL manually
        //    if (string.IsNullOrEmpty(rawUrl))
        //        rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";

        //    return rawUrl;
        //}

        ///// <summary>
        ///// Gets whether the request is made with AJAX 
        ///// </summary>
        ///// <param name="request">HTTP request</param>
        ///// <returns>Result</returns>
        //public virtual bool IsAjaxRequest(HttpRequest request)
        //{
        //    ArgumentNullException.ThrowIfNull(request);

        //    if (request.Headers == null)
        //        return false;

        //    return request.Headers.XRequestedWith == "XMLHttpRequest";
        //}

        #endregion
    }
}
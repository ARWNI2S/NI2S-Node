﻿using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// Represents a underlying persistent remote connection.
    /// </summary>
    public abstract class ContextInfo
    {
        protected ContextInfo()
        {

        }

        /// <summary>
        /// Gets or sets a unique identifier to represent this connection.
        /// </summary>
        public abstract string Id { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the remote target. Can be null.
        /// </summary>
        /// <remarks>The result is null if the connection isn't a TCP connection, e.g., a Unix Domain Socket or a transport that isn't TCP based.</remarks>
        public abstract IPAddress RemoteIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the port of the remote target.
        /// </summary>
        public abstract int RemotePort { get; set; }
        /// <summary>
        /// Gets or sets the IP address of the local host.
        /// </summary>
        public abstract IPAddress LocalIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the port of the local host.
        /// </summary>
        public abstract int LocalPort { get; set; }
        /// <summary>
        /// Gets or sets the client certificate.
        /// </summary>
        public abstract X509Certificate2 ClientCertificate { get; set; }

        /// <summary>
        /// Retrieves the client certificate.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Asynchronously returns an System.Security.Cryptography.X509Certificates.X509Certificate2.Can be null.
        /// </returns>
        public abstract Task<X509Certificate2> GetClientCertificateAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Close connection gracefully.
        /// </summary>
        public virtual void RequestClose() { }

    }
}

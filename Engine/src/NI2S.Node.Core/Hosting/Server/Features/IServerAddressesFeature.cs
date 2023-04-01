using System.Collections.Generic;

namespace NI2S.Node.Hosting.Server.Features
{
    /// <summary>
    /// Specifies the address used by the server.
    /// </summary>
    public interface IServerAddressesFeature
    {
        /// <summary>
        /// An <see cref="ICollection{T}" /> of addresses used by the server.
        /// </summary>
        ICollection<string> Addresses { get; }

        /// <summary>
        /// <see langword="true" /> to prefer URLs configured by the host rather than the server.
        /// </summary>
        bool PreferHostingUrls { get; set; }
    }
}

using System.Net;
using System.Net.Security;

namespace ARWNI2S.Node.Core.Network.Client
{
    public class SecurityOptions : SslClientAuthenticationOptions
    {
        public NetworkCredential Credential { get; set; }
    }
}
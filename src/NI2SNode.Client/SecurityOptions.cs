using System.Net;
using System.Net.Security;

namespace NI2S.Node.Client
{
    public class SecurityOptions : SslClientAuthenticationOptions
    {
        public NetworkCredential? Credential { get; set; }
    }
}

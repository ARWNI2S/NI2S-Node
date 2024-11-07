using ARWNI2S.Engine.Network;
using ARWNI2S.Infrastructure.Network;
using Microsoft.Extensions.Options;
using System.Text;

namespace ARWNI2S.Runtime.Network.Protocol
{
    class DefaultStringEncoderForDI : DefaultStringEncoder
    {
        public DefaultStringEncoderForDI(IOptions<ServerOptions> serverOptions)
            : base(serverOptions.Value?.DefaultTextEncoding ?? new UTF8Encoding(false))
        {

        }
    }
}
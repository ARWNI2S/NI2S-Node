using ARWNI2S.Engine.Network.Configuration.Options;
using ARWNI2S.Engine.Network.Protocol;
using Microsoft.Extensions.Options;
using System.Text;

namespace ARWNI2S.Runtime.Network
{
    class DefaultStringEncoderForDI : DefaultStringEncoder
    {
        public DefaultStringEncoderForDI(IOptions<ServerOptions> serverOptions)
            : base(serverOptions.Value?.DefaultTextEncoding ?? new UTF8Encoding(false))
        {

        }
    }
}
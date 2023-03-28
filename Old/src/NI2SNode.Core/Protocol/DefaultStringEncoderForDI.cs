using Microsoft.Extensions.Options;
using NI2S.Node.Configuration;
using System.Text;

namespace NI2S.Node.Protocol
{
    class DefaultStringEncoderForDI : DefaultStringEncoder
    {
        public DefaultStringEncoderForDI(IOptions<ServerOptions> serverOptions)
            : base(serverOptions.Value?.DefaultTextEncoding ?? new UTF8Encoding(false))
        {

        }
    }
}
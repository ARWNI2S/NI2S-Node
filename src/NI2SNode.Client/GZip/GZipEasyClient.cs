using Microsoft.Extensions.Logging;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using System.Security.Authentication;

namespace NI2S.Node.Client.GZip
{
    public class GZipEasyClient<TReceivePackage> : EasyClient<TReceivePackage> where TReceivePackage : class
    {
        public GZipEasyClient(IPipelineFilter<TReceivePackage> pipelineFilter) : base(pipelineFilter)
        {
        }

        public GZipEasyClient(IPipelineFilter<TReceivePackage> pipelineFilter, ILogger logger) : base(pipelineFilter, logger)
        {
        }

        public GZipEasyClient(IPipelineFilter<TReceivePackage> pipelineFilter, ChannelOptions options) : base(pipelineFilter, options)
        {
        }

        protected GZipEasyClient()
        {
        }
        protected override IConnector GetConnector()
        {
            var security = Security;

            if (security != null)
            {
                if (security.EnabledSslProtocols != SslProtocols.None)
                    return new SocketConnector(LocalEndPoint, new SslStreamConnector(security, new GZipConnector()));
            }

            return new SocketConnector(LocalEndPoint, new GZipConnector());
        }
    }
}

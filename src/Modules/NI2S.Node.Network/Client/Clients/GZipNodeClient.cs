using Microsoft.Extensions.Logging;
using NI2S.Node.Networking;
using NI2S.Node.Configuration.Options;
using System.Security.Authentication;

namespace NI2S.Node.Client
{
    public class GZipNodeClient<TReceivePackage> : NodeClient<TReceivePackage> where TReceivePackage : class
    {
        public GZipNodeClient(IPipelineFilter<TReceivePackage> pipelineFilter) : base(pipelineFilter)
        {
        }

        public GZipNodeClient(IPipelineFilter<TReceivePackage> pipelineFilter, ILogger logger) : base(pipelineFilter, logger)
        {
        }

        public GZipNodeClient(IPipelineFilter<TReceivePackage> pipelineFilter, ChannelOptions options) : base(pipelineFilter, options)
        {
        }

        protected GZipNodeClient()
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

using ARWNI2S.Node.Core.Network.Pipeline;
using ARWNI2S.Node.Core.Network.Protocol;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ARWNI2S.Node.Core.Network
{
    public partial class NodeClientFactory : INodeClientFactory
    {
        private readonly ConcurrentDictionary<string, NodeClient> _activeClients;

        private readonly ILoggerFactory _loggerFactory;

        public NodeClientFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public NodeClient GetOrCreateClient<TScope>()
        {
            if (!_activeClients.ContainsKey(typeof(TScope).Name))
            {
                _activeClients.AddOrUpdate(typeof(TScope).Name, new NodeClient(new ClientPipelineFilter(), new NI2SPacketEncoder(), _loggerFactory.CreateLogger<NodeClient>()), (_, client) => { return client; });
            }

            return _activeClients[typeof(TScope).Name];
        }
    }
}
using ARWNI2S.Node.Core.Network;
using ARWNI2S.Node.Core.Network.Client;
using ARWNI2S.Node.Core.Network.Protocol;
using ARWNI2S.Runtime.Clustering.Messages;

namespace ARWNI2S.Runtime.Clustering.Extensions
{
    internal static class NodeClientHealthMonitorExtensions
    {
        private static ReadOnlyMemory<byte> GenerateRequestQuorumData(QuorumRequest quorumRequest)
        {
            throw new NotImplementedException();
        }


        internal static async Task SendQuorumRequestAsync(this INodeClient<NI2SProtoPacket> nodeClient, QuorumRequest quorumRequest)
        {
            await nodeClient.SendAsync(GenerateRequestQuorumData(quorumRequest));
        }

        internal static async Task<QuorumResponse> ReceiveQuorumResponseAsync(this INodeClient<NI2SProtoPacket> nodeClient)
        {
            var quorumResponse = new QuorumResponse();
            var response = await nodeClient.ReceiveAsync();
            if (response != null)
            {

            }
            return quorumResponse;
        }
    }
}

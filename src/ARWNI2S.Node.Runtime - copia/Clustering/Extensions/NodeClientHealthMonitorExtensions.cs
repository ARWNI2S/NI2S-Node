using ARWNI2S.Runtime.Clustering.Messages;

namespace ARWNI2S.Runtime.Clustering.Extensions
{
    internal static class NodeClientHealthMonitorExtensions
    {
        private static NI2SProtoPacket GenerateRequestQuorumData(QuorumRequest quorumRequest)
        {
            throw new NotImplementedException();
        }

        internal static async Task SendQuorumRequestAsync(this NodeClient nodeClient, QuorumRequest quorumRequest)
        {
            await nodeClient.SendAsync(GenerateRequestQuorumData(quorumRequest));
        }


        internal static async Task<QuorumResponse> ReceiveQuorumResponseAsync(this NodeClient nodeClient)
        {
            var quorumResponse = new QuorumResponse();
            
            return await Task.FromResult(quorumResponse);
        }
    }
}

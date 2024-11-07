using ARWNI2S.Node.Core.Entities.Clustering;

namespace ARWNI2S.Runtime.Clustering.Messages
{
    internal class QuorumRequest
    {
        public QuorumRequest(Guid targetNode, NI2SNode votingNode)
        {
            TargetNode = targetNode;
            VotingNode = votingNode;
        }

        public Guid TargetNode { get; }
        public NI2SNode VotingNode { get; }
    }
}

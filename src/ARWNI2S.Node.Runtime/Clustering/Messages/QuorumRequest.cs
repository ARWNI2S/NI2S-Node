using ARWNI2S.Node.Core.Entities.Clustering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

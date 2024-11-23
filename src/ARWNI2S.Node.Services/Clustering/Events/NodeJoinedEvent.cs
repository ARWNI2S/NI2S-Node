using ARWNI2S.Node.Data.Entities.Clustering;

namespace ARWNI2S.Node.Services.Clustering.Events
{
    public class NodeJoinedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="node">Node</param>
        public NodeJoinedEvent(ClusterNode node/*, ExternalAuthenticationParameters parameters*/)
        {
            Node = node;
            //AuthenticationParameters = parameters;
        }

        /// <summary>
        /// Gets or sets node
        /// </summary>
        public ClusterNode Node { get; }

        ///// <summary>
        ///// Gets or sets external authentication parameters
        ///// </summary>
        //public ExternalAuthenticationParameters AuthenticationParameters { get; }
    }
}

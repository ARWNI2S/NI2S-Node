using ARWNI2S.Engine.Clustering.Data;

namespace ARWNI2S.Engine.Clustering.Events
{
    public class NodeDeadEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="node">Node</param>
        public NodeDeadEvent(ClusterNode node/*, ExternalAuthenticationParameters parameters*/)
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

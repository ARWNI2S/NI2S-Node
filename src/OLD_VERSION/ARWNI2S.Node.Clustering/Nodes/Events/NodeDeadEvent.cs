using ARWNI2S.Clustering.Data;

namespace ARWNI2S.Clustering.Nodes.Events
{
    public class NodeDeadEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="node">Node</param>
        public NodeDeadEvent(NI2SNode node/*, ExternalAuthenticationParameters parameters*/)
        {
            Node = node;
            //AuthenticationParameters = parameters;
        }

        /// <summary>
        /// Gets or sets node
        /// </summary>
        public NI2SNode Node { get; }

        ///// <summary>
        ///// Gets or sets external authentication parameters
        ///// </summary>
        //public ExternalAuthenticationParameters AuthenticationParameters { get; }
    }
}

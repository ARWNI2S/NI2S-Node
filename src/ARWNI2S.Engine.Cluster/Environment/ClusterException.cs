using System.Runtime.Serialization;

namespace ARWNI2S.Cluster.Environment
{
    public class ClusterException : NI2SException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterException"/> class.
        /// </summary>
        public ClusterException()
            : base("Unexpected error.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public ClusterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public ClusterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterException"/> class.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="info" /> is <see langword="null" />.</exception>
        [Obsolete]
        protected ClusterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

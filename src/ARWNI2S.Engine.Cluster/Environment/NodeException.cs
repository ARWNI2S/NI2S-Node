// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Runtime.Serialization;

namespace ARWNI2S.Cluster.Environment
{
    public class NodeException : NI2SException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class.
        /// </summary>
        public NodeException()
            : base("Unexpected error.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public NodeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public NodeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class.
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
        protected NodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

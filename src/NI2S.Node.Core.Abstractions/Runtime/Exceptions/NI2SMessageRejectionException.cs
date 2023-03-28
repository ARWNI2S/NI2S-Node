using System;
using System.Runtime.Serialization;

namespace NI2S.Node.Runtime
{
    /// <summary>
    /// Indicates that an Orleans message was rejected.
    /// </summary>
    [Serializable]
    [GenerateSerializer]
    public class NI2SMessageRejectionException : NI2SException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SMessageRejectionException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal NI2SMessageRejectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SMessageRejectionException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        internal NI2SMessageRejectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SMessageRejectionException"/> class. 
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="SerializationException">
        /// The class name is <see langword="null"/> or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> is <see langword="null"/>.
        /// </exception>
        protected NI2SMessageRejectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}


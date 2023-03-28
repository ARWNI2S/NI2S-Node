using System;
using System.Runtime.Serialization;

namespace NI2S.Node.Runtime
{
    /// <summary>
    /// An exception class used by the Orleans runtime for reporting errors.
    /// </summary>
    /// <remarks>
    /// This is also the base class for any more specific exceptions 
    /// raised by the Orleans runtime.
    /// </remarks>
    [Serializable]
    [GenerateSerializer]
    public class NI2SException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class.
        /// </summary>
        public NI2SException()
            : base("Unexpected error.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public NI2SException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public NI2SException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="info" /> is <see langword="null" />.</exception>
        protected NI2SException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
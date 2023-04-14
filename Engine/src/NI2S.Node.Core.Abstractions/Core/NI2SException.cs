// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Runtime.Serialization;

namespace NI2S.Node.Core
{
    /// <summary>
    /// Represents errors that occur during application execution
    /// </summary>
    [Serializable]
    public partial class NI2SException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class.
        /// </summary>
        public NI2SException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NI2SException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class with a specified error message.
        /// </summary>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public NI2SException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected NI2SException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SException"/> class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a <see langword="null"/> null reference if no inner
        /// exception is specified.</param>
        public NI2SException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
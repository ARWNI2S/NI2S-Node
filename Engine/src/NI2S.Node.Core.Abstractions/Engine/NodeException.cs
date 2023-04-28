﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Runtime.Serialization;

namespace NI2S.Node.Engine
{
    /// <summary>
    /// Represents errors that occur during engine execution
    /// </summary>
    [Serializable]
    public partial class NodeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class.
        /// </summary>
        public NodeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NodeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class with a specified error message.
        /// </summary>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public NodeException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected NodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeException"/> class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a <see langword="null"/> null reference if no inner
        /// exception is specified.</param>
        public NodeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
using Orleans;
using System;
using System.Runtime.Serialization;

namespace ARWNI2S.Exceptions
{
    /// <summary>
    /// Signifies that an attempt was made to invoke a actor extension method on a actor where that extension was not installed.
    /// </summary>
    [Serializable]
    [GenerateSerializer]
    public sealed class GrainExtensionNotInstalledException : NI2SException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrainExtensionNotInstalledException"/> class.
        /// </summary>
        public GrainExtensionNotInstalledException()
            : base("GrainExtensionNotInstalledException")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrainExtensionNotInstalledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public GrainExtensionNotInstalledException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrainExtensionNotInstalledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public GrainExtensionNotInstalledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrainExtensionNotInstalledException"/> class.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [Obsolete]
        private GrainExtensionNotInstalledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}


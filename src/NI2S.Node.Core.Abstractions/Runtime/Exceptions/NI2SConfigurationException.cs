using System;
using System.Runtime.Serialization;

namespace NI2S.Node.Runtime
{
    /// <summary>
    /// Represents a configuration exception.
    /// </summary>
    [Serializable]
    [GenerateSerializer]
    public sealed class NI2SConfigurationException : Exception
    {
        /// <inheritdoc />
        public NI2SConfigurationException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public NI2SConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <inheritdoc />
        /// <exception cref="SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="info" /> is <see langword="null" />.</exception>
        private NI2SConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
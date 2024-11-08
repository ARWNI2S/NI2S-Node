namespace ARWNI2S.Infrastructure.Lifecycle
{
    /// <summary>
    /// Indicates a lifecycle was canceled, either by request or due to observer error.
    /// </summary>
    [Serializable]
    public sealed class NI2SLifecycleCanceledException : NI2SException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SLifecycleCanceledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal NI2SLifecycleCanceledException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SLifecycleCanceledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        internal NI2SLifecycleCanceledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}


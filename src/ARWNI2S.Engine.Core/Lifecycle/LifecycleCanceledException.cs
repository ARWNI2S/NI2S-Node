namespace ARWNI2S.Lifecycle
{
    /// <summary>
    /// Indicates a lifecycle was canceled, either by request or due to observer error.
    /// </summary>
    [Serializable]
    public sealed class LifecycleCanceledException : NiisException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LifecycleCanceledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal LifecycleCanceledException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LifecycleCanceledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        internal LifecycleCanceledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}


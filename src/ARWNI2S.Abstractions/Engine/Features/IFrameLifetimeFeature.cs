namespace ARWNI2S.Node.Features
{
    /// <summary>
    /// Provides access to the HTTP request lifetime operations.
    /// </summary>
    public interface IFrameLifetimeFeature
    {
        /// <summary>
        /// A <see cref="CancellationToken"/> that fires if the request is aborted and
        /// the application should cease processing. The token will not fire if the request
        /// completes successfully.
        /// </summary>
        CancellationToken FrameAborted { get; set; }

        /// <summary>
        /// Forcefully aborts the request if it has not already completed. This will result in
        /// FrameAborted being triggered.
        /// </summary>
        void Abort();
    }
}
namespace ARWNI2S.Engine.Features
{
    /// <summary>
    /// Feature to uniquely identify a frame.
    /// </summary>
    internal interface IFrameIdentifierFeature
    {
        /// <summary>
        /// Gets or sets a value to uniquely identify a request.
        /// This can be used for logging and diagnostics.
        /// </summary>
        string TraceIdentifier { get; set; }
    }
}
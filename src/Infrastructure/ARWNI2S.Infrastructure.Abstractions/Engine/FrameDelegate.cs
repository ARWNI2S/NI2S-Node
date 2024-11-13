namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// A function that can process an NI2S message.
    /// </summary>
    /// <param name="context">The <see cref="EngineContext"/> for the message.</param>
    /// <returns>A task that represents the completion of message processing.</returns>
    public delegate Task FrameDelegate(EngineContext context);
}
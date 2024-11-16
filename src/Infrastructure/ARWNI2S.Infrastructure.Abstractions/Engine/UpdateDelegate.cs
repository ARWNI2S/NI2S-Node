namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// A function that can process an NI2S frame.
    /// </summary>
    /// <param name="context">The <see cref="EngineContext"/> for the frame.</param>
    /// <returns>A task that represents the completion of frame processing.</returns>
    public delegate Task UpdateDelegate(EngineContext context);
}
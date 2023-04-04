using System.Threading.Tasks;

namespace NI2S.Node.Engine
{
    /// <summary>
    /// A function that can process a system message.
    /// </summary>
    /// <param name="context">The <see cref="EngineContext"/> for the message.</param>
    /// <returns>A task that represents the completion of message processing.</returns>
    public delegate Task MessageHandlerDelegate(EngineContext context);
}

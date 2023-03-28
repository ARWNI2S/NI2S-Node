using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// A function that can process an HTTP request.
    /// </summary>
    /// <param name="context">The <see cref="DummyContext"/> for the request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public delegate Task MessageDelegate(DummyContext context);
}
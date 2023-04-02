using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// A delegate that is applied as a filter on a route handler.
    /// </summary>
    /// <param name="context">The <see cref="EndpointFilterInvocationContext"/> associated with the current request.</param>
    /// <returns>
    /// A <see cref="ValueTask"/> result of calling the handler and applying any modifications made by filters in the pipeline.
    /// </returns>
    public delegate ValueTask<object> EndpointFilterDelegate(EndpointFilterInvocationContext context);
}
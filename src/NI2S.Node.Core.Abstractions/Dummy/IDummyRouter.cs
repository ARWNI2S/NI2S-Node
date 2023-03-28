using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// Interface for implementing a router.
    /// </summary>
    public interface IDummyRouter
    {
        /// <summary>
        /// Asynchronously routes based on the current <paramref name="context"/>.
        /// </summary>
        /// <param name="context">A <see cref="DummyRouteContext"/> instance.</param>
        Task RouteAsync(DummyRouteContext context);

        /// <summary>
        /// Returns the URL that is associated with the route details provided in <paramref name="context"/>
        /// </summary>
        /// <param name="context">A <see cref="DummyVirtualPathContext"/> instance.</param>
        /// <returns>A <see cref="DummyVirtualPathData"/> object. Can be null.</returns>
        DummyVirtualPathData GetVirtualPath(DummyVirtualPathContext context);
    }
}
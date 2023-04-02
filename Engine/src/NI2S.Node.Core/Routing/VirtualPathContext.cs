using NI2S.Node.Dummy;

namespace NI2S.Node.Routing
{
    /// <summary>
    /// A context for virtual path generation operations.
    /// </summary>
    public class VirtualPathContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="VirtualPathContext"/>.
        /// </summary>
        /// <param name="dummyContext">The <see cref="Dummy.DummyContext"/> associated with the current request.</param>
        /// <param name="ambientValues">The set of route values associated with the current request.</param>
        /// <param name="values">The set of new values provided for virtual path generation.</param>
        public VirtualPathContext(
            DummyContext dummyContext,
            RouteValueDictionary ambientValues,
            RouteValueDictionary values)
            : this(dummyContext, ambientValues, values, null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="VirtualPathContext"/>.
        /// </summary>
        /// <param name="dummyContext">The <see cref="Dummy.DummyContext"/> associated with the current request.</param>
        /// <param name="ambientValues">The set of route values associated with the current request.</param>
        /// <param name="values">The set of new values provided for virtual path generation.</param>
        /// <param name="routeName">The name of the route to use for virtual path generation.</param>
        public VirtualPathContext(
            DummyContext dummyContext,
            RouteValueDictionary ambientValues,
            RouteValueDictionary values,
            string routeName)
        {
            DummyContext = dummyContext;
            AmbientValues = ambientValues;
            Values = values;
            RouteName = routeName;
        }

        /// <summary>
        /// Gets the set of route values associated with the current request.
        /// </summary>
        public RouteValueDictionary AmbientValues { get; }

        /// <summary>
        /// Gets the <see cref="Dummy.DummyContext"/> associated with the current request.
        /// </summary>
        public DummyContext DummyContext { get; }

        /// <summary>
        /// Gets the name of the route to use for virtual path generation.
        /// </summary>
        public string RouteName { get; }

        /// <summary>
        /// Gets or sets the set of new values provided for virtual path generation.
        /// </summary>
        public RouteValueDictionary Values { get; set; }
    }
}
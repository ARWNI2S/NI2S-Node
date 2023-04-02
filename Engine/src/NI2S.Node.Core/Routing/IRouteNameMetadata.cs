namespace NI2S.Node.Routing
{
    /// <summary>
    /// Represents metadata used during link generation to find
    /// the associated endpoint using route name.
    /// </summary>
    public interface IRouteNameMetadata
    {
        /// <summary>
        /// Gets the route name. Can be <see langword="null"/>.
        /// </summary>
        string RouteName { get; }
    }
}

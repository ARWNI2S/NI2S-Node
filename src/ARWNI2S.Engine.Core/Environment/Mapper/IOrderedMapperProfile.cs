namespace ARWNI2S.Engine.Environment.Mapper
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public interface IOrderedMapperProfile
    {
        /// <summary>
        /// Gets order of this configuration implementation
        /// </summary>
        int Order { get; }
    }
}

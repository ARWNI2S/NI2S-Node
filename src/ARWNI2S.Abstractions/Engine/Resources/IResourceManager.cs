namespace ARWNI2S.Engine.Resources
{

    /// <summary>
    /// Respresents a resource manager
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public interface IResourceManager<TResource> : ICollection<TResource>
        where TResource : class, IResource { }
}

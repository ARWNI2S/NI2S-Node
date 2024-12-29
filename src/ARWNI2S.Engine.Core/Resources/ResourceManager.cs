using ARWNI2S.Engine.Resources.Internal;

namespace ARWNI2S.Engine.Resources
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResourceManager<TResource> : IResourceManager<TResource>
        where TResource : class, IResource
    {
        private readonly HandleManager<TResource> _handleManager = new();

        //public virtual TResource this[object id] { get; }

        /// <inheritdoc/>
        public virtual bool Destroy(ResourceId id)
        {
            return false;
            //_handleManager.Release;
            //_handleManager.Dereference;
            //_handleManager.Acquire;
            //_handleManager.UsedHandles;
            //_handleManager.HasUsedHandles;
        }

        /// <inheritdoc/>
        public virtual bool Destroy(TResource resource)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual TResource Get(ResourceId id)
        {
            return default;
        }

        /// <inheritdoc/>
        public virtual bool Insert(ResourceId id, TResource resource)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual TResource Lock(ResourceId id)
        {
            return default;
        }

        /// <inheritdoc/>
        public virtual bool Remove(ResourceId id)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool Remove(TResource resource)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool Reserve(int size)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual int Unock(ResourceId id)
        {
            return -1;
        }

        /// <inheritdoc/>
        public virtual int Unock(TResource id)
        {
            return -1;
        }

        bool IResourceManager<TResource>.Destroy(object id) => Destroy((ResourceId)id);
        TResource IResourceManager<TResource>.Get(object id) => Get((ResourceId)id);
        bool IResourceManager<TResource>.Insert(object id, TResource resource) => Insert((ResourceId)id, resource);
        TResource IResourceManager<TResource>.Lock(object id) => Lock((ResourceId)id);
        bool IResourceManager<TResource>.Remove(object id) => Remove((ResourceId)id);
        int IResourceManager<TResource>.Unock(object id) => Unock((ResourceId)id);
    }
}

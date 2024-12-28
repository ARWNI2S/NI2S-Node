using ARWNI2S.Engine.Resources.Internal;
using ARWNI2S.Resources;

namespace ARWNI2S.Engine.Resources
{
    public abstract class ResourceManager<TResource, TInfo> : IResourceManager<TResource, TInfo>
        where TResource : class, IResource<TInfo>
        where TInfo : struct, IResourceInfo
    {
        private readonly HandleManager<TResource, TInfo> _handleManager = new();

        public virtual TResource Get(TInfo info)
        {
            //Create and acquire handler
            Handle<TInfo> handle = new();
            _handleManager.Acquire(ref handle);

            
        }
    }
}

using ARWNI2S.Engine.Resources;
using ARWNI2S.Engine.Resources.Internal;

namespace ARWNI2S.Environment
{
    public abstract class ResourceManagerBase<TResource>
        where TResource : class, IResource
    {
        private readonly HandleManager<TResource> _handleManager;

        protected ResourceManagerBase()
        {
            _handleManager = new HandleManager<TResource>();
        }

        protected TResource AcquireResource()
        {
            var handle = new Handle<TResource>();

            var resource = _handleManager.Acquire(ref handle);
            resource.SetHandle(handle);

            return resource;
        }

        protected void ReleaseResource(TResource resource)
        {
            _handleManager.Release(resource.GetHandle<TResource>());
        }
    }
}

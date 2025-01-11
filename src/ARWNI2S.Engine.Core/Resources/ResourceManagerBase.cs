using ARWNI2S.Engine.Core;
using ARWNI2S.Engine.Environment.Internal;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Engine.Resources
{
    public abstract class ResourceManagerBase<TKey, TResource>
        where TResource : ResourceBase
    {
        private readonly HandleManager<TResource> _handleManager;
        private readonly Dictionary<TKey, Handle<TResource>> _identities;

        protected ResourceManagerBase()
            : this(new ResourceKeyComparer()) { }

        protected ResourceManagerBase(IEqualityComparer<TKey> keyComparer)
        {
            _handleManager = new HandleManager<TResource>();
            _identities = new Dictionary<TKey, Handle<TResource>>(keyComparer);
        }

        internal Handle<TResource> Get(TKey key)
        {
            if (!_identities.ContainsKey(key))
            {
                Handle<TResource> handle = new();
                _identities.Add(key, handle);

                var res = _handleManager.Acquire(ref handle) ?? CreateEmpty();

                if (!res.LoadResource(key))
                {
                    Delete(handle);
                    _identities[key] = new();
                }

            }
            return _identities[key];
        }

        internal void Delete(Handle<TResource> handle)
        {
            var res = _handleManager.Dereference(handle);
            if (res != null)
            {
                _identities
                    .Where(x => x.Value == handle)
                    .ToList()
                    .ForEach(x => _identities.Remove(x.Key));

                res.UnloadResource();

                _handleManager.Release(handle);
            }
        }

        internal TResource GetResource(Handle<TResource> handle)
        {
            return _handleManager.Dereference(handle);
        }

        protected abstract TResource CreateEmpty();

        protected TResource GetResource(TKey key)
        {
            return GetResource(Get(key));
        }

        private class ResourceKeyComparer : IEqualityComparer<TKey>
        {
            public new bool Equals(TKey x, TKey y)
            {
                if (x is null && y is null) return true;
                if (x is null || y is null) return false;

                if (x is EntityId xId && y is EntityId yId)
                    return xId.Equals(yId);

                if (x is TKey xKey && y is TKey yKey)
                    return x.Equals(y);

                return false;
            }

            public int GetHashCode([DisallowNull] TKey obj)
            {
                return obj.GetHashCode();
            }
        }
    }

    public abstract class ResourceManagerBase<TResource> : ResourceManagerBase<object, TResource>
           where TResource : ResourceBase
    {

    }
}

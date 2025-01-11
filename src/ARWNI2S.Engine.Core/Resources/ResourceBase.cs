// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Resources
{
    public abstract class ResourceBase
    {
        private object _identity;

        protected TKey GetIdentity<TKey>()
        {
            if (_identity is not TKey key)
                throw new InvalidCastException($"Cannot cast identity to {typeof(TKey).Name}");

            return key;
        }

        protected bool SetIdentity<TKey>(TKey identity)
        {
            if (_identity is null)
            {
                _identity = identity;
                return true;
            }

            if (_identity is not TKey key)
                throw new InvalidCastException($"Cannot cast identity to {typeof(TKey).Name}");

            if (key.Equals(identity))
                return false;

            _identity = identity;
            return true;
        }

        internal bool LoadResource(object key)
        {
            _identity = key;
            try
            {
                if (!Create())
                    return false;

                OnCreated();
            }
            catch
            {
                return false;
            }

            return true;
        }

        internal void UnloadResource()
        {
            _identity = null;
            OnDestroy();
            Destroy();
        }

        protected abstract bool Create();

        protected virtual void OnCreated() { /* Do nothing */ }

        protected virtual void OnDestroy() { /* Do nothing */ }

        protected abstract void Destroy();
    }
}

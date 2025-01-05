using ARWNI2S.Engine.Core;
using ARWNI2S.Engine.Resources.Internal;

namespace ARWNI2S.Engine.Resources
{
    public abstract class ResourceBase : IResource
    {
        private bool disposedValue;
        private int _handle;

        protected ulong Id { get; }
        public Priority Priority { get; protected set; }
        public int ReferenceCount { get; protected set; }
        public TimeOnly LastAccess { get; protected set; }
        public abstract int Size { get; }
        public bool Disposed => disposedValue;
        public bool Locked => ReferenceCount > 0;

        public abstract void Clear();
        public abstract bool Create();
        public abstract void Destroy();
        public abstract bool Recreate();

        public static bool operator <(ResourceBase left, ResourceBase right)
        {
            if (left.Priority == right.Priority)
            {
                return (left.LastAccess < right.LastAccess);
            }
            return (left.Priority < right.Priority);
        }

        public static bool operator >(ResourceBase left, ResourceBase right)
        {
            return !(left < right);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        //// TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        //~ResourceBase()
        //{
        //    // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //    Dispose(disposing: false);
        //}

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        void IResource.SetHandle<TTag>(Handle<TTag> handle) { _handle = handle; }

        Handle<TTag> IResource.GetHandle<TTag>() { return (Handle<TTag>)_handle; }

        object INiisEntity.Id => Id;
    }
}

using ARWNI2S.Engine.Core;

namespace ARWNI2S.Engine.Resources.Assets
{
    public abstract class NI2SAsset : IResource
    {
        private bool disposedValue;

        public AssetId Id { get; }
        public bool Disposed => disposedValue;
        public Priority Priority { get; set; }
        public int ReferenceCount { get; set; }
        public bool Locked { get; }
        public TimeOnly LastAccess { get; set; }

        public abstract void Clear();
        public abstract bool Create();
        public abstract void Destroy();
        public abstract bool Recreate();

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

        // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        ~NI2SAsset()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        object INiisEntity.Id => Id;
    }
}

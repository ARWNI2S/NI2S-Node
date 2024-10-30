using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Infrastructure.Memory
{
    /// <summary>
    /// Represents a memory pool of objects.
    /// </summary>
    /// <typeparam name="T">Any object</typeparam>
    public abstract class ObjectPool<T> : IPool<T>, IDisposable
    {
        private static readonly int RECOMMENDED_MAXSIZE = 5000;

        /// <summary>
        /// Contiene los objetos actualmente en la reserva de memoria.
        /// </summary>
        protected List<T> pool;
        private int _nbrObjects = 0;
        private int _maxSize = RECOMMENDED_MAXSIZE;
        private bool _disposed;

        private int _minPoolSize;

        /// <summary>
        /// Devuelve el numero de objetos en la reserva de memoria.
        /// </summary>
        public int Count
        {
            get { return pool.Count; }
        }
        /// <summary>
        /// Devuelve el tamaño maximo de la reserva de memoria.
        /// </summary>
        public int MaxSize
        {
            get { return _maxSize; }
        }
        /// <summary>
        /// Devuelve el numero de objetos en memoria.
        /// </summary>
        public int TotalObjects
        {
            get { return _nbrObjects; }
        }

        public int MinPoolSize
        {
            get
            {
                return _minPoolSize;
            }
        }

        public int MaxPoolSize
        {
            get
            {
                return _maxSize;
            }
        }

        public int AvailableItemsCount
        {
            get
            {
                return pool.Count;
            }
        }

        public int TotalItemsCount
        {
            get
            {
                return _nbrObjects;
            }
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        protected ObjectPool()
            : this(0, RECOMMENDED_MAXSIZE) { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxSize">El tamaño maximo de la reserva de memoria.</param>
        protected ObjectPool(int maxSize)
            : this(0, maxSize) { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxSize">El tamaño maximo de la reserva de memoria.</param>
        /// <param name="fill">Indica si deben iniciarse todos los objetos de la reserva de memoria.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Pool pattern implementation used for item creation internally.")]
        protected ObjectPool(int maxSize, bool fill)
            : this(fill ? maxSize : 0, maxSize) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxSize">El tamaño maximo de la reserva de memoria.</param>
        /// <param name="initialObjects">Indica el numero inicial de objetos en la reserva de memoria. 0 o un numero negativo
        /// no crearan ningun objeto, un numero mayor que <paramref name="maxSize"/> inicializa la reserva de memoria al maximo de
        /// su capacidad.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Pool pattern implementation used for item creation internally.")]
        protected ObjectPool(int maxSize, int initialObjects)
        {
            Initialize(initialObjects, maxSize);
        }

        public void Initialize(int minPoolSize, int maxPoolSize)
        {
            _maxSize = maxPoolSize;
            pool = new List<T>(_maxSize);
            if (minPoolSize > 0)
                Fill(minPoolSize);
        }

        public void Push(T item)
        {
            ReleaseObject(item);
        }

        public bool TryGet(out T item)
        {
            item = GetObject();
            return item != null;
        }

        /// <summary>
        /// Always allows for unmanaged resource disposing.
        /// </summary>
        ~ObjectPool()
        {
            Dispose(false);
        }

        /// <summary>
        /// Rellena la reserva de memoria hasta alcanzar el tamaño maximo de la reserva.
        /// </summary>
        public void Fill()
        {
            Fill(_maxSize - _nbrObjects);
        }

        /// <summary>
        /// Rellena la reserva de memoria con un numero de objetos determinado por el parametro <paramref name="nbrNewObjects"/>, hasta alcanzar el tamaño maximo de la reserva.
        /// </summary>
        /// <param name="nbrNewObjects">Indica el numero de objetos a añadir a la reserva de memoria.</param>
        public void Fill(int nbrNewObjects)
        {
            lock (pool)
            {
                if (_nbrObjects + nbrNewObjects > _maxSize)
                    nbrNewObjects = _maxSize - _nbrObjects;

                for (int i = 0; i < nbrNewObjects; i++)
                {
                    pool.Add(CreateNewPoolObject());
                    _nbrObjects++;
                }
            }
        }

        /// <summary>
        /// Metodo abstracto para la creacion de un objeto del tipo generico <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Un nuevo objeto del tipo generico <typeparamref name="T"/>.</returns>
        protected abstract T CreateNewPoolObject();

        /// <summary>
        /// Devuelve un objeto de la reserva de memoria. Si la reserva de memoria esta vacio, se crea un nuevo objeto, hasta el tamaño maximo permitido.
        /// Si se ha alcanzado el tamaño maximo, la solicitud queda en espera hasta que se libera un objeto.
        /// </summary>
        /// <returns>Un objeto del tipo generico <typeparamref name="T"/>, extraido de la reserva de memoria.</returns>
        private T GetObject()
        {
            lock (pool)
            {
                T obj;
                if (pool.Count == 0 && _nbrObjects < _maxSize) // no hay objetos pero queda espacio para nuevos.
                {
                    obj = CreateNewPoolObject();
                    _nbrObjects++;
                }
                else
                {
                    // Si no hay objetos, debemos esperar a que se libere al menos uno.
                    while (pool.Count == 0)
                        Monitor.Wait(pool);

                    obj = pool[pool.Count - 1];   // mas rapido al final.
                    pool.RemoveAt(pool.Count - 1);
                }
                return obj;
            }
        }

        /// <summary>
        /// Libera un objeto en uso y lo situa en la reserva de memoria.
        /// </summary>
        /// <param name="obj">El objeto en uso que va a ser liberado.</param>
        protected virtual void ReleaseObject(T obj)
        {
            lock (pool)
            {
                pool.Add(obj);
                Monitor.Pulse(pool);
            }
        }

        #region Miembros de IDisposable

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// The bulk of the clean-up code is implemented in Dispose(bool)
        /// </summary>
        /// <param name="disposing">is Disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // allow objects to free unmanaged resources
                if (typeof(T) is IDisposable)
                    if (pool != null && pool.Count > 0)
                        foreach (T obj in pool)
                            ((IDisposable)obj).Dispose();

                if (disposing)
                {
                    // free managed resources
                    for (int a = 0; a < pool.Count; a++)
                        pool[a] = default;
                    pool.Clear();
                    pool = null;
                }

                _disposed = true;
            }
        }
    }
}

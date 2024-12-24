
namespace ARWNI2S.Engine.Runtime
{
    public abstract partial class UpdateFunction : IDisposable
    {
        private static readonly IList<UpdateFunction> _pool;

        private static int instanceCount;

        static UpdateFunction() { _pool = []; }

        public static T Get<T>() where T : UpdateFunction, new()
        {
            if (_pool.Count == 0 && instanceCount == Constants.MemPool_MaxSize)
                Monitor.Wait(_pool);


            if (Get(typeof(T)) is not T result)
            {
                result = new T();

                instanceCount++;
                if (instanceCount > Constants.MemPool_MaxSize && _pool.Count == 1)
                    _pool.RemoveAt(0);
            }

            return result;
        }
        private static UpdateFunction Get(Type type)
        {
            lock (_pool)
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    if (type.Equals(_pool[i].GetType()))
                    {
                        var result = _pool[i];
                        _pool.RemoveAt(i);
                        return result;
                    }
                }
            }
            return null;
        }

        protected UpdateFunction()
        {
            GC.SuppressFinalize(this);
        }

        private UpdateFunction(UpdateFunction source) { throw new InvalidOperationException("Do not copy"); }

        /// <summary>
        /// Destructor, unregisters the update function
        /// </summary>
        ~UpdateFunction() { GC.ReRegisterForFinalize(this); }

        public void Dispose()
        {
            Clear();
            _pool.Add(this);
        }

        private void Clear()
        {
            throw new NotImplementedException();
        }
    }
}

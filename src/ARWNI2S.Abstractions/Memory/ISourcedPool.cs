namespace ARWNI2S.Memory
{
    public interface ISourcedPool<T> : IPoolInfo
    {
        /// <summary>
        /// Initializes the specified min pool size.
        /// </summary>
        /// <param name="minPoolSize">The min size of the pool.</param>
        /// <param name="maxPoolSize">The max size of the pool.</param>
        /// <param name="sourceCreator">The source creator.</param>
        /// <returns></returns>
        void Initialize(int minPoolSize, int maxPoolSize, IPoolSourceCreator<T> sourceCreator);

        /// <summary>
        /// Pushes the specified item into the pool.
        /// </summary>
        /// <param name="item">The item.</param>
        void Push(T item);

        /// <summary>
        /// Tries to get one item from the pool.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool TryGet(out T item);
    }
}

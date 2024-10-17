namespace ARWNI2S.Infrastructure.Memory
{
    /// <summary>
    /// The basic interface of smart pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T> : IPoolInfo
    {
        /// <summary>
        /// Initializes the specified min pool size.
        /// </summary>
        /// <param name="minPoolSize">The min size of the pool.</param>
        /// <param name="maxPoolSize">The max size of the pool.</param>
        /// <returns></returns>
        void Initialize(int minPoolSize, int maxPoolSize);

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

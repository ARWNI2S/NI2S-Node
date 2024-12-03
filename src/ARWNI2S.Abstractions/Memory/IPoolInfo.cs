namespace ARWNI2S.Memory
{
    /// <summary>
    /// The pool information interface
    /// </summary>
    public interface IPoolInfo
    {
        /// <summary>
        /// Gets the min size of the pool.
        /// </summary>
        /// <value>
        /// The min size of the pool.
        /// </value>
        int MinPoolSize { get; }

        /// <summary>
        /// Gets the max size of the pool.
        /// </summary>
        /// <value>
        /// The max size of the pool.
        /// </value>
        int MaxPoolSize { get; }

        /// <summary>
        /// Gets the avialable items count.
        /// </summary>
        /// <value>
        /// The avialable items count.
        /// </value>
        int AvailableItemsCount { get; }

        /// <summary>
        /// Gets the total items count, include items in the pool and outside the pool.
        /// </summary>
        /// <value>
        /// The total items count.
        /// </value>
        int TotalItemsCount { get; }
    }
}

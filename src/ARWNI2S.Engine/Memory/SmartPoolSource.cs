namespace ARWNI2S.Engine.Memory
{
    /// <summary>
    /// SmartPoolSource
    /// </summary>
    public class SmartPoolSource : IPoolSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartPoolSource" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="itemsCount">The items count.</param>
        public SmartPoolSource(object source, int itemsCount)
        {
            Source = source;
            Count = itemsCount;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public object Source { get; private set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; private set; }
    }
}

using System.Collections.Concurrent;

namespace ARWNI2S.Infrastructure.Memory
{
    /// <summary>
    /// The smart pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartPool<T> : ISourcedPool<T>
    {
        private ConcurrentStack<T> _globalStack;

        private IPoolSource[] _itemsSource;

        private int _currentSourceCount;

        private IPoolSourceCreator<T> _sourceCreator;

        private int _minPoolSize;

        /// <summary>
        /// Gets the size of the min pool.
        /// </summary>
        /// <value>
        /// The size of the min pool.
        /// </value>
        public int MinPoolSize
        {
            get
            {
                return _minPoolSize;
            }
        }

        private int _maxPoolSize;

        /// <summary>
        /// Gets the size of the max pool.
        /// </summary>
        /// <value>
        /// The size of the max pool.
        /// </value>
        public int MaxPoolSize
        {
            get
            {
                return _maxPoolSize;
            }
        }

        /// <summary>
        /// Gets the avialable items count.
        /// </summary>
        /// <value>
        /// The avialable items count.
        /// </value>
        public int AvailableItemsCount
        {
            get
            {
                return _globalStack.Count;
            }
        }

        private int _totalItemsCount;

        /// <summary>
        /// Gets the total items count, include items in the pool and outside the pool.
        /// </summary>
        /// <value>
        /// The total items count.
        /// </value>
        public int TotalItemsCount
        {
            get { return _totalItemsCount; }
        }

        /// <summary>
        /// Initializes the specified min and max pool size.
        /// </summary>
        /// <param name="minPoolSize">The min size of the pool.</param>
        /// <param name="maxPoolSize">The max size of the pool.</param>
        /// <param name="sourceCreator">The source creator.</param>
        public void Initialize(int minPoolSize, int maxPoolSize, IPoolSourceCreator<T> sourceCreator)
        {
            _minPoolSize = minPoolSize;
            _maxPoolSize = maxPoolSize;
            _sourceCreator = sourceCreator;
            _globalStack = new ConcurrentStack<T>();

            var n = 0;

            if (minPoolSize != maxPoolSize)
            {
                var currentValue = minPoolSize;

                while (true)
                {
                    n++;

                    var thisValue = currentValue * 2;

                    if (thisValue >= maxPoolSize)
                        break;

                    currentValue = thisValue;
                }
            }

            _itemsSource = new IPoolSource[n + 1];

            T[] items;
            _itemsSource[0] = sourceCreator.Create(minPoolSize, out items);
            _currentSourceCount = 1;

            for (var i = 0; i < items.Length; i++)
            {
                _globalStack.Push(items[i]);
            }

            _totalItemsCount = _minPoolSize;
        }

        private int _isIncreasing = 0;

        /// <summary>
        /// Pushes the specified item into the pool.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Push(T item)
        {
            _globalStack.Push(item);
        }

        bool TryPopWithWait(out T item, int waitTicks)
        {
            var spinWait = new SpinWait();

            while (true)
            {
                spinWait.SpinOnce();

                if (_globalStack.TryPop(out item))
                    return true;

                if (spinWait.Count >= waitTicks)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries to get one item from the pool.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool TryGet(out T item)
        {
            if (_globalStack.TryPop(out item))
                return true;

            var currentSourceCount = _currentSourceCount;

            if (currentSourceCount >= _itemsSource.Length)
            {
                return TryPopWithWait(out item, 100);
            }

            var isIncreasing = _isIncreasing;

            if (isIncreasing == 1)
                return TryPopWithWait(out item, 100);

            if (Interlocked.CompareExchange(ref _isIncreasing, 1, isIncreasing) != isIncreasing)
                return TryPopWithWait(out item, 100);

            IncreaseCapacity();

            _isIncreasing = 0;

            if (!_globalStack.TryPop(out item))
            {
                return false;
            }

            return true;
        }

        private void IncreaseCapacity()
        {
            var newItemsCount = Math.Min(_totalItemsCount, _maxPoolSize - _totalItemsCount);

            T[] items;
            _itemsSource[_currentSourceCount++] = _sourceCreator.Create(newItemsCount, out items);

            _totalItemsCount += newItemsCount;

            for (var i = 0; i < items.Length; i++)
            {
                _globalStack.Push(items[i]);
            }
        }
    }
}

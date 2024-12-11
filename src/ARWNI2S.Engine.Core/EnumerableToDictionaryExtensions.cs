namespace ARWNI2S
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class EnumerableToDictionaryExtensions
    {
        /// <summary>
        /// Convert to lookup-like dictionary, for JSON serialization
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="xs">List of objects</param>
        /// <param name="keySelector">A key-selector function</param>
        /// <param name="valueSelector">A value-selector function</param>
        /// <returns>A dictionary with values grouped by key</returns>
        public static IDictionary<TKey, IList<TValue>> ToGroupedDictionary<T, TKey, TValue>(
          this IEnumerable<T> xs,
          Func<T, TKey> keySelector,
          Func<T, TValue> valueSelector)
        {
            var result = new Dictionary<TKey, IList<TValue>>();

            foreach (var x in xs)
            {
                var key = keySelector(x);
                var value = valueSelector(x);

                if (result.TryGetValue(key, out var list))
                    list.Add(value);
                else
                    result[key] = [value];
            }

            return result;
        }

        /// <summary>
        /// Convert to lookup-like dictionary, for JSON serialization
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="xs">List of objects</param>
        /// <param name="keySelector">A key-selector function</param>
        /// <returns>A dictionary with values grouped by key</returns>
        public static IDictionary<TKey, IList<T>> ToGroupedDictionary<T, TKey>(
          this IEnumerable<T> xs,
          Func<T, TKey> keySelector)
        {
            return xs.ToGroupedDictionary(keySelector, x => x);
        }
    }
}

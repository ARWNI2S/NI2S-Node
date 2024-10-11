namespace ARWNI2S.Infrastructure.Collections
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Shortcut to create HashSet from IEnumerable that supports type inference
        /// (which the standard constructor does not)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HashSet<T> ToSet<T>(this IEnumerable<T> values)
        {
            if (values == null)
                return null;
            return new HashSet<T>(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ListEquals<T>(this IList<T> a, IList<T> b)
        {
            if (a.Count != b.Count)
                return false;
            return new HashSet<T>(a).SetEquals(new HashSet<T>(b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IEnumerableEquals<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return new HashSet<T>(a).SetEquals(new HashSet<T>(b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSupersetOf<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return new HashSet<T>(a).IsSupersetOf(new HashSet<T>(b));
        }

        /// <summary>
        /// Gets the value by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<object, object> dictionary, object key)
            where T : new()
        {
            T defaultValue = default;
            return GetValue(dictionary, key, defaultValue);
        }

        /// <summary>
        /// Gets the value by key and default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<object, object> dictionary, object key, T defaultValue)
        {

            if (!dictionary.TryGetValue(key, out object valueObj))
            {
                return defaultValue;
            }
            else
            {
                return (T)valueObj;
            }
        }

        /// <summary>
        /// </summary>
        public static IEnumerable<List<T>> BatchIEnumerable<T>(this IEnumerable<T> sequence, int batchSize)
        {
            var batch = new List<T>(batchSize);
            foreach (var item in sequence)
            {
                batch.Add(item);
                // when we've accumulated enough in the batch, send it out  
                if (batch.Count >= batchSize)
                {
                    yield return batch; // batch.ToArray();
                    batch = new List<T>(batchSize);
                }
            }
            if (batch.Count > 0)
            {
                yield return batch; //batch.ToArray();
            }
        }

        /// <summary>
        /// Synchronize contents of two dictionaries with mutable values
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="a">Dictionary</param>
        /// <param name="b">Dictionary</param>
        /// <param name="copy">Return a copy of a value</param>
        /// <param name="sync">Synchronize two mutable values</param>
        private static void Synchronize<TKey, TValue>(this Dictionary<TKey, TValue> a, Dictionary<TKey, TValue> b, Func<TValue, TValue> copy, Action<TValue, TValue> sync)
        {
            var akeys = a.Keys.ToSet();
            var bkeys = b.Keys.ToSet();
            var aonly = akeys.Except(bkeys).ToSet();
            var bonly = bkeys.Except(akeys).ToSet();
            var both = akeys.Intersect(bkeys).ToSet();
            foreach (var ak in aonly)
            {
                b.Add(ak, copy(a[ak]));
            }
            foreach (var bk in bonly)
            {
                a.Add(bk, copy(b[bk]));
            }
            foreach (var k in both)
            {
                sync(a[k], b[k]);
            }
        }

        /// <summary>
        /// Synchronize contents of two dictionaries with immutable values
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="a">Dictionary</param>
        /// <param name="b">Dictionary</param>
        /// <param name="sync">Synchronize two values, return synced value</param>
        private static void Synchronize<TKey, TValue>(this Dictionary<TKey, TValue> a, Dictionary<TKey, TValue> b, Func<TValue, TValue, TValue> sync)
        {
            var akeys = a.Keys.ToSet();
            var bkeys = b.Keys.ToSet();
            var aonly = akeys.Except(bkeys).ToSet();
            var bonly = bkeys.Except(akeys).ToSet();
            var both = akeys.Intersect(bkeys).ToSet();
            foreach (var ak in aonly)
            {
                b.Add(ak, a[ak]);
            }
            foreach (var bk in bonly)
            {
                a.Add(bk, b[bk]);
            }
            foreach (var k in both)
            {
                var s = sync(a[k], b[k]);
                a[k] = s;
                b[k] = s;
            }
        }

        /// <summary>
        /// Synchronize contents of two nested dictionaries with mutable values
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TKey2">Nested key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="a">Dictionary</param>
        /// <param name="b">Dictionary</param>
        /// <param name="copy">Return a copy of a value</param>
        /// <param name="sync">Synchronize two mutable values</param>
        private static void Synchronize2<TKey, TKey2, TValue>(this Dictionary<TKey, Dictionary<TKey2, TValue>> a, Dictionary<TKey, Dictionary<TKey2, TValue>> b, Func<TValue, TValue> copy, Action<TValue, TValue> sync)
        {
            a.Synchronize(b, d => d.Copy(copy), (d1, d2) => d1.Synchronize(d2, copy, sync));
        }

        /// <summary>
        /// Synchronize contents of two nested dictionaries with immutable values
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TKey2">Nested key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="a">Dictionary</param>
        /// <param name="b">Dictionary</param>
        /// <param name="sync">Synchronize two immutable values</param>
        private static void Synchronize2<TKey, TKey2, TValue>(this Dictionary<TKey, Dictionary<TKey2, TValue>> a, Dictionary<TKey, Dictionary<TKey2, TValue>> b, Func<TValue, TValue, TValue> sync)
        {
            a.Synchronize(b, d => new Dictionary<TKey2, TValue>(d), (d1, d2) => d1.Synchronize(d2, sync));
        }

        public static Dictionary<TKey, TValue> Copy<TKey, TValue>(this Dictionary<TKey, TValue> original)
        {
            return new Dictionary<TKey, TValue>(original);
        }

        /// <summary>
        /// Copy a dictionary with mutable values
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="original"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        private static Dictionary<TKey, TValue> Copy<TKey, TValue>(this Dictionary<TKey, TValue> original, Func<TValue, TValue> copy)
        {
            return original.ToDictionary(pair => pair.Key, pair => copy(pair.Value));
        }

        /// <summary>
        /// ToString every element of an enumeration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="toString">Can supply null to use Object.ToString()</param>
        /// <param name="separator">Before each element, or space if unspecified</param>
        /// <returns></returns>
        public static string ToStrings<T>(this IEnumerable<T> list, Func<T, object> toString = null, string separator = " ")
        {
            if (list == null) return "";
            toString = toString ?? (x => x);
            //Func<T, string> toStringPrinter = (x => 
            //    {
            //        object obj = toString(x);
            //        if(obj != null)
            //            return obj.ToString();
            //        else
            //            return "null";
            //    });
            //return Utils.IEnumerableToString(list, toStringPrinter, separator);
            //Do NOT use Aggregate for string concatenation. It is very inefficient, will reallocate and copy lots of intermediate strings.
            //toString = toString ?? (x => x);
            return list.Aggregate("", (s, x) => s + separator + toString(x));
        }

        public static List<T> Union<T>(this List<T> list, List<T> other)
        {
            if (list == null && other == null)
                return null;
            if (list == null)
                return other;
            if (other == null)
                return list;
            list.AddRange(other);
            return list;
        }

        /// <summary>
        /// Clones the elements in the specific range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static T[] CloneRange<T>(this IList<T> source, int offset, int length)
        {
            T[] target;


            if (source is T[] array)
            {
                target = new T[length];
                Array.Copy(array, offset, target, 0, length);
                return target;
            }

            target = new T[length];

            for (int i = 0; i < length; i++)
            {
                target[i] = source[offset + i];
            }

            return target;
        }
    }
}

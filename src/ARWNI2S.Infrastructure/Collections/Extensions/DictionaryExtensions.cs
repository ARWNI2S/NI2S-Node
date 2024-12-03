using System.Collections;

namespace ARWNI2S.Extensions
{
    public static class DictionaryExtensions
    {
        #region Predicate operations

        /// <summary>
        /// Creates a delegate that converts keys to values by used a dictionary to map values. Keys
        /// that a not present in the dictionary are converted to the default value (zero or null).
        /// </summary>
        /// <remarks>This delegate can be used as a parameter in Convert or ConvertAll methods to convert
        /// entire collections.</remarks>
        /// <param name="dictionary">The dictionary used to perform the conversion.</param>
        /// <returns>A delegate to a method that converts keys to values. </returns>
        public static Converter<TKey, TValue> GetDictionaryConverter<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return GetDictionaryConverter(dictionary, default);
        }

        /// <summary>
        /// Creates a delegate that converts keys to values by used a dictionary to map values. Keys
        /// that a not present in the dictionary are converted to a supplied default value.
        /// </summary>
        /// <remarks>This delegate can be used as a parameter in Convert or ConvertAll methods to convert
        /// entire collections.</remarks>
        /// <param name="dictionary">The dictionary used to perform the conversion.</param>
        /// <param name="defaultValue">The result of the conversion for keys that are not present in the dictionary.</param>
        /// <returns>A delegate to a method that converts keys to values. </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
        public static Converter<TKey, TValue> GetDictionaryConverter<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue defaultValue)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            return delegate (TKey key)
            {
                if (dictionary.TryGetValue(key, out TValue value))
                    return value;
                else
                    return defaultValue;
            };
        }

        #endregion

        #region string representations (not yet coded)

        /// <summary>
        /// Gets a string representation of the mappings in a dictionary.
        /// The string representation starts with "{", has a list of mappings separated
        /// by commas (", "), and ends with "}". Each mapping is represented
        /// by "key->value". Each key and value in the dictionary is 
        /// converted to a string by calling its ToString method (null is represented by "null").
        /// Contained collections (except strings) are recursively converted to strings by this method.
        /// </summary>
        /// <param name="dictionary">A dictionary to get the string representation of.</param>
        /// <returns>The string representation of the collection, or "null" 
        /// if <paramref name="dictionary"/> is null.</returns>
        public static string ToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            bool firstItem = true;

            if (dictionary == null)
                return "null";

            System.Text.StringBuilder builder = new();

            builder.Append('{');

            // Call ToString on each item and put it in.
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (!firstItem)
                    builder.Append(", ");

                if (pair.Key == null)
                    builder.Append("null");
                else if (pair.Key is IEnumerable && !(pair.Key is string))
                    builder.Append(((IEnumerable)pair.Key).TypedAs<object>().ToString(true, "{", ",", "}"));
                else
                    builder.Append(pair.Key.ToString());

                builder.Append("->");

                if (pair.Value == null)
                    builder.Append("null");
                else if (pair.Value is IEnumerable && !(pair.Value is string))
                    builder.Append(((IEnumerable)pair.Value).TypedAs<object>().ToString(true, "{", ",", "}"));
                else
                    builder.Append(pair.Value.ToString());


                firstItem = false;
            }

            builder.Append('}');
            return builder.ToString();
        }

        #endregion string representations
    }
}

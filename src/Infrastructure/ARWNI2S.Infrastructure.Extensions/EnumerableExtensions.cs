﻿using System.Text;

namespace ARWNI2S.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns a human-readable text string that describes an IEnumerable collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of the list elements.</typeparam>
        /// <param name="collection">The IEnumerable to describe.</param>
        /// <param name="toString"></param>
        /// <param name="separator"></param>
        /// <param name="putInBrackets"></param>
        /// <returns>A string assembled by wrapping the string descriptions of the individual
        /// elements with square brackets and separating them with commas.</returns>
        public static string EnumerableToString<T>(this IEnumerable<T> collection, Func<T, string> toString = null,
                                                        string separator = ", ", bool putInBrackets = true)
        {
            if (collection == null)
            {
                if (putInBrackets) return "[]";
                else return "null";
            }
            var sb = new StringBuilder();
            if (putInBrackets) sb.Append("[");
            var enumerator = collection.GetEnumerator();
            bool firstDone = false;
            while (enumerator.MoveNext())
            {
                T value = enumerator.Current;
                string val;
                if (toString != null)
                    val = toString(value);
                else
                    val = value == null ? "null" : value.ToString();

                if (firstDone)
                {
                    sb.Append(separator);
                    sb.Append(val);
                }
                else
                {
                    sb.Append(val);
                    firstDone = true;
                }
            }
            if (putInBrackets) sb.Append("]");
            return sb.ToString();
        }
    }
}

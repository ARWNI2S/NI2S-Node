
using ARWNI2S.Collections.Internals;
using ARWNI2S.Resources;

namespace ARWNI2S.Extensions
{
    public static class GenericCollectionExtensions
    {
        /// <summary>
        /// Returns a read-only view onto a collection. The returned ICollection&lt;T&gt; interface
        /// only allows operations that do not change the collection: GetEnumerator, Contains, CopyTo,
        /// Count. The ReadOnly property returns false, indicating that the collection is read-only. All other
        /// methods on the interface throw a NotSupportedException.
        /// </summary>
        /// <remarks>The data in the underlying collection is not copied. If the underlying
        /// collection is changed, then the read-only view also changes accordingly.</remarks>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="collection">The collection to wrap.</param>
        /// <returns>A read-only view onto <paramref name="collection"/>. If <paramref name="collection"/> is null, then null is returned.</returns>
        public static ICollection<T> ReadOnly<T>(this ICollection<T> collection)
        {
            if (collection == null)
                return null;
            else
                return new ReadOnlyCollection<T>(collection);
        }

        #region Predicate operations

        /// <summary>
        /// Removes all the items in the collection that satisfy the condition
        /// defined by <paramref name="predicate"/>.
        /// </summary>
        /// <remarks>If the collection if an array or implements IList&lt;T&gt;, an efficient algorithm that
        /// compacts items is used. If not, then ICollection&lt;T&gt;.Remove is used
        /// to remove items from the collection. If the collection is an array or fixed-size list,
        /// the non-removed elements are placed, in order, at the beginning of
        /// the list, and the remaining list items are filled with a default value (0 or null).</remarks>
        /// <param name="collection">The collection to check all the items in.</param>
        /// <param name="predicate">A delegate that defines the condition to check for.</param>
        /// <returns>Returns a collection of the items that were removed. This collection contains the
        /// items in the same order that they orginally appeared in <paramref name="collection"/>.</returns>
        public static ICollection<T> RemoveWhere<T>(this ICollection<T> collection, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(predicate);
            if (collection is T[])
                collection = new ArrayWrapper<T>((T[])collection);
            if (collection.IsReadOnly)
                throw new ArgumentException(LocalizedStrings.Collections_ListIsReadOnly, nameof(collection));

            if (collection is IList<T> list)
            {
                T item;
                int i = -1, j = 0;
                int listCount = list.Count;
                List<T> removed = [];

                // Remove item where predicate is true, compressing items to lower in the list. This is much more
                // efficient than the naive algorithm that uses IList<T>.Remove().
                while (j < listCount)
                {
                    item = list[j];
                    if (predicate(item))
                    {
                        removed.Add(item);
                    }
                    else
                    {
                        ++i;
                        if (i != j)
                            list[i] = item;
                    }
                    ++j;
                }

                ++i;
                if (i < listCount)
                {
                    // remove items from the end.
                    if (list is IList && ((IList)list).IsFixedSize)
                    {
                        // An array or similar. Null out the last elements.
                        while (i < listCount)
                            list[i++] = default;
                    }
                    else
                    {
                        // Normal list.
                        while (i < listCount)
                        {
                            list.RemoveAt(listCount - 1);
                            --listCount;
                        }
                    }
                }

                return removed;
            }
            else
            {
                // We have to copy all the items to remove to a List, because collections can't be modifed 
                // during an enumeration.
                List<T> removed = [];

                foreach (T item in collection)
                    if (predicate(item))
                        removed.Add(item);

                foreach (T item in removed)
                    collection.Remove(item);

                return removed;
            }
        }

        #endregion

    }
}

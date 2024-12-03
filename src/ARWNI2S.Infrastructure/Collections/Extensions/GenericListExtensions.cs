using ARWNI2S.Resources;
using ARWNI2S.Runtime;

namespace ARWNI2S.Extensions
{
    public static class GenericListExtensions
    {
        #region Find and SearchForSubsequence

        /// <summary>
        /// Finds the index of the first item in a list that satisfies the condition
        /// defined by <paramref name="predicate"/>.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="predicate">A delegate that defined the condition to check for.</param>
        /// <returns>The index of the first item satisfying the condition. -1 if no such item exists in the list.</returns>
        public static int FindFirstIndexWhere<T>(this IList<T> list, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(predicate);

            int index = 0;
            foreach (T item in list)
            {
                if (predicate(item))
                {
                    return index;
                }
                ++index;
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Finds the index of the last item in a list that satisfies the condition
        /// defined by <paramref name="predicate"/>.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="predicate">A delegate that defined the condition to check for.</param>
        /// <returns>The index of the last item satisfying the condition. -1 if no such item exists in the list.</returns>
        public static int FindLastIndexWhere<T>(this IList<T> list, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(predicate);

            for (int index = list.Count - 1; index >= 0; --index)
            {
                if (predicate(list[index]))
                {
                    return index;
                }
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Enumerates the indices of all the items in <paramref name="list"/> that satisfy the condition defined
        /// by <paramref name="predicate"/>.
        /// </summary>
        /// <param name="list">The list to check all the items in.</param>
        /// <param name="predicate">A delegate that defines the condition to check for.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items that satisfy the condition.</returns>
        public static IEnumerable<int> FindIndicesWhere<T>(this IList<T> list, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(predicate);

            int index = 0;
            foreach (T item in list)
            {
                if (predicate(item))
                {
                    yield return index;
                }
                ++index;
            }
        }

        /// <summary>
        /// Finds the index of the first item in a list equal to a given item.
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <returns>The index of the first item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        public static int FirstIndexOf<T>(this IList<T> list, T item)
        {
            return FirstIndexOf(list, item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Finds the index of the first item in a list equal to a given item. A passed
        /// IEqualityComparer is used to determine equality.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        /// <returns>The index of the first item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        public static int FirstIndexOf<T>(this IList<T> list, T item, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(equalityComparer);

            int index = 0;
            foreach (T x in list)
            {
                if (equalityComparer.Equals(x, item))
                {
                    return index;
                }
                ++index;
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Finds the index of the last item in a list equal to a given item.
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <returns>The index of the last item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        public static int LastIndexOf<T>(this IList<T> list, T item)
        {
            return LastIndexOf(list, item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Finds the index of the last item in a list equal to a given item. A passed
        /// IEqualityComparer is used to determine equality.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        /// <returns>The index of the last item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        public static int LastIndexOf<T>(this IList<T> list, T item, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(equalityComparer);

            for (int index = list.Count - 1; index >= 0; --index)
            {
                if (equalityComparer.Equals(list[index], item))
                {
                    return index;
                }
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Enumerates the indices of all the items in a list equal to a given item.
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to <paramref name="item"/>. </returns>
        public static IEnumerable<int> IndicesOf<T>(this IList<T> list, T item)
        {
            return IndicesOf(list, item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Enumerates the indices of all the items in a list equal to a given item. A passed
        /// IEqualityComparer is used to determine equality.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to <paramref name="item"/>. </returns>
        public static IEnumerable<int> IndicesOf<T>(this IList<T> list, T item, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(equalityComparer);

            int index = 0;
            foreach (T x in list)
            {
                if (equalityComparer.Equals(x, item))
                {
                    yield return index;
                }
                ++index;
            }
        }

        /// <summary>
        /// Finds the index of the first item in a list equal to one of several given items.
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">The items to search for.</param>
        /// <returns>The index of the first item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        /// -1 if no such item exists in the list.</returns>
        public static int FirstIndexOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor)
        {
            return FirstIndexOfMany(list, itemsToLookFor, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Finds the index of the first item in a list equal to one of several given items. A passed
        /// IEqualityComparer is used to determine equality.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">The items to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. 
        /// Only the Equals and GetHashCode methods will be called.</param>
        /// <returns>The index of the first item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        /// -1 if no such item exists in the list.</returns>
        public static int FirstIndexOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(itemsToLookFor);
            ArgumentNullException.ThrowIfNull(equalityComparer);

            // Create a set of the items we are looking for, for efficient lookup.
            Set<T> setToLookFor = new(itemsToLookFor, equalityComparer);

            // Scan the list for the items.
            int index = 0;
            foreach (T x in list)
            {
                if (setToLookFor.Contains(x))
                {
                    return index;
                }
                ++index;
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Finds the index of the first item in a list "equal" to one of several given items. The passed 
        /// BinaryPredicate is used to determine if two items are "equal".
        /// </summary>
        /// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being removed need not be true equality. This methods finds 
        /// first item X which satisfies BinaryPredicate(X,Y), where Y is one of the items in <paramref name="itemsToLookFor"/></remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">The items to search for.</param>
        /// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        /// <returns>The index of the first item "equal" to any of the items in the collection <paramref name="itemsToLookFor"/>, using 
        /// <paramtype name="BinaryPredicate{T}"/> as the test for equality. 
        /// -1 if no such item exists in the list.</returns>
        public static int FirstIndexOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor, BinaryPredicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(itemsToLookFor);
            ArgumentNullException.ThrowIfNull(predicate);

            // Scan the list for the items.
            int index = 0;
            foreach (T x in list)
            {
                foreach (T y in itemsToLookFor)
                {
                    if (predicate(x, y))
                    {
                        return index;
                    }
                }

                ++index;
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Finds the index of the last item in a list equal to one of several given items.
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">The items to search for.</param>
        /// <returns>The index of the last item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        /// -1 if no such item exists in the list.</returns>
        public static int LastIndexOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor)
        {
            return LastIndexOfMany(list, itemsToLookFor, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Finds the index of the last item in a list equal to one of several given items. A passed
        /// IEqualityComparer is used to determine equality.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">The items to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality.</param>
        /// <returns>The index of the last item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        /// -1 if no such item exists in the list.</returns>
        public static int LastIndexOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(itemsToLookFor);
            ArgumentNullException.ThrowIfNull(equalityComparer);

            // Create a set of the items we are looking for, for efficient lookup.
            Set<T> setToLookFor = new(itemsToLookFor, equalityComparer);

            // Scan the list
            for (int index = list.Count - 1; index >= 0; --index)
            {
                if (setToLookFor.Contains(list[index]))
                {
                    return index;
                }
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Finds the index of the last item in a list "equal" to one of several given items. The passed 
        /// BinaryPredicate is used to determine if two items are "equal".
        /// </summary>
        /// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being removed need not be true equality. This methods finds 
        /// last item X which satisfies BinaryPredicate(X,Y), where Y is one of the items in <paramref name="itemsToLookFor"/></remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">The items to search for.</param>
        /// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        /// <returns>The index of the last item "equal" to any of the items in the collection <paramref name="itemsToLookFor"/>, using 
        /// <paramtype name="BinaryPredicate"/> as the test for equality. 
        /// -1 if no such item exists in the list.</returns>
        public static int LastIndexOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor, BinaryPredicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(itemsToLookFor);
            ArgumentNullException.ThrowIfNull(predicate);

            // Scan the list
            for (int index = list.Count - 1; index >= 0; --index)
            {
                foreach (T y in itemsToLookFor)
                {
                    if (predicate(list[index], y))
                    {
                        return index;
                    }
                }
            }

            // didn't find any item that matches.
            return -1;
        }

        /// <summary>
        /// Enumerates the indices of all the items in a list equal to one of several given items. 
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">A collection of items to search for.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to
        /// any of the items in the collection <paramref name="itemsToLookFor"/>. </returns>
        public static IEnumerable<int> IndicesOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor)
        {
            return IndicesOfMany(list, itemsToLookFor, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Enumerates the indices of all the items in a list equal to one of several given items. A passed
        /// IEqualityComparer is used to determine equality.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">A collection of items to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. </param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to
        /// any of the items in the collection <paramref name="itemsToLookFor"/>. </returns>
        public static IEnumerable<int> IndicesOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(itemsToLookFor);
            ArgumentNullException.ThrowIfNull(equalityComparer);

            // Create a set of the items we are looking for, for efficient lookup.
            Set<T> setToLookFor = new(itemsToLookFor, equalityComparer);

            // Scan the list
            int index = 0;
            foreach (T x in list)
            {
                if (setToLookFor.Contains(x))
                {
                    yield return index;
                }
                ++index;
            }
        }

        /// <summary>
        /// Enumerates the indices of all the items in a list equal to one of several given items. The passed 
        /// BinaryPredicate is used to determine if two items are "equal".
        /// </summary>
        /// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being removed need not be true equality. This methods finds 
        /// last item X which satisfies BinaryPredicate(X,Y), where Y is one of the items in <paramref name="itemsToLookFor"/></remarks>
        /// <param name="list">The list to search.</param>
        /// <param name="itemsToLookFor">A collection of items to search for.</param>
        /// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items "equal" to any of the items 
        /// in the collection <paramref name="itemsToLookFor"/>, using 
        /// <paramtest name="BinaryPredicate"/> as the test for equality. </returns>
        public static IEnumerable<int> IndicesOfMany<T>(this IList<T> list, IEnumerable<T> itemsToLookFor, BinaryPredicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(itemsToLookFor);
            ArgumentNullException.ThrowIfNull(predicate);

            // Scan the list for the items.
            int index = 0;
            foreach (T x in list)
            {
                foreach (T y in itemsToLookFor)
                {
                    if (predicate(x, y))
                    {
                        yield return index;
                    }
                }

                ++index;
            }
        }

        /// <summary>
        /// Searchs a list for a sub-sequence of items that match a particular pattern. A subsequence 
        /// of <paramref name="list"/> matches pattern at index i if list[i] is equal to the first item
        /// in <paramref name="pattern"/>, list[i+1] is equal to the second item in <paramref name="pattern"/>,
        /// and so forth for all the items in <paramref name="pattern"/>.
        /// </summary>
        /// <remarks>The default sense of equality for T is used, as defined by T's
        /// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to search.</param>
        /// <param name="pattern">The sequence of items to search for.</param>
        /// <returns>The first index with <paramref name="list"/> that matches the items in <paramref name="pattern"/>.</returns>
        public static int SearchForSubsequence<T>(this IList<T> list, IEnumerable<T> pattern)
        {
            return SearchForSubsequence(list, pattern, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searchs a list for a sub-sequence of items that match a particular pattern. A subsequence 
        /// of <paramref name="list"/> matches pattern at index i if list[i] is "equal" to the first item
        /// in <paramref name="pattern"/>, list[i+1] is "equal" to the second item in <paramref name="pattern"/>,
        /// and so forth for all the items in <paramref name="pattern"/>. The passed 
        /// BinaryPredicate is used to determine if two items are "equal".
        /// </summary>
        /// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being tested
        /// for in the pattern need not be equality. </remarks>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to search.</param>
        /// <param name="pattern">The sequence of items to search for.</param>
        /// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        /// <returns>The first index with <paramref name="list"/> that matches the items in <paramref name="pattern"/>.</returns>
        public static int SearchForSubsequence<T>(this IList<T> list, IEnumerable<T> pattern, BinaryPredicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(pattern);
            ArgumentNullException.ThrowIfNull(predicate);

            // Put the pattern into an array for performance (don't keep allocating enumerators).
            T[] patternArray = ToArray(pattern);

            int listCount = list.Count, patternCount = patternArray.Length;
            if (patternCount == 0)
                return 0;              // A zero-length pattern occurs anywhere.
            if (listCount == 0)
                return -1;             // no room for a pattern;

            for (int start = 0; start <= listCount - patternCount; ++start)
            {
                for (int count = 0; count < patternCount; ++count)
                {
                    if (!predicate(list[start + count], patternArray[count]))
                        goto NOMATCH;
                }
                // Got through the whole pattern. We have a match.
                return start;

            NOMATCH:
                /* no match found at start. */
                ;
            }

            // no match found anywhere.
            return -1;
        }

        /// <summary>
        /// Searchs a list for a sub-sequence of items that match a particular pattern. A subsequence 
        /// of <paramref name="list"/> matches pattern at index i if list[i] is equal to the first item
        /// in <paramref name="pattern"/>, list[i+1] is equal to the second item in <paramref name="pattern"/>,
        /// and so forth for all the items in <paramref name="pattern"/>. The passed 
        /// instance of IEqualityComparer&lt;T&gt; is used for determining if two items are equal.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to search.</param>
        /// <param name="pattern">The sequence of items to search for.</param>
        /// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        /// <returns>The first index with <paramref name="list"/> that matches the items in <paramref name="pattern"/>.</returns>
        public static int SearchForSubsequence<T>(this IList<T> list, IEnumerable<T> pattern, IEqualityComparer<T> equalityComparer)
        {
            ArgumentNullException.ThrowIfNull(equalityComparer);

            return SearchForSubsequence(list, pattern, equalityComparer.Equals);
        }

        #endregion Find and SearchForSubsequence

        #region Predicate operations

        /// <summary>
        /// Partition a list or array based on a predicate. After partitioning, all items for which
        /// the predicate returned true precede all items for which the predicate returned false.
        /// </summary>
        /// <remarks>Although arrays cast to IList&lt;T&gt; are normally read-only, this method
        /// will work correctly and modify an array passed as <paramref name="list"/>.</remarks>
        /// <param name="list">The list or array to partition.</param>
        /// <param name="predicate">A delegate that defines the partitioning condition.</param>
        /// <returns>The index of the first item in the second half of the partition; i.e., the first item for
        /// which <paramref name="predicate"/> returned false. If the predicate was true for all items
        /// in the list, list.Count is returned.</returns>
        public static int Partition<T>(this IList<T> list, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(predicate);
            if (list is T[])
                list = new ArrayWrapper<T>((T[])list);
            if (list.IsReadOnly)
                throw new ArgumentException(LocalizedStrings.Collections_ListIsReadOnly, nameof(list));

            // Move from opposite ends of the list, swapping when necessary.
            int i = 0, j = list.Count - 1;
            for (; ; )
            {
                while (i <= j && predicate(list[i]))
                    ++i;
                while (i <= j && !predicate(list[j]))
                    --j;

                if (i > j)
                    break;
                else
                {
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                    ++i;
                    --j;
                }
            }

            return i;
        }

        /// <summary>
        /// Partition a list or array based on a predicate. After partitioning, all items for which
        /// the predicate returned true precede all items for which the predicate returned false. 
        /// The partition is stable, which means that if items X and Y have the same result from
        /// the predicate, and X precedes Y in the original list, X will precede Y in the 
        /// partitioned list.
        /// </summary>
        /// <remarks>Although arrays cast to IList&lt;T&gt; are normally read-only, this method
        /// will work correctly and modify an array passed as <paramref name="list"/>.</remarks>
        /// <param name="list">The list or array to partition.</param>
        /// <param name="predicate">A delegate that defines the partitioning condition.</param>
        /// <returns>The index of the first item in the second half of the partition; i.e., the first item for
        /// which <paramref name="predicate"/> returned false. If the predicate was true for all items
        /// in the list, list.Count is returned.</returns>
        public static int StablePartition<T>(this IList<T> list, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(predicate);
            if (list is T[])
                list = new ArrayWrapper<T>((T[])list);
            if (list.IsReadOnly)
                throw new ArgumentException(LocalizedStrings.Collections_ListIsReadOnly, nameof(list));

            int listCount = list.Count;
            if (listCount == 0)
                return 0;
            T[] temp = new T[listCount];

            // Copy from list to temp buffer, true items at fron, false item (in reverse order) at back.
            int i = 0, j = listCount - 1;
            foreach (T item in list)
            {
                if (predicate(item))
                    temp[i++] = item;
                else
                    temp[j--] = item;
            }

            // Copy back to the original list.
            int index = 0;
            while (index < i)
            {
                list[index] = temp[index];
                index++;
            }
            j = listCount - 1;
            while (index < listCount)
                list[index++] = temp[j--];

            return i;
        }


        #endregion
    }
}

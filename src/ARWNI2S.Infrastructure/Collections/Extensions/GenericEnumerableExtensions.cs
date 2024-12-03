using System.Text;

namespace ARWNI2S.Collections.Extensions
{
    public static class GenericEnumerableExtensions
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

        ///// <summary>
        ///// Determines if the two collections contain equal items in the same order. The two collections do not need
        ///// to be of the same type; it is permissible to compare an array and an OrderedBag, for instance.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <typeparam name="T">The type of items in the collections.</typeparam>
        ///// <param name="collection">The first collection to compare.</param>
        ///// <param name="other">The second collection to compare.</param>
        ///// <returns>True if the collections have equal items in the same order. If both collections are empty, true is returned.</returns>
        //public static bool EqualCollections<T>(this IEnumerable<T> collection, IEnumerable<T> other)
        //{
        //    return collection.EqualCollections(other, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Determines if the two collections contain equal items in the same order. The passed 
        ///// instance of IEqualityComparer&lt;T&gt; is used for determining if two items are equal.
        ///// </summary>
        ///// <typeparam name="T">The type of items in the collections.</typeparam>
        ///// <param name="collection">The first collection to compare.</param>
        ///// <param name="other">The second collection to compare.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. 
        ///// Only the Equals member function of this interface is called.</param>
        ///// <returns>True if the collections have equal items in the same order. If both collections are empty, true is returned.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="collection"/>, <paramref name="other"/>, or
        ///// <paramref name="equalityComparer"/> is null.</exception>
        //public static bool EqualCollections<T>(this IEnumerable<T> collection, IEnumerable<T> other, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(other);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    using (IEnumerator<T> enum1 = collection.GetEnumerator(), enum2 = other.GetEnumerator())
        //    {
        //        bool continue1, continue2;

        //        for (; ; )
        //        {
        //            continue1 = enum1.MoveNext(); continue2 = enum2.MoveNext();
        //            if (!continue1 || !continue2)
        //                break;

        //            if (!equalityComparer.Equals(enum1.Current, enum2.Current))
        //                return false;     // the two items are not equal.
        //        }

        //        // If both continue1 and continue2 are false, we reached the end of both sequences at the same
        //        // time and found success. If one is true and one is false, the sequences were of difference lengths -- failure.
        //        return continue1 == continue2;
        //    }
        //}

        ///// <summary>
        ///// Determines if the two collections contain "equal" items in the same order. The passed 
        ///// BinaryPredicate is used to determine if two items are "equal".
        ///// </summary>
        ///// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being tested
        ///// for need not be equality. For example, the following code determines if each integer in
        ///// list1 is less than or equal to the corresponding integer in list2.
        ///// <code>
        ///// List&lt;int&gt; list1, list2;
        ///// if (EqualCollections(list1, list2, delegate(int x, int y) { return x &lt;= y; }) {
        /////     // the check is true...
        ///// }
        ///// </code>
        ///// </remarks>
        ///// <typeparam name="T">The type of items in the collections.</typeparam>
        ///// <param name="collection">The first collection to compare.</param>
        ///// <param name="other">The second collection to compare.</param>
        ///// <param name="predicate">The BinaryPredicate used to compare items for "equality". 
        ///// This predicate can compute any relation between two items; it need not represent equality or an equivalence relation.</param>
        ///// <returns>True if <paramref name="predicate"/>returns true for each corresponding pair of
        ///// items in the two collections. If both collections are empty, true is returned.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="collection"/>, <paramref name="other"/>, or
        ///// <paramref name="predicate"/> is null.</exception>
        //public static bool EqualCollections<T>(this IEnumerable<T> collection, IEnumerable<T> other, BinaryPredicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(other);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    using (IEnumerator<T> enum1 = collection.GetEnumerator(), enum2 = other.GetEnumerator())
        //    {
        //        bool continue1, continue2;

        //        for (; ; )
        //        {
        //            continue1 = enum1.MoveNext(); continue2 = enum2.MoveNext();
        //            if (!continue1 || !continue2)
        //                break;

        //            if (!predicate(enum1.Current, enum2.Current))
        //                return false;     // the two items are not equal.
        //        }

        //        // If both continue1 and continue2 are false, we reached the end of both sequences at the same
        //        // time and found success. If one is true and one is false, the sequences were of difference lengths -- failure.
        //        return continue1 == continue2;
        //    }
        //}

        //#region Find and SearchForSubsequence


        ///// <summary>
        ///// Finds the first item in a collection that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <remarks>If the default value for T could be present in the collection, and 
        ///// would be matched by the predicate, then this method is inappropriate, because
        ///// you cannot disguish whether the default value for T was actually present in the collection,
        ///// or no items matched the predicate. In this case, use TryFindFirstWhere.</remarks>
        ///// <param name="collection">The collection to search.</param>
        ///// <param name="predicate">A delegate that defined the condition to check for.</param>
        ///// <returns>The first item in the collection that matches the condition, or the default value for T (0 or null) if no
        ///// item that matches the condition is found.</returns>
        ///// <seealso cref="TryFindFirstWhere{T}"/>
        //public static T FindFirstWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        //{
        //    if (TryFindFirstWhere(collection, predicate, out T retval))
        //        return retval;
        //    else
        //        return default;
        //}

        ///// <summary>
        ///// Finds the first item in a collection that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="collection">The collection to search.</param>
        ///// <param name="predicate">A delegate that defined the condition to check for.</param>
        ///// <param name="foundItem">Outputs the first item in the collection that matches the condition, if the method returns true.</param>
        ///// <returns>True if an item satisfying the condition was found. False if no such item exists in the collection.</returns>
        ///// <seealso cref="FindFirstWhere{T}"/>
        //public static bool TryFindFirstWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate, out T foundItem)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    foreach (T item in collection)
        //    {
        //        if (predicate(item))
        //        {
        //            foundItem = item;
        //            return true;
        //        }
        //    }

        //    // didn't find any item that matches.
        //    foundItem = default;
        //    return false;
        //}

        ///// <summary>
        ///// Finds the last item in a collection that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <remarks><para>If the collection implements IList&lt;T&gt;, then the list is scanned in reverse until a 
        ///// matching item is found. Otherwise, the entire collection is iterated in the forward direction.</para>
        ///// <para>If the default value for T could be present in the collection, and 
        ///// would be matched by the predicate, then this method is inappropriate, because
        ///// you cannot disguish whether the default value for T was actually present in the collection,
        ///// or no items matched the predicate. In this case, use TryFindFirstWhere.</para></remarks>
        ///// <param name="collection">The collection to search.</param>
        ///// <param name="predicate">A delegate that defined the condition to check for.</param>
        ///// <returns>The last item in the collection that matches the condition, or the default value for T (0 or null) if no
        ///// item that matches the condition is found.</returns>
        ///// <seealso cref="TryFindLastWhere{T}"/>
        //public static T FindLastWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        //{
        //    if (TryFindLastWhere(collection, predicate, out T retval))
        //        return retval;
        //    else
        //        return default;
        //}

        ///// <summary>
        ///// Finds the last item in a collection that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <remarks>If the collection implements IList&lt;T&gt;, then the list is scanned in reverse until a 
        ///// matching item is found. Otherwise, the entire collection is iterated in the forward direction.</remarks>
        ///// <param name="collection">The collection to search.</param>
        ///// <param name="predicate">A delegate that defined the condition to check for.</param>
        ///// <param name="foundItem">Outputs the last item in the collection that matches the condition, if the method returns true.</param>
        ///// <returns>True if an item satisfying the condition was found. False if no such item exists in the collection.</returns>
        ///// <seealso cref="FindLastWhere{T}"/>
        //public static bool TryFindLastWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate, out T foundItem)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    if (collection is IList<T> list)
        //    {
        //        // If it's a list, we can iterate in reverse.
        //        for (int index = list.Count - 1; index >= 0; --index)
        //        {
        //            T item = list[index];
        //            if (predicate(item))
        //            {
        //                foundItem = item;
        //                return true;
        //            }
        //        }

        //        // didn't find any item that matches.
        //        foundItem = default;
        //        return false;
        //    }
        //    else
        //    {
        //        // Otherwise, iterate the whole thing and remember the last matching one.
        //        bool found = false;
        //        foundItem = default;

        //        foreach (T item in collection)
        //        {
        //            if (predicate(item))
        //            {
        //                foundItem = item;
        //                found = true;
        //            }
        //        }

        //        return found;
        //    }
        //}

        ///// <summary>
        ///// Enumerates all the items in <paramref name="collection"/> that satisfy the condition defined
        ///// by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="collection">The collection to check all the items in.</param>
        ///// <param name="predicate">A delegate that defines the condition to check for.</param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the items that satisfy the condition.</returns>
        //public static IEnumerable<T> FindWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    foreach (T item in collection)
        //    {
        //        if (predicate(item))
        //        {
        //            yield return item;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Finds the index of the first item in a list that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="predicate">A delegate that defined the condition to check for.</param>
        ///// <returns>The index of the first item satisfying the condition. -1 if no such item exists in the list.</returns>
        //public static int FindFirstIndexWhere<T>(IList<T> list, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    int index = 0;
        //    foreach (T item in list)
        //    {
        //        if (predicate(item))
        //        {
        //            return index;
        //        }
        //        ++index;
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Finds the index of the last item in a list that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="predicate">A delegate that defined the condition to check for.</param>
        ///// <returns>The index of the last item satisfying the condition. -1 if no such item exists in the list.</returns>
        //public static int FindLastIndexWhere<T>(IList<T> list, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    for (int index = list.Count - 1; index >= 0; --index)
        //    {
        //        if (predicate(list[index]))
        //        {
        //            return index;
        //        }
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Enumerates the indices of all the items in <paramref name="list"/> that satisfy the condition defined
        ///// by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="list">The list to check all the items in.</param>
        ///// <param name="predicate">A delegate that defines the condition to check for.</param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items that satisfy the condition.</returns>
        //public static IEnumerable<int> FindIndicesWhere<T>(IList<T> list, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    int index = 0;
        //    foreach (T item in list)
        //    {
        //        if (predicate(item))
        //        {
        //            yield return index;
        //        }
        //        ++index;
        //    }
        //}

        ///// <summary>
        ///// Finds the index of the first item in a list equal to a given item.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="item">The item to search for.</param>
        ///// <returns>The index of the first item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        //public static int FirstIndexOf<T>(IList<T> list, T item)
        //{
        //    return FirstIndexOf(list, item, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Finds the index of the first item in a list equal to a given item. A passed
        ///// IEqualityComparer is used to determine equality.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="item">The item to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        ///// <returns>The index of the first item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        //public static int FirstIndexOf<T>(IList<T> list, T item, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    int index = 0;
        //    foreach (T x in list)
        //    {
        //        if (equalityComparer.Equals(x, item))
        //        {
        //            return index;
        //        }
        //        ++index;
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Finds the index of the last item in a list equal to a given item.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="item">The item to search for.</param>
        ///// <returns>The index of the last item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        //public static int LastIndexOf<T>(IList<T> list, T item)
        //{
        //    return LastIndexOf(list, item, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Finds the index of the last item in a list equal to a given item. A passed
        ///// IEqualityComparer is used to determine equality.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="item">The item to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        ///// <returns>The index of the last item equal to <paramref name="item"/>. -1 if no such item exists in the list.</returns>
        //public static int LastIndexOf<T>(IList<T> list, T item, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    for (int index = list.Count - 1; index >= 0; --index)
        //    {
        //        if (equalityComparer.Equals(list[index], item))
        //        {
        //            return index;
        //        }
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Enumerates the indices of all the items in a list equal to a given item.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="item">The item to search for.</param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to <paramref name="item"/>. </returns>
        //public static IEnumerable<int> IndicesOf<T>(IList<T> list, T item)
        //{
        //    return IndicesOf(list, item, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Enumerates the indices of all the items in a list equal to a given item. A passed
        ///// IEqualityComparer is used to determine equality.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="item">The item to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to <paramref name="item"/>. </returns>
        //public static IEnumerable<int> IndicesOf<T>(IList<T> list, T item, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    int index = 0;
        //    foreach (T x in list)
        //    {
        //        if (equalityComparer.Equals(x, item))
        //        {
        //            yield return index;
        //        }
        //        ++index;
        //    }
        //}

        ///// <summary>
        ///// Finds the index of the first item in a list equal to one of several given items.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">The items to search for.</param>
        ///// <returns>The index of the first item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        ///// -1 if no such item exists in the list.</returns>
        //public static int FirstIndexOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor)
        //{
        //    return FirstIndexOfMany(list, itemsToLookFor, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Finds the index of the first item in a list equal to one of several given items. A passed
        ///// IEqualityComparer is used to determine equality.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">The items to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. 
        ///// Only the Equals and GetHashCode methods will be called.</param>
        ///// <returns>The index of the first item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        ///// -1 if no such item exists in the list.</returns>
        //public static int FirstIndexOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(itemsToLookFor);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    // Create a set of the items we are looking for, for efficient lookup.
        //    Set<T> setToLookFor = new(itemsToLookFor, equalityComparer);

        //    // Scan the list for the items.
        //    int index = 0;
        //    foreach (T x in list)
        //    {
        //        if (setToLookFor.Contains(x))
        //        {
        //            return index;
        //        }
        //        ++index;
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Finds the index of the first item in a list "equal" to one of several given items. The passed 
        ///// BinaryPredicate is used to determine if two items are "equal".
        ///// </summary>
        ///// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being removed need not be true equality. This methods finds 
        ///// first item X which satisfies BinaryPredicate(X,Y), where Y is one of the items in <paramref name="itemsToLookFor"/></remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">The items to search for.</param>
        ///// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        ///// <returns>The index of the first item "equal" to any of the items in the collection <paramref name="itemsToLookFor"/>, using 
        ///// <paramtype name="BinaryPredicate{T}"/> as the test for equality. 
        ///// -1 if no such item exists in the list.</returns>
        //public static int FirstIndexOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor, BinaryPredicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(itemsToLookFor);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    // Scan the list for the items.
        //    int index = 0;
        //    foreach (T x in list)
        //    {
        //        foreach (T y in itemsToLookFor)
        //        {
        //            if (predicate(x, y))
        //            {
        //                return index;
        //            }
        //        }

        //        ++index;
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Finds the index of the last item in a list equal to one of several given items.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">The items to search for.</param>
        ///// <returns>The index of the last item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        ///// -1 if no such item exists in the list.</returns>
        //public static int LastIndexOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor)
        //{
        //    return LastIndexOfMany(list, itemsToLookFor, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Finds the index of the last item in a list equal to one of several given items. A passed
        ///// IEqualityComparer is used to determine equality.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">The items to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality.</param>
        ///// <returns>The index of the last item equal to any of the items in the collection <paramref name="itemsToLookFor"/>. 
        ///// -1 if no such item exists in the list.</returns>
        //public static int LastIndexOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(itemsToLookFor);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    // Create a set of the items we are looking for, for efficient lookup.
        //    Set<T> setToLookFor = new(itemsToLookFor, equalityComparer);

        //    // Scan the list
        //    for (int index = list.Count - 1; index >= 0; --index)
        //    {
        //        if (setToLookFor.Contains(list[index]))
        //        {
        //            return index;
        //        }
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Finds the index of the last item in a list "equal" to one of several given items. The passed 
        ///// BinaryPredicate is used to determine if two items are "equal".
        ///// </summary>
        ///// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being removed need not be true equality. This methods finds 
        ///// last item X which satisfies BinaryPredicate(X,Y), where Y is one of the items in <paramref name="itemsToLookFor"/></remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">The items to search for.</param>
        ///// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        ///// <returns>The index of the last item "equal" to any of the items in the collection <paramref name="itemsToLookFor"/>, using 
        ///// <paramtype name="BinaryPredicate"/> as the test for equality. 
        ///// -1 if no such item exists in the list.</returns>
        //public static int LastIndexOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor, BinaryPredicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(itemsToLookFor);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    // Scan the list
        //    for (int index = list.Count - 1; index >= 0; --index)
        //    {
        //        foreach (T y in itemsToLookFor)
        //        {
        //            if (predicate(list[index], y))
        //            {
        //                return index;
        //            }
        //        }
        //    }

        //    // didn't find any item that matches.
        //    return -1;
        //}

        ///// <summary>
        ///// Enumerates the indices of all the items in a list equal to one of several given items. 
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">A collection of items to search for.</param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to
        ///// any of the items in the collection <paramref name="itemsToLookFor"/>. </returns>
        //public static IEnumerable<int> IndicesOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor)
        //{
        //    return IndicesOfMany(list, itemsToLookFor, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Enumerates the indices of all the items in a list equal to one of several given items. A passed
        ///// IEqualityComparer is used to determine equality.
        ///// </summary>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">A collection of items to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. </param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items equal to
        ///// any of the items in the collection <paramref name="itemsToLookFor"/>. </returns>
        //public static IEnumerable<int> IndicesOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(itemsToLookFor);
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    // Create a set of the items we are looking for, for efficient lookup.
        //    Set<T> setToLookFor = new(itemsToLookFor, equalityComparer);

        //    // Scan the list
        //    int index = 0;
        //    foreach (T x in list)
        //    {
        //        if (setToLookFor.Contains(x))
        //        {
        //            yield return index;
        //        }
        //        ++index;
        //    }
        //}

        ///// <summary>
        ///// Enumerates the indices of all the items in a list equal to one of several given items. The passed 
        ///// BinaryPredicate is used to determine if two items are "equal".
        ///// </summary>
        ///// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being removed need not be true equality. This methods finds 
        ///// last item X which satisfies BinaryPredicate(X,Y), where Y is one of the items in <paramref name="itemsToLookFor"/></remarks>
        ///// <param name="list">The list to search.</param>
        ///// <param name="itemsToLookFor">A collection of items to search for.</param>
        ///// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        ///// <returns>An IEnumerable&lt;T&gt; that enumerates the indices of items "equal" to any of the items 
        ///// in the collection <paramref name="itemsToLookFor"/>, using 
        ///// <paramtest name="BinaryPredicate"/> as the test for equality. </returns>
        //public static IEnumerable<int> IndicesOfMany<T>(IList<T> list, IEnumerable<T> itemsToLookFor, BinaryPredicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(itemsToLookFor);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    // Scan the list for the items.
        //    int index = 0;
        //    foreach (T x in list)
        //    {
        //        foreach (T y in itemsToLookFor)
        //        {
        //            if (predicate(x, y))
        //            {
        //                yield return index;
        //            }
        //        }

        //        ++index;
        //    }
        //}

        ///// <summary>
        ///// Searchs a list for a sub-sequence of items that match a particular pattern. A subsequence 
        ///// of <paramref name="list"/> matches pattern at index i if list[i] is equal to the first item
        ///// in <paramref name="pattern"/>, list[i+1] is equal to the second item in <paramref name="pattern"/>,
        ///// and so forth for all the items in <paramref name="pattern"/>.
        ///// </summary>
        ///// <remarks>The default sense of equality for T is used, as defined by T's
        ///// implementation of IComparable&lt;T&gt;.Equals or object.Equals.</remarks>
        ///// <typeparam name="T">The type of items in the list.</typeparam>
        ///// <param name="list">The list to search.</param>
        ///// <param name="pattern">The sequence of items to search for.</param>
        ///// <returns>The first index with <paramref name="list"/> that matches the items in <paramref name="pattern"/>.</returns>
        //public static int SearchForSubsequence<T>(IList<T> list, IEnumerable<T> pattern)
        //{
        //    return SearchForSubsequence(list, pattern, EqualityComparer<T>.Default);
        //}

        ///// <summary>
        ///// Searchs a list for a sub-sequence of items that match a particular pattern. A subsequence 
        ///// of <paramref name="list"/> matches pattern at index i if list[i] is "equal" to the first item
        ///// in <paramref name="pattern"/>, list[i+1] is "equal" to the second item in <paramref name="pattern"/>,
        ///// and so forth for all the items in <paramref name="pattern"/>. The passed 
        ///// BinaryPredicate is used to determine if two items are "equal".
        ///// </summary>
        ///// <remarks>Since an arbitrary BinaryPredicate is passed to this function, what is being tested
        ///// for in the pattern need not be equality. </remarks>
        ///// <typeparam name="T">The type of items in the list.</typeparam>
        ///// <param name="list">The list to search.</param>
        ///// <param name="pattern">The sequence of items to search for.</param>
        ///// <param name="predicate">The BinaryPredicate used to compare items for "equality". </param>
        ///// <returns>The first index with <paramref name="list"/> that matches the items in <paramref name="pattern"/>.</returns>
        //public static int SearchForSubsequence<T>(IList<T> list, IEnumerable<T> pattern, BinaryPredicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(list);
        //    ArgumentNullException.ThrowIfNull(pattern);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    // Put the pattern into an array for performance (don't keep allocating enumerators).
        //    T[] patternArray = ToArray(pattern);

        //    int listCount = list.Count, patternCount = patternArray.Length;
        //    if (patternCount == 0)
        //        return 0;              // A zero-length pattern occurs anywhere.
        //    if (listCount == 0)
        //        return -1;             // no room for a pattern;

        //    for (int start = 0; start <= listCount - patternCount; ++start)
        //    {
        //        for (int count = 0; count < patternCount; ++count)
        //        {
        //            if (!predicate(list[start + count], patternArray[count]))
        //                goto NOMATCH;
        //        }
        //        // Got through the whole pattern. We have a match.
        //        return start;

        //    NOMATCH:
        //        /* no match found at start. */
        //        ;
        //    }

        //    // no match found anywhere.
        //    return -1;
        //}

        ///// <summary>
        ///// Searchs a list for a sub-sequence of items that match a particular pattern. A subsequence 
        ///// of <paramref name="list"/> matches pattern at index i if list[i] is equal to the first item
        ///// in <paramref name="pattern"/>, list[i+1] is equal to the second item in <paramref name="pattern"/>,
        ///// and so forth for all the items in <paramref name="pattern"/>. The passed 
        ///// instance of IEqualityComparer&lt;T&gt; is used for determining if two items are equal.
        ///// </summary>
        ///// <typeparam name="T">The type of items in the list.</typeparam>
        ///// <param name="list">The list to search.</param>
        ///// <param name="pattern">The sequence of items to search for.</param>
        ///// <param name="equalityComparer">The IEqualityComparer&lt;T&gt; used to compare items for equality. Only the Equals method will be called.</param>
        ///// <returns>The first index with <paramref name="list"/> that matches the items in <paramref name="pattern"/>.</returns>
        //public static int SearchForSubsequence<T>(IList<T> list, IEnumerable<T> pattern, IEqualityComparer<T> equalityComparer)
        //{
        //    ArgumentNullException.ThrowIfNull(equalityComparer);

        //    return SearchForSubsequence(list, pattern, equalityComparer.Equals);
        //}

        //#endregion Find and SearchForSubsequence


        //#region Predicate operations

        ///// <summary>
        ///// Determines if a collection contains any item that satisfies the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="collection">The collection to check all the items in.</param>
        ///// <param name="predicate">A delegate that defines the condition to check for.</param>
        ///// <returns>True if the collection contains one or more items that satisfy the condition
        ///// defined by <paramref name="predicate"/>. False if the collection does not contain
        ///// an item that satisfies <paramref name="predicate"/>.</returns>
        //public static bool Exists<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    foreach (T item in collection)
        //    {
        //        if (predicate(item))
        //            return true;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Determines if all of the items in the collection satisfy the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="collection">The collection to check all the items in.</param>
        ///// <param name="predicate">A delegate that defines the condition to check for.</param>
        ///// <returns>True if all of the items in the collection satisfy the condition
        ///// defined by <paramref name="predicate"/>, or if the collection is empty. False if one or more items
        ///// in the collection do not satisfy <paramref name="predicate"/>.</returns>
        //public static bool TrueForAll<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    foreach (T item in collection)
        //    {
        //        if (!predicate(item))
        //            return false;
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// Counts the number of items in the collection that satisfy the condition
        ///// defined by <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="collection">The collection to count items in.</param>
        ///// <param name="predicate">A delegate that defines the condition to check for.</param>
        ///// <returns>The number of items in the collection that satisfy <paramref name="predicate"/>.</returns>
        //public static int CountWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(predicate);

        //    int count = 0;
        //    foreach (T item in collection)
        //    {
        //        if (predicate(item))
        //            ++count;
        //    }

        //    return count;
        //}

        ///// <summary>
        ///// Convert a collection of items by applying a delegate to each item in the collection. The resulting collection
        ///// contains the result of applying <paramref name="converter"/> to each item in <paramref name="sourceCollection"/>, in
        ///// order.
        ///// </summary>
        ///// <typeparam name="TSource">The type of items in the collection to convert.</typeparam>
        ///// <typeparam name="TDest">The type each item is being converted to.</typeparam>
        ///// <param name="sourceCollection">The collection of item being converted.</param>
        ///// <param name="converter">A delegate to the method to call, passing each item in <paramref name="sourceCollection"/>.</param>
        ///// <returns>The resulting collection from applying <paramref name="converter"/> to each item in <paramref name="sourceCollection"/>, in
        ///// order.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="sourceCollection"/> or <paramref name="converter"/> is null.</exception>
        //public static IEnumerable<TDest> Convert<TSource, TDest>(this IEnumerable<TSource> sourceCollection, Converter<TSource, TDest> converter)
        //{
        //    ArgumentNullException.ThrowIfNull(sourceCollection);
        //    ArgumentNullException.ThrowIfNull(converter);

        //    foreach (TSource sourceItem in sourceCollection)
        //        yield return converter(sourceItem);
        //}

        ///// <summary>
        ///// Performs the specified action on each item in a collection.
        ///// </summary>
        ///// <param name="collection">The collection to process.</param>
        ///// <param name="action">An Action delegate which is invoked for each item in <paramref name="collection"/>.</param>
        //public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        //{
        //    ArgumentNullException.ThrowIfNull(collection);
        //    ArgumentNullException.ThrowIfNull(action);

        //    foreach (T item in collection)
        //        action(item);
        //}

        //#endregion Predicate operations

        //#region string representations (not yet coded)

        ///// <summary>
        ///// Gets a string representation of the elements in the collection.
        ///// The string representation starts with "{", has a list of items separated
        ///// by commas (","), and ends with "}". Each item in the collection is 
        ///// converted to a string by calling its ToString method (null is represented by "null").
        ///// Contained collections (except strings) are recursively converted to strings by this method.
        ///// </summary>
        ///// <param name="collection">A collection to get the string representation of.</param>
        ///// <returns>The string representation of the collection. If <paramref name="collection"/> is null, then the string "null" is returned.</returns>
        //public static string ToString<T>(this IEnumerable<T> collection)
        //{
        //    return ToString(collection, true, "{", ",", "}");
        //}

        ///// <summary>
        ///// Gets a string representation of the elements in the collection.
        ///// The string to used at the beginning and end, and to separate items,
        ///// and supplied by parameters. Each item in the collection is 
        ///// converted to a string by calling its ToString method (null is represented by "null").
        ///// </summary>
        ///// <param name="collection">A collection to get the string representation of.</param>
        ///// <param name="recursive">If true, contained collections (except strings) are converted to strings by a recursive call to this method, instead
        ///// of by calling ToString.</param>
        ///// <param name="start">The string to appear at the beginning of the output string.</param>
        ///// <param name="separator">The string to appear between each item in the string.</param>
        ///// <param name="end">The string to appear at the end of the output string.</param>
        ///// <returns>The string representation of the collection. If <paramref name="collection"/> is null, then the string "null" is returned.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="start"/>, <paramref name="separator"/>, or <paramref name="end"/>
        /////  is null.</exception>
        //public static string ToString<T>(this IEnumerable<T> collection, bool recursive, string start, string separator, string end)
        //{
        //    ArgumentNullException.ThrowIfNull(start);
        //    ArgumentNullException.ThrowIfNull(separator);
        //    ArgumentNullException.ThrowIfNull(end);

        //    if (collection == null)
        //        return "null";

        //    bool firstItem = true;

        //    System.Text.StringBuilder builder = new();

        //    builder.Append(start);

        //    // Call ToString on each item and put it in.
        //    foreach (T item in collection)
        //    {
        //        if (!firstItem)
        //            builder.Append(separator);

        //        if (item == null)
        //            builder.Append("null");
        //        else if (recursive && item is IEnumerable && !(item is string))
        //            builder.Append(((IEnumerable)item).TypedAs<object>().ToString(recursive, start, separator, end));
        //        else
        //            builder.Append(item.ToString());

        //        firstItem = false;
        //    }

        //    builder.Append(end);
        //    return builder.ToString();
        //}

        //#endregion string representations

    }
}

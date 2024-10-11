﻿using System.Diagnostics.CodeAnalysis;
using ARWNI2S.Infrastructure.Collections.Internals;
using ARWNI2S.Infrastructure.Collections.Resources;

// CONSIDER: RemoveIdentical to remove an identical item only. Can this be done with current RedBlack tree implementation? How useful is it?

namespace ARWNI2S.Infrastructure.Collections.Sorted
{
    /// <summary>
    /// OrderedBag&lt;T&gt; is a collection that contains items of type T. 
    /// The item are maintained in a sorted order. Unlike a OrderedSet, duplicate items (items that
    /// compare equal to each other) are allows in an OrderedBag.
    /// </summary>
    /// <remarks>
    /// <p>The items are compared in one of three ways. If T implements IComparable&lt;TKey&gt; or IComparable,
    /// then the CompareTo method of that interface will be used to compare items. Alternatively, a comparison
    /// function can be passed in either as a delegate, or as an instance of IComparer&lt;TKey&gt;.</p>
    /// <p>OrderedBag is implemented as a balanced binary tree. Inserting, deleting, and looking up an
    /// an element all are done in log(N) + M time, where N is the number of keys in the tree, and M is the current number
    /// of copies of the element being handled.</p>
    /// <p><see cref="Generic.Bag{T}"/> is similar, but uses hashing instead of comparison, and does not maintain
    /// the keys in sorted order.</p>
    ///</remarks>
    ///<seealso cref="Generic.Bag{T}"/>
    [Serializable]
    public class OrderedBag<T> : CollectionBase<T>, ICloneable
    {
        // The comparer used to compare items.
        private readonly IComparer<T> _comparer;

        // The red-black tree that actually does the work of storing the items.
        private RedBlackTree<T> _tree;

        #region Constructors

        /// <summary>
        /// Creates a new OrderedBag. The T must implement IComparable&lt;T&gt;
        /// or IComparable. 
        /// The CompareTo method of this interface will be used to compare items in this bag.
        /// </summary>
        ///<remarks>
        /// Items that are null are permitted, and will be sorted before all other items.
        ///</remarks>
        /// <exception cref="InvalidOperationException">T does not implement IComparable&lt;TKey&gt;.</exception>
        public OrderedBag() :
            this(Compare.DefaultComparer<T>())
        {
        }

        /// <summary>
        /// Creates a new OrderedBag. The passed delegate will be used to compare items in this bag.
        /// </summary>
        /// <param name="comparison">A delegate to a method that will be used to compare items.</param>
        public OrderedBag(Comparison<T> comparison) :
            this(Compare.ComparerFromComparison(comparison))
        {
        }

        /// <summary>
        /// Creates a new OrderedBag. The Compare method of the passed comparison object
        /// will be used to compare items in this bag.
        /// </summary>
        /// <remarks>
        /// The GetHashCode and Equals methods of the provided IComparer&lt;T&gt; will never
        /// be called, and need not be implemented.
        /// </remarks>
        /// <param name="comparer">An instance of IComparer&lt;T&gt; that will be used to compare items.</param>
        public OrderedBag(IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);

            _comparer = comparer;
            _tree = new RedBlackTree<T>(comparer);
        }

        /// <summary>
        /// Creates a new OrderedBag. The T must implement IComparable&lt;T&gt;
        /// or IComparable. 
        /// The CompareTo method of this interface will be used to compare items in this bag. The bag is
        /// initialized with all the items in the given collection.
        /// </summary>
        ///<remarks>
        /// Items that are null are permitted, and will be sorted before all other items.
        ///</remarks>
        /// <param name="collection">A collection with items to be placed into the OrderedBag.</param>
        /// <exception cref="InvalidOperationException">T does not implement IComparable&lt;TKey&gt;.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Collection copy implementation.")]
        public OrderedBag(IEnumerable<T> collection) :
            this(collection, Compare.DefaultComparer<T>())
        {
        }

        /// <summary>
        /// Creates a new OrderedBag. The passed delegate will be used to compare items in this bag.
        /// The bag is initialized with all the items in the given collection.
        /// </summary>
        /// <param name="collection">A collection with items to be placed into the OrderedBag.</param>
        /// <param name="comparison">A delegate to a method that will be used to compare items.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Collection copy implementation.")]
        public OrderedBag(IEnumerable<T> collection, Comparison<T> comparison) :
            this(collection, Compare.ComparerFromComparison(comparison))
        {
        }

        /// <summary>
        /// Creates a new OrderedBag. The Compare method of the passed comparison object
        /// will be used to compare items in this bag. The bag is
        /// initialized with all the items in the given collection.
        /// </summary>
        /// <remarks>
        /// The GetHashCode and Equals methods of the provided IComparer&lt;T&gt; will never
        /// be called, and need not be implemented.
        /// </remarks>
        /// <param name="collection">A collection with items to be placed into the OrderedBag.</param>
        /// <param name="comparer">An instance of IComparer&lt;T&gt; that will be used to compare items.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Collection copy implementation.")]
        public OrderedBag(IEnumerable<T> collection, IComparer<T> comparer) :
            this(comparer)
        {
            AddMany(collection);
        }

        /// <summary>
        /// Creates a new OrderedBag given a comparer and a tree that contains the data. Used
        /// internally for Clone.
        /// </summary>
        /// <param name="comparer">Comparer for the bag.</param>
        /// <param name="tree">Data for the bag.</param>
        private OrderedBag(IComparer<T> comparer, RedBlackTree<T> tree)
        {
            _comparer = comparer;
            _tree = tree;
        }

        #endregion Constructors

        #region Cloning

        /// <summary>
        /// Makes a shallow clone of this bag; i.e., if items of the
        /// bag are reference types, then they are not cloned. If T is a value type,
        /// then each element is copied as if by simple assignment.
        /// </summary>
        /// <remarks>Cloning the bag takes time O(N), where N is the number of items in the bag.</remarks>
        /// <returns>The cloned bag.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Makes a shallow clone of this bag; i.e., if items of the
        /// bag are reference types, then they are not cloned. If T is a value type,
        /// then each element is copied as if by simple assignment.
        /// </summary>
        /// <remarks>Cloning the bag takes time O(N), where N is the number of items in the bag.</remarks>
        /// <returns>The cloned bag.</returns>
        public OrderedBag<T> Clone()
        {
            OrderedBag<T> newBag = new OrderedBag<T>(_comparer, _tree.Clone());
            return newBag;
        }

        /// <summary>
        /// Makes a deep clone of this bag. A new bag is created with a clone of
        /// each element of this bag, by calling ICloneable.Clone on each element. If T is
        /// a value type, then each element is copied as if by simple assignment.
        /// </summary>
        /// <remarks><para>If T is a reference type, it must implement
        /// ICloneable. Otherwise, an InvalidOperationException is thrown.</para>
        /// <para>Cloning the bag takes time O(N log N), where N is the number of items in the bag.</para></remarks>
        /// <returns>The cloned bag.</returns>
        /// <exception cref="InvalidOperationException">T is a reference type that does not implement ICloneable.</exception>
        public OrderedBag<T> CloneContents()
        {
            if (!CollectionUtils.IsCloneableType(typeof(T), out bool itemIsValueType))
                throw new InvalidOperationException(string.Format(LocalizedStrings.Collections_TypeNotCloneable, typeof(T).FullName));

            OrderedBag<T> clone = new OrderedBag<T>(_comparer);

            // Clone each item, and add it to the new ordered bag.
            foreach (T item in this)
            {
                T itemClone;

                if (itemIsValueType)
                    itemClone = item;
                else
                {
                    if (item == null)
                        itemClone = default;    // Really null, because we know T is a reference type
                    else
                        itemClone = (T)((ICloneable)item).Clone();
                }

                clone.Add(itemClone);
            }

            return clone;
        }

        #endregion Cloning

        #region Basic collection containment

        /// <summary>
        /// Returns the IComparer&lt;T&gt; used to compare items in this bag. 
        /// </summary>
        /// <value>If the bag was created using a comparer, that comparer is returned. If the bag was
        /// created using a comparison delegate, then a comparer equivalent to that delegate
        /// is returned. Otherwise
        /// the default comparer for T (Comparer&lt;T&gt;.Default) is returned.</value>
        public IComparer<T> Comparer
        {
            get
            {
                return _comparer;
            }
        }

        /// <summary>
        /// Returns the number of items in the bag.
        /// </summary>
        /// <remarks>The size of the bag is returned in constant time.</remarks>
        /// <value>The number of items in the bag.</value>
        public sealed override int Count
        {
            get
            {
                return _tree.ElementCount;
            }
        }

        /// <summary>
        /// Returns the number of copies of <paramref name="item"/> in the bag. More precisely, returns
        /// the number of items in the bag that compare equal to <paramref name="item"/>.
        /// </summary>
        /// <remarks>NumberOfCopies() takes time O(log N + M), where N is the total number of items in the
        /// bag, and M is the number of copies of <paramref name="item"/> in the bag.</remarks>
        /// <param name="item">The item to search for in the bag.</param>
        /// <returns>The number of items in the bag that compare equal to <paramref name="item"/>.</returns>
        public int NumberOfCopies(T item)
        {
            return _tree.CountRange(_tree.EqualRangeTester(item));
        }

        /// <summary>
        /// Returns an enumerator that enumerates all the items in the bag. 
        /// The items are enumerated in sorted order.
        /// </summary>
        /// <remarks>
        /// <p>Typically, this method is not called directly. Instead the "foreach" statement is used
        /// to enumerate the items, which uses this method implicitly.</p>
        /// <p>If an item is added to or deleted from the bag while it is being enumerated, then 
        /// the enumeration will end with an InvalidOperationException.</p>
        /// <p>Enumeration all the items in the bag takes time O(N), where N is the number
        /// of items in the bag.</p>
        /// </remarks>
        /// <returns>An enumerator for enumerating all the items in the OrderedBag.</returns>		
        public sealed override IEnumerator<T> GetEnumerator()
        {
            return _tree.GetEnumerator();
        }

        /// <summary>
        /// Determines if this bag contains an item equal to <paramref name="item"/>. The bag
        /// is not changed.
        /// </summary>
        /// <remarks>Searching the bag for an item takes time O(log N), where N is the number of items in the bag.</remarks>
        /// <param name="item">The item to search for.</param>
        /// <returns>True if the bag contains <paramref name="item"/>. False if the bag does not contain <paramref name="item"/>.</returns>
        public sealed override bool Contains(T item)
        {
            return _tree.Find(item, false, false, out T dummy);
        }

        /// <summary>
        /// <para>Enumerates all of the items in this bag that are equal to <paramref name="item"/>, according to the 
        /// comparison mechanism that was used when the bag was created. The bag
        /// is not changed.</para>
        /// <para>If the bag does contain an item equal to <paramref name="item"/>, then the enumeration contains
        /// no items.</para>
        /// </summary>
        /// <remarks>Enumeration the items in the bag equal to <paramref name="item"/> takes time O(log N + M), where N 
        /// is the total number of items in the bag, and M is the number of items equal to <paramref name="item"/>.</remarks>
        /// <param name="item">The item to search for.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates all the items in the bag equal to <paramref name="item"/>. </returns>
        public IEnumerable<T> GetEqualItems(T item)
        {
            return _tree.EnumerateRange(_tree.EqualRangeTester(item));
        }

        /// <summary>
        /// Enumerates all the items in the bag, but enumerates equal items
        /// just once, even if they occur multiple times in the bag.
        /// </summary>
        /// <remarks>If the bag is changed while items are being enumerated, the
        /// enumeration will terminate with an InvalidOperationException.</remarks>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the unique items.</returns>
        public IEnumerable<T> DistinctItems()
        {
            T previous = default;
            bool atBeginning = true;

            // Enumerate the items, but only yield ones not equal to the previous one.
            foreach (T item in this)
            {
                if (atBeginning || _comparer.Compare(item, previous) != 0)
                    yield return item;
                previous = item;
                atBeginning = false;
            }
        }

        #endregion

        #region Index by sorted order

        /// <summary>
        /// Get the item by its index in the sorted order. The smallest item has index 0,
        /// the next smallest item has index 1, and the largest item has index Count-1. 
        /// </summary>
        /// <remarks>The indexer takes time O(log N), which N is the number of items in 
        /// the set.</remarks>
        /// <param name="index">The index to get the item by.</param>
        /// <returns>The item at the given index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is
        /// less than zero or greater than or equal to Count.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");

                return _tree.GetItemByIndex(index);
            }
        }

        /// <summary>
        /// Get the index of the given item in the sorted order. The smallest item has index 0,
        /// the next smallest item has index 1, and the largest item has index Count-1. If multiple
        /// equal items exist, the largest index of the equal items is returned.
        /// </summary>
        /// <remarks>Finding the index takes time O(log N), which N is the number of items in 
        /// the set.</remarks>
        /// <param name="item">The item to get the index of.</param>
        /// <returns>The index of the last item in the sorted bag equal to <paramref name="item"/>, or -1 if the item is not present
        /// in the set.</returns>
        public int LastIndexOf(T item)
        {
            return _tree.FindIndex(item, false);
        }

        /// <summary>
        /// Get the index of the given item in the sorted order. The smallest item has index 0,
        /// the next smallest item has index 1, and the largest item has index Count-1. If multiple
        /// equal items exist, the smallest index of the equal items is returned.
        /// </summary>
        /// <remarks>Finding the index takes time O(log N), which N is the number of items in 
        /// the set.</remarks>
        /// <param name="item">The item to get the index of.</param>
        /// <returns>The index of the first item in the sorted bag equal to <paramref name="item"/>, or -1 if the item is not present
        /// in the set.</returns>
        public int IndexOf(T item)
        {
            return _tree.FindIndex(item, true);
        }

        #endregion

        #region Adding elements

        /// <summary>
        /// Adds a new item to the bag. Since bags can contain duplicate items, the item 
        /// is added even if the bag already contains an item equal to <paramref name="item"/>. In
        /// this case, the new item is placed after all equal items already present in the bag.
        /// </summary>
        /// <remarks>
        /// <para>Adding an item takes time O(log N), where N is the number of items in the bag.</para></remarks>
        /// <param name="item">The item to add to the bag.</param>
        public sealed override void Add(T item)
        {
            _tree.Insert(item, DuplicatePolicy.InsertLast, out T dummy);
        }

        /// <summary>
        /// Adds all the items in <paramref name="collection"/> to the bag. 
        /// </summary>
        /// <remarks>
        /// <para>Adding the collection takes time O(M log N), where N is the number of items in the bag, and M is the 
        /// number of items in <paramref name="collection"/>.</para></remarks>
        /// <param name="collection">A collection of items to add to the bag.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public void AddMany(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            // If we're adding ourselves, we need to copy to a separate array to avoid modification
            // during enumeration.
            if (this == collection)
                collection = ToArray();

            foreach (T item in collection)
                Add(item);
        }

        #endregion Adding elements

        #region Removing elements

        /// <summary>
        /// Searches the bag for one item equal to <paramref name="item"/>, and if found,
        /// removes it from the bag. If not found, the bag is unchanged. If more than one item
        /// equal to <paramref name="item"/>, the item that was last inserted is removed.
        /// </summary>
        /// <remarks>
        /// <para>Equality between items is determined by the comparison instance or delegate used
        /// to create the bag.</para>
        /// <para>Removing an item from the bag takes time O(log N), where N is the number of items in the bag.</para></remarks>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if <paramref name="item"/> was found and removed. False if <paramref name="item"/> was not in the bag.</returns>
        public sealed override bool Remove(T item)
        {
            return _tree.Delete(item, false, out T dummy);
        }

        /// <summary>
        /// Searches the bag for all items equal to <paramref name="item"/>, and 
        /// removes all of them from the bag. If not found, the bag is unchanged.
        /// </summary>
        /// <remarks>
        /// <para>Equality between items is determined by the comparison instance or delegate used
        /// to create the bag.</para>
        /// <para>RemoveAllCopies() takes time O(M log N), where N is the total number of items in the bag, and M is
        /// the number of items equal to <paramref name="item"/>.</para></remarks>
        /// <param name="item">The item to remove.</param>
        /// <returns>The number of copies of <paramref name="item"/> that were found and removed. </returns>
        public int RemoveAllCopies(T item)
        {
            return _tree.DeleteRange(_tree.EqualRangeTester(item));
        }

        /// <summary>
        /// Removes all the items in <paramref name="collection"/> from the bag. Items not
        /// present in the bag are ignored.
        /// </summary>
        /// <remarks>
        /// <para>Equality between items is determined by the comparison instance or delegate used
        /// to create the bag.</para>
        /// <para>Removing the collection takes time O(M log N), where N is the number of items in the bag, and M is the 
        /// number of items in <paramref name="collection"/>.</para></remarks>
        /// <param name="collection">A collection of items to remove from the bag.</param>
        /// <returns>The number of items removed from the bag.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public int RemoveMany(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            int count = 0;

            if (collection == this)
            {
                count = Count;
                Clear();            // special case, otherwise we will throw.
            }
            else
            {
                foreach (T item in collection)
                {
                    if (Remove(item))
                        ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Removes all items from the bag.
        /// </summary>
        /// <remarks>Clearing the bag takes a constant amount of time, regardless of the number of items in it.</remarks>
        public sealed override void Clear()
        {
            _tree.StopEnumerations();  // Invalidate any enumerations.

            // The simplest and fastest way is simply to throw away the old tree and create a new one.
            _tree = new RedBlackTree<T>(_comparer);
        }

        #endregion Removing elements

        #region First/last items

        /// <summary>
        /// If the collection is empty, throw an invalid operation exception.
        /// </summary>
        /// <exception cref="InvalidOperationException">The bag is empty.</exception>
        private void CheckEmpty()
        {
            if (Count == 0)
                throw new InvalidOperationException(LocalizedStrings.Collections_CollectionIsEmpty);
        }

        /// <summary>
        /// Returns the first item in the bag: the item
        /// that would appear first if the bag was enumerated. This is also
        /// the smallest item in the bag.
        /// </summary>
        /// <remarks>GetFirst() takes time O(log N), where N is the number of items in the bag.</remarks>
        /// <returns>The first item in the bag. If more than one item
        /// is smallest, the first one added is returned.</returns>
        /// <exception cref="InvalidOperationException">The bag is empty.</exception>
        public T GetFirst()
        {
            CheckEmpty();
            _tree.FirstItemInRange(_tree.EntireRangeTester, out T item);
            return item;
        }

        /// <summary>
        /// Returns the last item in the bag: the item
        /// that would appear last if the bag was enumerated. This is also the largest
        /// item in the bag.
        /// </summary>
        /// <remarks>GetLast() takes time O(log N), where N is the number of items in the bag.</remarks>
        /// <returns>The last item in the bag. If more than one item
        /// is largest, the last one added is returned.</returns>
        /// <exception cref="InvalidOperationException">The bag is empty.</exception>
        public T GetLast()
        {
            CheckEmpty();
            _tree.LastItemInRange(_tree.EntireRangeTester, out T item);
            return item;
        }

        /// <summary>
        /// Removes the first item in the bag. This is also the smallest
        /// item in the bag.
        /// </summary>
        /// <remarks>RemoveFirst() takes time O(log N), where N is the number of items in the bag.</remarks>
        /// <returns>The item that was removed, which was the smallest item in the bag. </returns>
        /// <exception cref="InvalidOperationException">The bag is empty.</exception>
        public T RemoveFirst()
        {
            CheckEmpty();
            _tree.DeleteItemFromRange(_tree.EntireRangeTester, true, out T item);
            return item;
        }

        /// <summary>
        /// Removes the last item in the bag. This is also the largest item in the bag.
        /// </summary>
        /// <remarks>RemoveLast() takes time O(log N), where N is the number of items in the bag.</remarks>
        /// <returns>The item that was removed, which was the largest item in the bag. </returns>
        /// <exception cref="InvalidOperationException">The bag is empty.</exception>
        public T RemoveLast()
        {
            CheckEmpty();
            _tree.DeleteItemFromRange(_tree.EntireRangeTester, false, out T item);
            return item;
        }

        #endregion

        #region Set operations

        /// <summary>
        /// Checks that this bag and another bag were created with the same comparison
        /// mechanism. Throws exception if not compatible.
        /// </summary>
        /// <param name="otherBag">Other bag to check comparision mechanism.</param>
        /// <exception cref="InvalidOperationException">If otherBag and this bag don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        private void CheckConsistentComparison(OrderedBag<T> otherBag)
        {
            ArgumentNullException.ThrowIfNull(otherBag);

            if (!Equals(_comparer, otherBag._comparer))
                throw new InvalidOperationException(LocalizedStrings.Collections_InconsistentComparisons);
        }

        /// <summary>
        /// Determines if this bag is a superset of another bag. Neither bag is modified.
        /// This bag is a superset of <paramref name="otherBag"/> if every element in
        /// <paramref name="otherBag"/> is also in this bag, at least the same number of
        /// times.
        /// </summary>
        /// <remarks>IsSupersetOf is computed in time O(M log N), where M is the size of the 
        /// <paramref name="otherBag"/>, and N is the size of the this set.</remarks>
        /// <param name="otherBag">OrderedBag to compare to.</param>
        /// <returns>True if this is a superset of <paramref name="otherBag"/>.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public bool IsSupersetOf(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);

            if (otherBag.Count > Count)
                return false;     // Can't be a superset of a bigger bag

            // Checks each item in the other bag to make sure it is in this bag.
            foreach (T item in otherBag.DistinctItems())
            {
                if (NumberOfCopies(item) < otherBag.NumberOfCopies(item))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if this bag is a proper superset of another bag. Neither bag is modified.
        /// This bag is a proper superset of <paramref name="otherBag"/> if every element in
        /// <paramref name="otherBag"/> is also in this bag, at least the same number of
        /// times. Additional, this bag must have strictly more items than <paramref name="otherBag"/>.
        /// </summary>
        /// <remarks>IsProperSupersetOf is computed in time O(M log N), where M is the number of unique items in 
        /// <paramref name="otherBag"/>.</remarks>
        /// <param name="otherBag">OrderedBag to compare to.</param>
        /// <returns>True if this is a proper superset of <paramref name="otherBag"/>.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public bool IsProperSupersetOf(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);

            if (otherBag.Count >= Count)
                return false;     // Can't be a proper superset of a bigger or equal set

            return IsSupersetOf(otherBag);
        }

        /// <summary>
        /// Determines if this bag is a subset of another bag. Neither bag is modified.
        /// This bag is a subset of <paramref name="otherBag"/> if every element in this bag
        /// is also in <paramref name="otherBag"/>, at least the same number of
        /// times.
        /// </summary>
        /// <remarks>IsSubsetOf is computed in time O(N log M), where M is the size of the 
        /// <paramref name="otherBag"/>, and N is the size of the this bag.</remarks>
        /// <param name="otherBag">OrderedBag to compare to.</param>
        /// <returns>True if this is a subset of <paramref name="otherBag"/>.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public bool IsSubsetOf(OrderedBag<T> otherBag)
        {
            return otherBag.IsSupersetOf(this);
        }

        /// <summary>
        /// Determines if this bag is a proper subset of another bag. Neither bag is modified.
        /// This bag is a subset of <paramref name="otherBag"/> if every element in this bag
        /// is also in <paramref name="otherBag"/>, at least the same number of
        /// times. Additional, this bag must have strictly fewer items than <paramref name="otherBag"/>.
        /// </summary>
        /// <remarks>IsSubsetOf is computed in time O(N log M), where M is the size of the 
        /// <paramref nameb="otherBag"/>, and N is the size of the this bag.</remarks>
        /// <param name="otherBag">OrderedBag to compare to.</param>
        /// <returns>True if this is a proper subset of <paramref name="otherBag"/>.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public bool IsProperSubsetOf(OrderedBag<T> otherBag)
        {
            return otherBag.IsProperSupersetOf(this);
        }

        /// <summary>
        /// Determines if this bag is disjoint from another bag. Two bags are disjoint
        /// if no item from one set is equal to any item in the other bag.
        /// </summary>
        /// <remarks>
        /// <para>The answer is computed in time O(N), where N is the size of the smaller set.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to check disjointness with.</param>
        /// <returns>True if the two bags are disjoint, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        public bool IsDisjointFrom(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            OrderedBag<T> smaller, larger;
            if (otherBag.Count > Count)
            {
                smaller = this; larger = otherBag;
            }
            else
            {
                smaller = otherBag; larger = this;
            }

            foreach (T item in smaller)
            {
                if (larger.Contains(item))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if this bag is equal to another bag. This bag is equal to
        /// <paramref name="otherBag"/> if they contain the same items, each the
        /// same number of times.
        /// </summary>
        /// <remarks>IsEqualTo is computed in time O(N), where N is the number of items in 
        /// this bag.</remarks>
        /// <param name="otherBag">OrderedBag to compare to</param>
        /// <returns>True if this bag is equal to <paramref name="otherBag"/>, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        public bool IsEqualTo(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);

            // Must be the same size.
            if (otherBag.Count != Count)
                return false;

            // Since both bags are ordered, we can simply compare items in order.
            using (IEnumerator<T> enum1 = GetEnumerator(), enum2 = otherBag.GetEnumerator())
            {
                bool continue1, continue2;

                for (; ; )
                {
                    continue1 = enum1.MoveNext(); continue2 = enum2.MoveNext();
                    if (!continue1 || !continue2)
                        break;

                    if (_comparer.Compare(enum1.Current, enum2.Current) != 0)
                        return false;     // the two items are not equal.
                }

                // If both continue1 and continue2 are false, we reached the end of both sequences at the same
                // time and found success. If one is true and one is false, the sequences were of difference lengths -- failure.
                return continue1 == continue2;
            }
        }

        /// <summary>
        /// Computes the union of this bag with another bag. The union of two bags
        /// is all items from both of the bags. If an item appears X times in one bag,
        /// and Y times in the other bag, the union contains the item Maximum(X,Y) times. This bag receives
        /// the union of the two bags, the other bag is unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The union of two bags is computed in time O(M + N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to union with.</param>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public void UnionWith(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);

            T previous = default;
            bool atBeginning = true;
            int copiesInThis = 0, copiesInOther = 0;

            // Enumerate each of the items in the other bag. Add items that need to be
            // added to this bag.
            // CONSIDER: may be able to improve this algorithm if otherBag is larger than this bag.
            foreach (T item in otherBag)
            {
                if (atBeginning || _comparer.Compare(item, previous) != 0)
                {
                    copiesInThis = NumberOfCopies(item);
                    copiesInOther = 1;
                }
                else
                {
                    ++copiesInOther;
                }

                if (copiesInOther > copiesInThis)
                    Add(item);

                previous = item;
                atBeginning = false;
            }
        }

        /// <summary>
        /// Computes the union of this bag with another bag. The union of two bags
        /// is all items from both of the bags.  If an item appears X times in one bag,
        /// and Y times in the other bag, the union contains the item Maximum(X,Y) times. A new bag is 
        /// created with the union of the bags and is returned. This bag and the other bag 
        /// are unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The union of two bags is computed in time O(M + N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to union with.</param>
        /// <returns>The union of the two bags.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public OrderedBag<T> Union(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            OrderedBag<T> smaller, larger, result;
            if (otherBag.Count > Count)
            {
                smaller = this; larger = otherBag;
            }
            else
            {
                smaller = otherBag; larger = this;
            }

            result = larger.Clone();
            result.UnionWith(smaller);
            return result;
        }

        /// <summary>
        /// Computes the sum of this bag with another bag. The sum of two bags
        /// is all items from both of the bags. If an item appears X times in one bag,
        /// and Y times in the other bag, the sum contains the item (X+Y) times. This bag receives
        /// the sum of the two bags, the other bag is unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The sum of two bags is computed in time O(M + N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to sum with.</param>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public void SumWith(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);

            AddMany(otherBag);

            // CONSIDER: if otherBag is much larger, maybe better to clone it,
            // add all of the current into it, and replace.
        }

        /// <summary>
        /// Computes the sum of this bag with another bag. he sum of two bags
        /// is all items from both of the bags.  If an item appears X times in one bag,
        /// and Y times in the other bag, the sum contains the item (X+Y) times. A new bag is 
        /// created with the sum of the bags and is returned. This bag and the other bag 
        /// are unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The sum of two bags is computed in time O(M + N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to sum with.</param>
        /// <returns>The sum of the two bags.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public OrderedBag<T> Sum(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            OrderedBag<T> smaller, larger, result;
            if (otherBag.Count > Count)
            {
                smaller = this; larger = otherBag;
            }
            else
            {
                smaller = otherBag; larger = this;
            }

            result = larger.Clone();
            result.AddMany(smaller);
            return result;
        }

        /// <summary>
        /// Computes the intersection of this bag with another bag. The intersection of two bags
        /// is all items that appear in both of the bags. If an item appears X times in one bag,
        /// and Y times in the other bag, the sum contains the item Minimum(X,Y) times. This bag receives
        /// the intersection of the two bags, the other bag is unchanged.
        /// </summary>
        /// <remarks>
        /// <para>When equal items appear in both bags, the intersection will include an arbitrary choice of one of the
        /// two equal items.</para>
        /// <para>The intersection of two bags is computed in time O(N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to intersection with.</param>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public void IntersectionWith(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            _tree.StopEnumerations();

            OrderedBag<T> smaller, larger;
            if (otherBag.Count > Count)
            {
                smaller = this; larger = otherBag;
            }
            else
            {
                smaller = otherBag; larger = this;
            }

            RedBlackTree<T> newTree = new RedBlackTree<T>(_comparer);

            T previous = default;
            bool atBeginning = true;
            int copiesInSmaller = 0, copiesInLarger = 0;

            // Enumerate each of the items in the smaller bag. Add items that need to be
            // added to the intersection.
            foreach (T item in smaller)
            {
                if (atBeginning || _comparer.Compare(item, previous) != 0)
                {
                    copiesInLarger = larger.NumberOfCopies(item);
                    copiesInSmaller = 1;
                }
                else
                {
                    ++copiesInSmaller;
                }

                if (copiesInSmaller <= copiesInLarger)
                    newTree.Insert(item, DuplicatePolicy.InsertLast, out T dummy);

                previous = item;
                atBeginning = false;
            }

            _tree = newTree;
        }

        /// <summary>
        /// Computes the intersection of this bag with another bag. The intersection of two bags
        /// is all items that appear in both of the bags. If an item appears X times in one bag,
        /// and Y times in the other bag, the sum contains the item Minimum(X,Y) times. A new bag is 
        /// created with the intersection of the bags and is returned. This bag and the other bag 
        /// are unchanged.
        /// </summary>
        /// <remarks>
        /// <para>When equal items appear in both bags, the intersection will include an arbitrary choice of one of the
        /// two equal items.</para>
        /// <para>The intersection of two bags is computed in time O(N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to intersection with.</param>
        /// <returns>The intersection of the two bags.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public OrderedBag<T> Intersection(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            OrderedBag<T> smaller, larger, result;
            if (otherBag.Count > Count)
            {
                smaller = this; larger = otherBag;
            }
            else
            {
                smaller = otherBag; larger = this;
            }

            T previous = default;
            bool atBeginning = true;
            int copiesInSmaller = 0, copiesInLarger = 0;

            // Enumerate each of the items in the smaller bag. Add items that need to be
            // added to the intersection.
            result = new OrderedBag<T>(_comparer);
            foreach (T item in smaller)
            {
                if (atBeginning || _comparer.Compare(item, previous) != 0)
                {
                    copiesInLarger = larger.NumberOfCopies(item);
                    copiesInSmaller = 1;
                }
                else
                {
                    ++copiesInSmaller;
                }

                if (copiesInSmaller <= copiesInLarger)
                    result.Add(item);

                previous = item;
                atBeginning = false;
            }

            return result;
        }

        /// <summary>
        /// Computes the difference of this bag with another bag. The difference of these two bags
        /// is all items that appear in this bag, but not in <paramref name="otherBag"/>. If an item appears X times in this bag,
        /// and Y times in the other bag, the difference contains the item X - Y times (zero times if Y >= X). This bag receives
        /// the difference of the two bags; the other bag is unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The difference of two bags is computed in time O(M + N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to difference with.</param>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public void DifferenceWith(OrderedBag<T> otherBag)
        {
            // Difference with myself is nothing. This check is needed because the
            // main algorithm doesn't work correctly otherwise.
            if (this == otherBag)
                Clear();

            CheckConsistentComparison(otherBag);

            T previous = default;
            bool atBeginning = true;
            int copiesInThis = 0, copiesInOther = 0;

            // Enumerate each of the items in the other bag. Remove items that need to be
            // removed from this bag.
            // CONSIDER: should be able to improve this algorithm if otherBag is larger than this bag.
            foreach (T item in otherBag)
            {
                if (atBeginning || _comparer.Compare(item, previous) != 0)
                {
                    copiesInThis = NumberOfCopies(item);
                    copiesInOther = 1;
                }
                else
                {
                    ++copiesInOther;
                }

                if (copiesInOther <= copiesInThis)
                    Remove(item);

                previous = item;
                atBeginning = false;
            }

        }

        /// <summary>
        /// Computes the difference of this bag with another bag. The difference of these two bags
        /// is all items that appear in this bag, but not in <paramref name="otherBag"/>. If an item appears X times in this bag,
        /// and Y times in the other bag, the difference contains the item X - Y times (zero times if Y >= X).  A new bag is 
        /// created with the difference of the bags and is returned. This bag and the other bag 
        /// are unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The difference of two bags is computed in time O(M + N log M), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to difference with.</param>
        /// <returns>The difference of the two bags.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public OrderedBag<T> Difference(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            OrderedBag<T> result = Clone();
            result.DifferenceWith(otherBag);
            return result;
        }

        /// <summary>
        /// Computes the symmetric difference of this bag with another bag. The symmetric difference of two bags
        /// is all items that appear in either of the bags, but not both. If an item appears X times in one bag,
        /// and Y times in the other bag, the symmetric difference contains the item AbsoluteValue(X - Y times). This bag receives
        /// the symmetric difference of the two bags; the other bag is unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The symmetric difference of two bags is computed in time O(M + N), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to symmetric difference with.</param>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public void SymmetricDifferenceWith(OrderedBag<T> otherBag)
        {
            _tree = SymmetricDifference(otherBag)._tree;
        }

        /// <summary>
        /// Computes the symmetric difference of this bag with another bag. The symmetric difference of two bags
        /// is all items that appear in either of the bags, but not both. If an item appears X times in one bag,
        /// and Y times in the other bag, the symmetric difference contains the item AbsoluteValue(X - Y times). A new bag is 
        /// created with the symmetric difference of the bags and is returned. This bag and the other bag 
        /// are unchanged.
        /// </summary>
        /// <remarks>
        /// <para>The symmetric difference of two bags is computed in time O(M + N), where M is the size of the 
        /// larger bag, and N is the size of the smaller bag.</para>
        /// </remarks>
        /// <param name="otherBag">Bag to symmetric difference with.</param>
        /// <returns>The symmetric difference of the two bags.</returns>
        /// <exception cref="InvalidOperationException">This bag and <paramref name="otherBag"/> don't use the same method for comparing items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="otherBag"/> is null.</exception>
        public OrderedBag<T> SymmetricDifference(OrderedBag<T> otherBag)
        {
            CheckConsistentComparison(otherBag);
            OrderedBag<T> result = new OrderedBag<T>(_comparer);
            IEnumerator<T> enum1 = GetEnumerator(), enum2 = otherBag.GetEnumerator();

            bool valid1 = enum1.MoveNext();
            bool valid2 = enum2.MoveNext();
            int compare;
            for (; ; )
            {
                // Which item is smaller? The end (!valid) is considered larger than any item.
                if (!valid1)
                {
                    if (!valid2)
                        break;
                    compare = 1;
                }
                else if (!valid2)
                    compare = -1;
                else
                    compare = _comparer.Compare(enum1.Current, enum2.Current);

                // If equal, move through both bags without adding. Otherwise, add the smaller item and advance
                // just through that bag.
                if (compare == 0)
                {
                    valid1 = enum1.MoveNext();
                    valid2 = enum2.MoveNext();
                }
                else if (compare < 0)
                {
                    result.Add(enum1.Current);
                    valid1 = enum1.MoveNext();
                }
                else
                { // compare > 0
                    result.Add(enum2.Current);
                    valid2 = enum2.MoveNext();
                }
            }

            return result;
        }

        #endregion Set operations

        #region Read-only list view

        /// <summary>
        /// Get a read-only list view of the items in this ordered bag. The
        /// items in the list are in sorted order, with the smallest item
        /// at index 0. This view does not copy any data, and reflects any
        /// changes to the underlying OrderedBag.
        /// </summary>
        /// <returns>A read-only IList&lt;T&gt; view onto this OrderedBag.</returns>
        public IList<T> AsList()
        {
            return new ListView(this, _tree.EntireRangeTester, true, false);
        }

        /// <summary>
        /// The nested class that provides a read-only list view
        /// of all or part of the collection.
        /// </summary>
        [Serializable]
        private class ListView : ReadOnlyListBase<T>
        {
            private readonly OrderedBag<T> _myBag;
            private readonly RedBlackTree<T>.RangeTester _rangeTester;   // range tester for the range being used.
            private readonly bool _entireTree;                   // is the view the whole tree?
            private readonly bool _reversed;                     // is the view reversed?

            /// <summary>
            /// Create a new list view wrapped the given set.
            /// </summary>
            /// <param name="myBag">The ordered bag to wrap.</param>
            /// <param name="rangeTester">Range tester that defines the range being used.</param>
            /// <param name="entireTree">If true, then rangeTester defines the entire tree. Used to optimize some operations.</param>
            /// <param name="reversed">Is the view enuemerated in reverse order?</param>
            public ListView(OrderedBag<T> myBag, RedBlackTree<T>.RangeTester rangeTester, bool entireTree, bool reversed)
            {
                _myBag = myBag;
                _rangeTester = rangeTester;
                _entireTree = entireTree;
                _reversed = reversed;
            }

            public sealed override int Count
            {
                get
                {
                    if (_entireTree)
                        return _myBag.Count;
                    else
                    {
                        // Note: we can't cache the result of this call because the underlying
                        // set can change, which would make the cached value incorrect.
                        return _myBag._tree.CountRange(_rangeTester);
                    }
                }
            }

            public sealed override T this[int index]
            {
                get
                {
                    if (_entireTree)
                    {
                        if (_reversed)
                            return _myBag[_myBag.Count - 1 - index];
                        else
                            return _myBag[index];
                    }
                    else
                    {
                        int firstIndex = _myBag._tree.FirstItemInRange(_rangeTester, out T dummy);
                        int lastIndex = _myBag._tree.LastItemInRange(_rangeTester, out dummy);
                        if (firstIndex < 0 || lastIndex < 0 || index < 0 || index >= lastIndex - firstIndex + 1)
                            throw new ArgumentOutOfRangeException("index");

                        if (_reversed)
                            return _myBag[lastIndex - index];
                        else
                            return _myBag[firstIndex + index];
                    }
                }
            }

            public sealed override int IndexOf(T item)
            {
                if (_entireTree)
                {
                    if (_reversed)
                        return _myBag.Count - 1 - _myBag.LastIndexOf(item);
                    else
                        return _myBag.IndexOf(item);
                }
                else
                {
                    T dummy;

                    if (_rangeTester(item) != 0)
                        return -1;

                    if (_reversed)
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, false);
                        if (indexInSet < 0)
                            return -1;
                        int indexOfEnd = _myBag._tree.LastItemInRange(_rangeTester, out dummy);
                        return indexOfEnd - indexInSet;

                    }
                    else
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, true);
                        if (indexInSet < 0)
                            return -1;
                        int indexOfStart = _myBag._tree.FirstItemInRange(_rangeTester, out dummy);
                        return indexInSet - indexOfStart;
                    }
                }
            }
        }

        #endregion Read-only list view

        #region Sub-views

        /// <summary>
        /// Returns a View collection that can be used for enumerating the items in the bag in 
        /// reversed order.
        /// </summary>
        ///<remarks>
        ///<p>Typically, this method is used in conjunction with a foreach statement. For example:
        ///<code>
        /// foreach(T item in bag.Reversed()) {
        ///    // process item
        /// }
        ///</code></p>
        /// <p>If an item is added to or deleted from the bag while the View is being enumerated, then 
        /// the enumeration will end with an InvalidOperationException.</p>
        ///<p>Calling Reverse does not copy the data in the tree, and the operation takes constant time.</p>
        ///</remarks>
        /// <returns>An OrderedBag.View of items in reverse order.</returns>
        internal View Reversed()   // A reversed view that can be enumerated
        {
            return new View(this, _tree.EntireRangeTester, true, true);
        }

        /// <summary>
        /// Returns a View collection that can be used for enumerating a range of the items in the bag.
        /// Only items that are greater than <paramref name="from"/> and 
        /// less than <paramref name="to"/> are included. The items are enumerated in sorted order.
        /// Items equal to the end points of the range can be included or excluded depending on the
        /// <paramref name="fromInclusive"/> and <paramref name="toInclusive"/> parameters.
        /// </summary>
        ///<remarks>
        ///<p>If <paramref name="from"/> is greater than or equal to <paramref name="to"/>, the returned collection is empty. </p>
        ///<p>Typically, this method is used in conjunction with a foreach statement. For example:
        ///<code>
        /// foreach(T item in bag.Range(from, true, to, false)) {
        ///    // process item
        /// }
        ///</code></p>
        /// <p>If an item is added to or deleted from the bag while the View is being enumerated, then 
        /// the enumeration will end with an InvalidOperationException.</p>
        ///<p>Calling Range does not copy the data in the tree, and the operation takes constant time.</p>
        ///</remarks>
        /// <param name="from">The lower bound of the range.</param>
        /// <param name="fromInclusive">If true, the lower bound is inclusive--items equal to the lower bound will
        /// be included in the range. If false, the lower bound is exclusive--items equal to the lower bound will not
        /// be included in the range.</param>
        /// <param name="to">The upper bound of the range. </param>
        /// <param name="toInclusive">If true, the upper bound is inclusive--items equal to the upper bound will
        /// be included in the range. If false, the upper bound is exclusive--items equal to the upper bound will not
        /// be included in the range.</param>
        /// <returns>An OrderedBag.View of items in the given range.</returns>
        internal View Range(T from, bool fromInclusive, T to, bool toInclusive)  // A partial view that can be enumerated
        {
            return new View(this, _tree.DoubleBoundedRangeTester(from, fromInclusive, to, toInclusive), false, false);
        }

        /// <summary>
        /// Returns a View collection that can be used for enumerating a range of the items in the bag.
        /// Only items that are greater than (and optionally, equal to) <paramref name="from"/> are included. 
        /// The items are enumerated in sorted order. Items equal to <paramref name="from"/> can be included
        /// or excluded depending on the <paramref name="fromInclusive"/> parameter.
        /// </summary>
        ///<remarks>
        ///<p>Typically, this method is used in conjunction with a foreach statement. For example:
        ///<code>
        /// foreach(T item in bag.RangeFrom(from, true)) {
        ///    // process item
        /// }
        ///</code></p>
        /// <p>If an item is added to or deleted from the bag while the View is being enumerated, then 
        /// the enumeration will end with an InvalidOperationException.</p>
        ///<p>Calling RangeFrom does not copy the data in the tree, and the operation takes constant time.</p>
        ///</remarks>
        /// <param name="from">The lower bound of the range.</param>
        /// <param name="fromInclusive">If true, the lower bound is inclusive--items equal to the lower bound will
        /// be included in the range. If false, the lower bound is exclusive--items equal to the lower bound will not
        /// be included in the range.</param>
        /// <returns>An OrderedBag.View of items in the given range.</returns>
        internal View RangeFrom(T from, bool fromInclusive)  // A partial view that can be enumerated
        {
            return new View(this, _tree.LowerBoundedRangeTester(from, fromInclusive), false, false);
        }

        /// <summary>
        /// Returns a View collection that can be used for enumerating a range of the items in the bag.
        /// Only items that are less than (and optionally, equal to) <paramref name="to"/> are included. 
        /// The items are enumerated in sorted order. Items equal to <paramref name="to"/> can be included
        /// or excluded depending on the <paramref name="toInclusive"/> parameter.
        /// </summary>
        ///<remarks>
        ///<p>Typically, this method is used in conjunction with a foreach statement. For example:
        ///<code>
        /// foreach(T item in bag.RangeTo(to, false)) {
        ///    // process item
        /// }
        ///</code></p>
        /// <p>If an item is added to or deleted from the bag while the View is being enumerated, then 
        /// the enumeration will end with an InvalidOperationException.</p>
        ///<p>Calling RangeTo does not copy the data in the tree, and the operation takes constant time.</p>
        ///</remarks>
        /// <param name="to">The upper bound of the range. </param>
        /// <param name="toInclusive">If true, the upper bound is inclusive--items equal to the upper bound will
        /// be included in the range. If false, the upper bound is exclusive--items equal to the upper bound will not
        /// be included in the range.</param>
        /// <returns>An OrderedBag.View of items in the given range.</returns>
        internal View RangeTo(T to, bool toInclusive)  // A partial view that can be enumerated
        {
            return new View(this, _tree.UpperBoundedRangeTester(to, toInclusive), false, false);
        }

        #endregion

        #region View nested class

        /// <summary>
        /// The OrderedBag&lt;T&gt;.View class is used to look at a subset of the items
        /// inside an ordered bag. It is returned from the Range, RangeTo, RangeFrom, and Reversed methods. 
        /// </summary>
        ///<remarks>
        /// <p>Views are dynamic. If the underlying bag changes, the view changes in sync. If a change is made
        /// to the view, the underlying bag changes accordingly.</p>
        ///<p>Typically, this class is used in conjunction with a foreach statement to enumerate the items 
        /// in a subset of the OrderedBag. For example:</p>
        ///<code>
        /// foreach(T item in bag.Range(from, to)) {
        ///    // process item
        /// }
        ///</code>
        ///</remarks>
        [Serializable]
        internal class View : CollectionBase<T>
        {
            private readonly OrderedBag<T> _myBag;
            private readonly RedBlackTree<T>.RangeTester _rangeTester;   // range tester for the range being used.
            private readonly bool _entireTree;                   // is the view the whole tree?
            private readonly bool _reversed;                     // is the view reversed?

            /// <summary>
            /// Initialize the view.
            /// </summary>
            /// <param name="myBag">OrderedBag being viewed</param>
            /// <param name="rangeTester">Range tester that defines the range being used.</param>
            /// <param name="entireTree">If true, then rangeTester defines the entire tree.</param>
            /// <param name="reversed">Is the view enuemerated in reverse order?</param>
            public View(OrderedBag<T> myBag, RedBlackTree<T>.RangeTester rangeTester, bool entireTree, bool reversed)
            {
                _myBag = myBag;
                _rangeTester = rangeTester;
                _entireTree = entireTree;
                _reversed = reversed;
            }

            /// <summary>
            /// Determine if the given item lies within the bounds of this view.
            /// </summary>
            /// <param name="item">Item to test.</param>
            /// <returns>True if the item is within the bounds of this view.</returns>
            private bool ItemInView(T item)
            {
                return _rangeTester(item) == 0;
            }

            /// <summary>
            /// Enumerate all the items in this view.
            /// </summary>
            /// <returns>An IEnumerator&lt;T&gt; with the items in this view.</returns>
            public sealed override IEnumerator<T> GetEnumerator()
            {
                if (_reversed)
                    return _myBag._tree.EnumerateRangeReversed(_rangeTester).GetEnumerator();
                else
                    return _myBag._tree.EnumerateRange(_rangeTester).GetEnumerator();
            }

            /// <summary>
            /// Number of items in this view.
            /// </summary>
            /// <value>Number of items that lie within the bounds the view.</value>
            public sealed override int Count
            {
                get
                {
                    if (_entireTree)
                        return _myBag.Count;
                    else
                    {
                        // Note: we can't cache the result of this call because the underlying
                        // set can change, which would make the cached value incorrect.
                        return _myBag._tree.CountRange(_rangeTester);
                    }
                }
            }

            /// <summary>
            /// Removes all the items within this view from the underlying bag.
            /// </summary>
            /// <example>The following removes all the items that start with "A" from an OrderedBag.
            /// <code>
            /// bag.Range("A", "B").Clear();
            /// </code>
            /// </example>
            public sealed override void Clear()
            {
                if (_entireTree)
                {
                    _myBag.Clear();
                }
                else
                {
                    _myBag._tree.DeleteRange(_rangeTester);
                }
            }

            /// <summary>
            /// Adds a new item to the bag underlying this View. If the bag already contains an item equal to
            /// <paramref name="item"/>, that item is replaces with <paramref name="item"/>. If
            /// <paramref name="item"/> is outside the range of this view, an InvalidOperationException
            /// is thrown.
            /// </summary>
            /// <remarks>
            /// <para>Equality between items is determined by the comparison instance or delegate used
            /// to create the bag.</para>
            /// <para>Adding an item takes time O(log N), where N is the number of items in the bag.</para></remarks>
            /// <param name="item">The item to add.</param>
            /// <returns>True if the bag already contained an item equal to <paramref name="item"/> (which was replaced), false 
            /// otherwise.</returns>
            public sealed override void Add(T item)
            {
                if (!ItemInView(item))
                    throw new ArgumentException(LocalizedStrings.Collections_OutOfViewRange, "item");
                else
                    _myBag.Add(item);
            }

            /// <summary>
            /// Searches the underlying bag for an item equal to <paramref name="item"/>, and if found,
            /// removes it from the bag. If not found, the bag is unchanged. If the item is outside
            /// the range of this view, the bag is unchanged.
            /// </summary>
            /// <remarks>
            /// <para>Equality between items is determined by the comparison instance or delegate used
            /// to create the bag.</para>
            /// <para>Removing an item from the bag takes time O(log N), where N is the number of items in the bag.</para></remarks>
            /// <param name="item">The item to remove.</param>
            /// <returns>True if <paramref name="item"/> was found and removed. False if <paramref name="item"/> was not in the bag, or
            /// was outside the range of this view.</returns>
            public sealed override bool Remove(T item)
            {
                if (!ItemInView(item))
                    return false;
                else
                    return _myBag.Remove(item);
            }

            /// <summary>
            /// Determines if this view of the bag contains an item equal to <paramref name="item"/>. The bag
            /// is not changed. If 
            /// </summary>
            /// <remarks>Searching the bag for an item takes time O(log N), where N is the number of items in the bag.</remarks>
            /// <param name="item">The item to search for.</param>
            /// <returns>True if the bag contains <paramref name="item"/>, and <paramref name="item"/> is within
            /// the range of this view. False otherwise.</returns>
            public sealed override bool Contains(T item)
            {
                if (!ItemInView(item))
                    return false;
                else
                    return _myBag.Contains(item);
            }

            /// <summary>
            /// Get the first index of the given item in the view. The smallest item in the view has index 0,
            /// the next smallest item has index 1, and the largest item has index Count-1. 
            /// </summary>
            /// <remarks>Finding the index takes time O(log N), which N is the number of items in 
            /// the set.</remarks>
            /// <param name="item">The item to get the index of.</param>
            /// <returns>The index of the first item in the view equal to <paramref name="item"/>, or -1 if the item is not present
            /// in the view.</returns>
            public int IndexOf(T item)
            {
                if (_entireTree)
                {
                    if (_reversed)
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, false);
                        if (indexInSet < 0)
                            return -1;

                        return _myBag.Count - 1 - indexInSet;
                    }
                    else
                    {
                        return _myBag._tree.FindIndex(item, true);
                    }
                }
                else
                {
                    T dummy;

                    if (!ItemInView(item))
                        return -1;

                    if (_reversed)
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, false);
                        if (indexInSet < 0)
                            return -1;
                        int indexOfEnd = _myBag._tree.LastItemInRange(_rangeTester, out dummy);
                        return indexOfEnd - indexInSet;

                    }
                    else
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, true);
                        if (indexInSet < 0)
                            return -1;
                        int indexOfStart = _myBag._tree.FirstItemInRange(_rangeTester, out dummy);
                        return indexInSet - indexOfStart;
                    }
                }
            }

            /// <summary>
            /// Get the last index of the given item in the view. The smallest item in the view has index 0,
            /// the next smallest item has index 1, and the largest item has index Count-1. 
            /// </summary>
            /// <remarks>Finding the index takes time O(log N), which N is the number of items in 
            /// the set.</remarks>
            /// <param name="item">The item to get the index of.</param>
            /// <returns>The index of the last item in the view equal to <paramref name="item"/>, or -1 if the item is not present
            /// in the view.</returns>
            public int LastIndexOf(T item)
            {
                if (_entireTree)
                {
                    if (_reversed)
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, true);
                        if (indexInSet < 0)
                            return -1;

                        return _myBag.Count - 1 - indexInSet;
                    }
                    else
                    {
                        return _myBag._tree.FindIndex(item, false);
                    }
                }
                else
                {
                    T dummy;

                    if (!ItemInView(item))
                        return -1;

                    if (_reversed)
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, true);
                        if (indexInSet < 0)
                            return -1;
                        int indexOfEnd = _myBag._tree.LastItemInRange(_rangeTester, out dummy);
                        return indexOfEnd - indexInSet;

                    }
                    else
                    {
                        int indexInSet = _myBag._tree.FindIndex(item, false);
                        if (indexInSet < 0)
                            return -1;
                        int indexOfStart = _myBag._tree.FirstItemInRange(_rangeTester, out dummy);
                        return indexInSet - indexOfStart;
                    }
                }
            }

            /// <summary>
            /// Get the item by its index in the sorted order. The smallest item in the view has index 0,
            /// the next smallest item has index 1, and the largest item has index Count-1. 
            /// </summary>
            /// <remarks>The indexer takes time O(log N), which N is the number of items in 
            /// the set.</remarks>
            /// <param name="index">The index to get the item by.</param>
            /// <returns>The item at the given index.</returns>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is
            /// less than zero or greater than or equal to Count.</exception>
            public T this[int index]
            {
                get
                {
                    if (_entireTree)
                    {
                        if (_reversed)
                        {
                            return _myBag[_myBag.Count - 1 - index];
                        }
                        else
                        {
                            return _myBag[index];
                        }
                    }
                    else
                    {
                        int firstIndex = _myBag._tree.FirstItemInRange(_rangeTester, out T dummy);
                        int lastIndex = _myBag._tree.LastItemInRange(_rangeTester, out dummy);
                        if (firstIndex < 0 || lastIndex < 0 || index < 0 || index >= lastIndex - firstIndex + 1)
                            throw new ArgumentOutOfRangeException("index");

                        if (_reversed)
                            return _myBag[lastIndex - index];
                        else
                            return _myBag[firstIndex + index];
                    }
                }
            }

            /// <summary>
            /// Get a read-only list view of the items in this view. The
            /// items in the list are in sorted order, with the smallest item
            /// at index 0. This view does not copy any data, and reflects any
            /// changes to the underlying OrderedSet.
            /// </summary>
            /// <returns>A read-only IList&lt;T&gt; view onto this view.</returns>
            public IList<T> AsList()
            {
                return new ListView(_myBag, _rangeTester, _entireTree, _reversed);
            }

            /// <summary>
            /// Creates a new View that has the same items as this view, in the reversed order.
            /// </summary>
            /// <returns>A new View that has the reversed order of this view, with the same upper 
            /// and lower bounds.</returns>
            public View Reversed()
            {
                return new View(_myBag, _rangeTester, _entireTree, !_reversed);
            }

            /// <summary>
            /// Returns the first item in this view: the item
            /// that would appear first if the view was enumerated. 
            /// </summary>
            /// <remarks>GetFirst() takes time O(log N), where N is the number of items in the bag.</remarks>
            /// <returns>The first item in the view. </returns>
            /// <exception cref="InvalidOperationException">The view has no items in it.</exception>
            public T GetFirst()
            {
                T item;
                int found;

                if (_reversed)
                    found = _myBag._tree.LastItemInRange(_rangeTester, out item);
                else
                    found = _myBag._tree.FirstItemInRange(_rangeTester, out item);

                if (found < 0)
                    throw new InvalidOperationException(LocalizedStrings.Collections_CollectionIsEmpty);

                return item;
            }

            /// <summary>
            /// Returns the last item in the view: the item
            /// that would appear last if the view was enumerated. 
            /// </summary>
            /// <remarks>GetLast() takes time O(log N), where N is the number of items in the bag.</remarks>
            /// <returns>The last item in the view. </returns>
            /// <exception cref="InvalidOperationException">The view has no items in it.</exception>
            public T GetLast()
            {
                T item;
                int found;

                if (_reversed)
                    found = _myBag._tree.FirstItemInRange(_rangeTester, out item);
                else
                    found = _myBag._tree.LastItemInRange(_rangeTester, out item);

                if (found < 0)
                    throw new InvalidOperationException(LocalizedStrings.Collections_CollectionIsEmpty);

                return item;
            }
        }

        #endregion
    }
}

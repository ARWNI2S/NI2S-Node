using System.Collections;

namespace ARWNI2S.Collections.Trees
{
    /// <summary>
    /// Implements a collection of Tree Nodes (Node&lt;T&gt; />)
    /// <para><b>Implementation Note:</b> The root of a data tree is always a Node&lt;T&gt;. You cannot
    /// create a standalone NodeList&lt;T&gt;.
    /// </para>
    /// </summary>
    /// <typeparam name="T">typeof the data value of each node</typeparam>
    public class TreeNodeCollection<T>
          : CollectionBase, IEnumerable<TreeNode<T>>
    {
        private TreeNode<T> _owner = null;

        /// <summary>
        /// Gets the <see cref="TreeNode&lt;T&gt;" /> to which this collection belongs (this==Owner.Childs). 
        /// Never null.
        /// </summary>
        public TreeNode<T> Owner { get { return _owner; } }

        internal TreeNodeCollection(TreeNode<T> owner)
        {
            ArgumentNullException.ThrowIfNull(owner);
            _owner = owner;
        }

        #region Collection implementation

        /// <summary>
        /// Provide the strongly typed member for ICollection.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(TreeNode<T>[] array, int index)
        {
            ((ICollection<TreeNode<T>>)this).CopyTo(array, index);
        }

        public new IEnumerator<TreeNode<T>> GetEnumerator()
        {
            foreach (TreeNode<T> node in InnerList)
                yield return node;
        }

        public void Insert(int index, TreeNode<T> node)
        {
            List.Insert(index, node);
        }

        public bool Contains(TreeNode<T> node)
        {
            return List.Contains(node);
        }


        /// <summary>
        /// Indexer accessing the index'th Node.
        /// If the owning node belongs to a tree, Setting the node fires a NodeChanged event
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TreeNode<T> this[int index]
        {
            get { return (TreeNode<T>)List[index]; }
            set { List[index] = value; }
        }

        /// <summary>
        /// Appends a new node with the specified value
        /// </summary>
        /// <param name="value">value for the new node</param>
        /// <returns>the node that was created</returns>
        public TreeNode<T> Add(T value)
        {
            TreeNode<T> n = new(value);
            List.Add(n);

            SendOwnerNodeChanged(TreeNodeEvent.ChildAdded, List.Count - 1);

            return n;
        }

        // required for XML Serializer, not to bad to have...
        public void Add(TreeNode<T> node)
        {
            List.Add(node);
            SendOwnerNodeChanged(TreeNodeEvent.ChildAdded, List.Count - 1);
        }

        /// <summary>
        /// Adds a range of nodes created from a range of values
        /// </summary>
        /// <param name="range">range of values </param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (T value in range)
                Add(value);
        }

        /// <summary>
        /// Adds a range of nodes created from a range of values passed as parameters
        /// </summary>
        public void AddRange(params T[] args)
        {
            AddRange(args as IEnumerable<T>);
        }


        /// <summary>
        /// Adds a new node with the given value at the specified index.
        /// </summary>
        /// <param name="index">Position where to insert the item.
        /// All values are accepted, if index is out of range, the new item is inserted as first or 
        /// last item</param>
        /// <param name="value">value for the new node</param>
        /// <returns></returns>
        public TreeNode<T> InsertAt(int index, T value)
        {
            TreeNode<T> n = new(value, _owner);

            // "tolerant insert"
            if (index < 0)
                index = 0;

            if (index >= Count)
            {
                index = Count;
                List.Add(n);
            }
            else
                List.Insert(index, n);

            SendOwnerNodeChanged(TreeNodeEvent.ChildAdded, index);

            return n;
        }

        /// <summary>
        /// Inserts a range of nodes created from a range of values
        /// </summary>
        /// <param name="index">index where to start inserting. As with InsertAt, all values areaccepted</param>
        /// <param name="values">a range of values set for the nodes</param>
        public void InsertRangeAt(int index, IEnumerable<T> values)
        {
            foreach (T value in values)
            {
                InsertAt(index, value);
                ++index;
            }
        }

        /// <summary>
        /// Inserts a new node before the specified node.
        /// </summary>
        /// <param name="insertPos">Existing node in front of which the new node is inserted</param>
        /// <param name="value">value for the new node</param>
        /// <returns>The newly created node</returns>
        public TreeNode<T> InsertBefore(TreeNode<T> insertPos, T value)
        {
            int index = IndexOf(insertPos);
            return InsertAt(index, value);
        }

        /// <summary>
        /// Inserts a new node after the specified node
        /// </summary>
        /// <param name="insertPos">Existing node after which the new node is inserted</param>
        /// <param name="value">value for the new node</param>
        /// <returns>The newly created node</returns>
        public TreeNode<T> InsertAfter(TreeNode<T> insertPos, T value)
        {
            int index = IndexOf(insertPos) + 1;
            if (index == 0)
                index = Count;
            return InsertAt(index, value);
        }

        public int IndexOf(TreeNode<T> node)
        {
            return List.IndexOf(node);
        }

        public void Remove(TreeNode<T> node)
        {
            int index = IndexOf(node);
            if (index < 0)
                throw new ArgumentException("the node to remove is not a in this collection");
            RemoveAt(index);
        }

        #endregion

        #region CollectionBase overrides (action handler)

        protected override void OnValidate(object value)
        {
            // Verify: value.Parent must be null or _owner)
            base.OnValidate(value);
            TreeNode<T> parent = ((TreeNode<T>)value).Parent;
            if (parent != null && parent != _owner)
                throw new ArgumentException("Cannot add a node referenced in another node collection");
        }

        protected override void OnInsert(int index, object value)
        {
            // set parent note to _owner
            ((TreeNode<T>)value).InternalSetParent(_owner);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ((TreeNode<T>)value).InternalDetach();

            SendOwnerNodeChanged(TreeNodeEvent.ChildRemoved, index);

            base.OnRemoveComplete(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                ((TreeNode<T>)oldValue).InternalDetach();
                ((TreeNode<T>)newValue).InternalSetParent(_owner);

            }
            base.OnSet(index, oldValue, newValue);
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            SendOwnerNodeChanged(TreeNodeEvent.NodeChanged, index);
            base.OnSetComplete(index, oldValue, newValue);
        }

        protected override void OnClear()
        {
            // set parent to null for all elements
            foreach (TreeNode<T> node in InnerList)
                node.InternalDetach();

            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            SendOwnerNodeChanged(TreeNodeEvent.ChildsCleared, 0);
            base.OnClearComplete();
        }

        #endregion // CollectionBase overrides

        #region protected helpers

        protected void SendOwnerNodeChanged(TreeNodeEvent changeHint, int index)
        {
            _owner.Root.SendNodeChanged(Owner, changeHint, index);
        }

        #endregion // Internal Helpers

        //* TODO:
        // * Exists(rpedicate), Find* 
        // * Swap (internal, expose at node?)
        // * Reverse, Sort
        // * TrueForAll
        // * 
        // */
    }
}

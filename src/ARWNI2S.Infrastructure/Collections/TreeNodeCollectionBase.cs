using System.Collections;
using ARWNI2S.Collections.Trees;

namespace ARWNI2S.Collections
{
    /// <summary>
    /// TreeNodeCollectionBase is an abstract class that can be used as a base class for a read-write collection that needs 
    /// to implement the generic IList&lt;T&gt; and non-generic IList collections. The derived class needs
    /// to override the following methods: Count, Clear, Insert, RemoveAt, and the indexer. The implementation
    /// of all the other methods in IList&lt;T&gt; and IList are handled by ListBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    internal abstract class TreeNodeCollectionBase<T> : CollectionBase<TreeNode<T>>, IList<TreeNode<T>>, IList
    {
        private TreeNode<T> _owner = null;

        private ArrayList _innerList;

        /// <summary>
        /// Gets the <see cref="TreeNode&lt;T&gt;" /> to which this collection belongs (this==Owner.Childs). 
        /// Never null.
        /// </summary>
        public TreeNode<T> Owner { get { return _owner; } }

        public override IEnumerator<TreeNode<T>> GetEnumerator()
        {
            foreach (TreeNode<T> node in _innerList)
                yield return node;
        }




        object IList.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        TreeNode<T> IList<TreeNode<T>>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        int IList<TreeNode<T>>.IndexOf(TreeNode<T> item)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList<TreeNode<T>>.Insert(int index, TreeNode<T> item)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void IList<TreeNode<T>>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}

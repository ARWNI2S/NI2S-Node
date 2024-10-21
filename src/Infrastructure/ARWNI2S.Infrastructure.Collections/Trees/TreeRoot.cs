namespace ARWNI2S.Infrastructure.Collections.Trees
{
    /// <summary>
    /// A TreeRoot object acts as source of tree node events. A single instance is associated
    /// with each tree (the Root property of all nodes of a tree return the same instance, nodes
    /// from different trees return different instances)
    /// </summary>
    /// <typeparam name="T">type of the data value at each tree node</typeparam>
    internal class TreeRoot<T>
    {
        private TreeNode<T> _root;

        internal TreeRoot(TreeNode<T> root)
        {
            _root = root;
        }

        public TreeNode<T> RootNode { get { return _root; } }

        /// <summary>
        /// Signals that a new value was assigned to a given node. <br/>
        /// Note: if T is a reference type and modified indirectly, this event doesn't fire
        /// </summary>
        public event EventHandler OnValueChanged;

        /// <summary>
        /// Signals that Node.Value was accessed.
        /// This can be used by a tree view controller to implement a defered 
        /// update even if T is a reference type and changed implicitely (i.e. 
        /// for cases where OnValueChanged does not fire)
        /// </summary>
        public event EventHandler OnValueAccessed;

        /// <summary>
        /// Signals that the Node structure has changed.
        /// </summary>
        public event EventHandler OnNodeChanged;

        #region Internal Helpers

        internal void SendValueChanged(TreeNode<T> node)
        {
            OnValueChanged?.Invoke(this, new TreeEventArgs<T>(node, TreeNodeEvent.ValueChanged, -1));
        }

        internal void SendValueAccessed(TreeNode<T> node)
        {
            OnValueAccessed?.Invoke(this, new TreeEventArgs<T>(node, TreeNodeEvent.ValueAccessed, -1));
        }

        internal void SendNodeChanged(TreeNode<T> node, TreeNodeEvent change, int index)
        {
            OnNodeChanged?.Invoke(this, new TreeEventArgs<T>(node, change, index));
        }

        #endregion
    }
}

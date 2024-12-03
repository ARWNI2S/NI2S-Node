using System.Xml.Serialization;

namespace ARWNI2S.Collections.Trees
{
    interface ITreeNodeMapper<T, V>
    {
        void UpdateNode(TreeNode<T> node, V viewNode);

        TreeNode<T> GetNodeInfo(V viewNode);
    }

    /// <summary>
    /// Represents a single Tree Node
    /// </summary>
    /// <typeparam name="T">Type of the Data Value at the node</typeparam>
    public class TreeNode<T>
    {
        private delegate string NodeToString(TreeNode<T> node);

        #region TreeNode Data
        private T _value;
        private TreeNode<T> _parent = null;
        private TreeNodeCollection<T> _nodes = null;
        private TreeRoot<T> _root = null;
        #endregion

        public TreeNode()
        {
            _value = default;
            _root = new TreeRoot<T>(this);
        }

        /// <summary>
        /// creates a new root node, and sets Value to value.
        /// </summary>
        /// <param name="value"></param>
        public TreeNode(T value)
        {
            _value = value;
            _root = new TreeRoot<T>(this);
        }

        /// <summary>
        /// Creates a new node as child of parent, and sets Value to value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        internal TreeNode(T value, TreeNode<T> parent)
        {
            _value = value;
            InternalSetParent(parent);
        }

        /// <summary>
        /// Node data
        /// Setting the value fires Tree[T].OnNodeChanged
        /// </summary>
        [XmlElement(ElementName = "InnerValue")]
        public T Value
        {
            get
            {
                _root.SendValueAccessed(this);
                return _value;
            }
            set
            {
                _value = value;
                _root.SendValueChanged(this);
            }
        }

        #region Navigation

        /// <summary>returns the parent node, or null if this is a root node</summary>
        [XmlIgnore]
        public TreeNode<T> Parent { get { return _parent; } }

        /// <summary>
        /// returns all siblings as a NodeList[T]. If this is a root node, the function returns null.
        /// </summary>
        [XmlIgnore]
        internal TreeNodeCollection<T> Siblings
        {
            get { return _parent != null ? _parent.Nodes : null; }
        }

        /// <summary>
        /// returns all child nodes as a NodeList[T]. 
        /// <para><b>Implementation note:</b> Childs always returns a non-null collection. 
        /// This collection is created on demand at the first access. To avoid unnecessary 
        /// creation of the collection, use HasChilds to check if the node has any child nodes</para>
        /// </summary>
        [XmlArrayItem("Node")]
        public TreeNodeCollection<T> Nodes
        {
            get
            {
                if (_nodes == null)
                    _nodes = new TreeNodeCollection<T>(this);
                return _nodes;
            }
        }

        /// <summary>
        /// The Root object this Node belongs to. never null
        /// </summary>
        [XmlIgnore]
        internal TreeRoot<T> Root { get { return _root; } }

        internal void SetRootLink(TreeRoot<T> root)
        {
            if (_root != root) // assume sub trees are consistent
            {
                _root = root;
                if (HasChildren)
                    foreach (TreeNode<T> n in Nodes)
                        n.SetRootLink(root);
            }
        }

        /// <summary>
        /// returns true if the node has child nodes.
        /// See also Implementation Note under Childs
        /// </summary>
        [XmlIgnore]
        public bool HasChildren { get { return _nodes != null && _nodes.Count != 0; } }

        /// <summary>
        /// returns true if this node is a root node. (Equivalent to Parent==null)
        /// </summary>
        [XmlIgnore]
        public bool IsRoot { get { return _parent == null; } }

        public bool IsAncestorOf(TreeNode<T> node)
        {
            if (node.Root != Root)
                return false; // different trees
            TreeNode<T> parent = node.Parent;
            while (parent != null && parent != this)
                parent = parent.Parent;
            return parent != null;
        }

        public bool IsChildOf(TreeNode<T> node)
        {
            return !IsAncestorOf(node);
        }

        public bool IsInLineWith(TreeNode<T> node)
        {
            return node == this ||
                   node.IsAncestorOf(this) ||
                   node.IsChildOf(node);
        }

        [XmlIgnore]
        public int Depth
        {
            get
            {
                int depth = 0;
                TreeNode<T> node = _parent;
                while (node != null)
                {
                    ++depth;
                    node = node._parent;
                }
                return depth;
            }
        }

        #endregion

        #region TreeNode Path

        public TreeNode<T> GetNodeAt(int index)
        {
            return Nodes[index];
        }

        public TreeNode<T> GetNodeAt(IEnumerable<int> index)
        {
            TreeNode<T> node = this;
            foreach (int elementIndex in index)
            {
                node = node.Nodes[elementIndex];
            }
            return node;
        }

        public TreeNode<T> GetNodeAt(params int[] index)
        {
            return GetNodeAt(index as IEnumerable<int>);
        }

        public int[] GetIndexPathTo(TreeNode<T> node)
        {
            if (Root != node.Root)
                throw new ArgumentException("parameter node must belong to the same tree");
            List<int> index = [];

            while (node != this && node._parent != null)
            {
                index.Add(node._parent.Nodes.IndexOf(node));
                node = node._parent;
            }

            if (node != this)
                throw new ArgumentException("node is not a child of this");

            index.Reverse();
            return [.. index];
        }

        public int[] GetIndexPath()
        {
            return Root.RootNode.GetIndexPathTo(this);
        }

        public System.Collections.IList GetNodePath()
        {
            List<TreeNode<T>> list = [];
            TreeNode<T> node = _parent;

            while (node != null)
            {
                list.Add(node);
                node = node.Parent;
            }
            list.Reverse();
            list.Add(this);

            return list;
        }

        public IList<T> GetElementPath()
        {
            List<T> list = [];
            TreeNode<T> node = _parent;

            while (node != null)
            {
                list.Add(node.Value);
                node = node.Parent;
            }
            list.Reverse();
            list.Add(_value);

            return list;
        }

        public string GetNodePathAsString(char separator)
        {
            return GetNodePathAsString(separator,
                     delegate (TreeNode<T> node) { return node.Value.ToString(); });
        }

        private string GetNodePathAsString(char separator, NodeToString toString)
        {
            string s = "";
            TreeNode<T> node = this;

            while (node != null)
            {
                if (s.Length != 0)
                    s = toString(node) + separator + s;
                else
                    s = toString(node);
                node = node.Parent;
            }

            return s;
        }

        #endregion

        #region Modify

        /// <summary>
        /// Removes the current node and all child nodes recursively from it's parent.
        /// Throws an InvalidOperationException if this is a root node.
        /// </summary>
        public void Remove()
        {
            if (_parent == null)
                throw new InvalidOperationException("cannot remove root node");
            Detach();
        }

        /// <summary>
        /// Detaches this node from it's parent. 
        /// Postcondition: this is a root node.
        /// </summary>
        /// <returns></returns>
        public TreeNode<T> Detach()
        {
            if (_parent != null)
                Siblings.Remove(this);

            return this;
        }

        #endregion

        #region Enumerators

        public IEnumerable<T> DepthFirstEnumerator
        {
            get
            {
                foreach (TreeNode<T> node in DepthFirstNodeEnumerator)
                    yield return node.Value;
            }
        }

        public IEnumerable<TreeNode<T>> DepthFirstNodeEnumerator
        {
            get
            {
                yield return this;
                if (_nodes != null)
                {
                    foreach (TreeNode<T> child in _nodes)
                    {
                        IEnumerator<TreeNode<T>> childEnum = child.DepthFirstNodeEnumerator.GetEnumerator();
                        while (childEnum.MoveNext())
                            yield return childEnum.Current;
                    }
                }
            }
        }

        public IEnumerable<TreeNode<T>> BreadthFirstNodeEnumerator
        {
            get
            {
                Queue<TreeNode<T>> todo = new();
                todo.Enqueue(this);
                while (0 < todo.Count)
                {
                    TreeNode<T> node = todo.Dequeue();
                    if (node._nodes != null)
                    {
                        foreach (TreeNode<T> child in node._nodes)
                            todo.Enqueue(child);
                    }
                    yield return node;
                }
            }
        }

        public IEnumerable<T> BreadthFirstEnumerator
        {
            get
            {
                foreach (TreeNode<T> node in BreadthFirstNodeEnumerator)
                    yield return node.Value;
            }
        }

        #endregion

        #region Internal Helpers

        internal void InternalDetach()
        {
            _parent = null;
            SetRootLink(new TreeRoot<T>(this));
        }

        internal void InternalSetParent(TreeNode<T> parent)
        {
            _parent = parent;
            if (_parent != null)
                SetRootLink(parent.Root);
        }

        #endregion
    }
}

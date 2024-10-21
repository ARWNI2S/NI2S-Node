namespace ARWNI2S.Infrastructure.Collections.Trees
{
    /// <summary>
    /// Stateful helper class to build simple tree structures.
    /// Provides the following methods:
    /// <list>
    /// <item>Add: Adds one or more nodes at the current level</item>
    /// <item>AddWithChild: Adds a new node and goes down one level</item>
    /// <item>Down: goes down one level</item>
    /// <item>Up: goes up one level</item>
    /// <item>sets the current level to the childs of the root node</item>
    /// <item>ToTree: resets the tree builder and returns the tree that was built</item>
    /// </list>
    /// 
    /// The TreeBuilder always generates a root node, and starts with root.Nodes as
    /// current level.
    /// 
    /// </summary>
    public class TreeBuilder<T>
    {
        private TreeNode<T> _root;
        private TreeNode<T> _current;

        public TreeNode<T> RootNode { get { return _root; } }
        public TreeNode<T> CurrentNode { get { return _current; } }

        public TreeBuilder()
        {
            Reset();
        }

        public TreeBuilder(T rootValue)
        {
            Reset();
            SetRootValue(rootValue);
        }

        public void Reset()
        {
            _root = new TreeNode<T>();
            _current = _root;
        }

        public TreeNode<T> ToTree()
        {
            TreeNode<T> result = _root;
            Reset();
            return result;
        }

        public TreeBuilder<T> Add(T value)
        {
            _current.Nodes.Add(value);
            return this;
        }

        public TreeBuilder<T> Add(IEnumerable<T> values)
        {
            _current.Nodes.AddRange(values);
            return this;
        }

        public TreeBuilder<T> Add(params T[] args)
        {
            return Add(args as IEnumerable<T>);
        }

        public TreeBuilder<T> Down()
        {
            _current = _current.Nodes[_current.Nodes.Count - 1];
            return this;
        }

        public TreeBuilder<T> AddWithChild(T value)
        {
            Add(value);
            Down();
            return this;
        }

        public TreeBuilder<T> Up()
        {
            _current = _current.Parent;
            return this;
        }

        public TreeBuilder<T> Root()
        {
            _current = _root;
            return this;
        }

        public TreeBuilder<T> SetRootValue(T value)
        {
            _root.Value = value;
            return this;
        }
    }
}

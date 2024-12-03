namespace ARWNI2S.Collections.Trees
{

    /// <summary>EventArgs for changes in a TreeNode</summary>
    public class TreeEventArgs<T> : EventArgs
    {
        public TreeEventArgs(TreeNode<T> node, TreeNodeEvent change, int index)
        {
            Node = node;
            Change = change;
            Index = index;
        }

        /// <summary>The Node for which the event occured.</summary>
        public TreeNode<T> Node;

        /// <summary>
        /// <list>
        ///   <item>ValueAccessed: the get - accessor for node.Value was called. index is unused</item>
        ///   <item>ValueChanged: A new value was assigned to node.Value. index is unused</item>
        ///   <item>NodeChanged: The node itself has changed (e.g. another node was assigned) All child nodes may have changed, too</item>
        ///   <item>ChildOrderChanged: the order of the elements of node.Childs has changed</item>
        ///   <item>ChildAdded: a child node was added at position <c>index</c></item>
        ///   <item>ChildRemoved: a child node was removed at position <c>index</c>.
        ///         This notification is <b>not</b> sent when all items are removed using Clear
        ///   </item>
        ///   <item>ChildsCleared: all childs were removed.</item>
        /// </list>
        /// </summary>
        public TreeNodeEvent Change;

        /// <summary>Index of the child node affected. See the Change member for more information.</summary>
        public int Index;
    }

}

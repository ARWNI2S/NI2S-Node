namespace ARWNI2S.Collections
{
    public interface ITree
    {
    }

    public interface ITree<T> : ITree, ICollection<ITreeNode<T>>
    {
    }
}

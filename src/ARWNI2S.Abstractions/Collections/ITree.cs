using System.Collections;

namespace ARWNI2S.Collections
{
    public interface ITree : ICollection
    {
    }

    public interface ITree<T> : ITree, ICollection<ITreeNode<T>>
    {
    }
}

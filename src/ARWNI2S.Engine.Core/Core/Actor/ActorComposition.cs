using ARWNI2S.Collections;
using ARWNI2S.Entities;
using System.Collections;

namespace ARWNI2S.Core.Actor
{
    public class ActorComposition : IActorComponents
    {
        public IActorComponent this[string name] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IActorComponent this[Guid key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public int Revision => throw new NotImplementedException();

        public void Add(ITreeNode<IActorComponent> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ITreeNode<IActorComponent> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ITreeNode<IActorComponent>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ITreeNode<IActorComponent>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(ITreeNode<IActorComponent> item)
        {
            throw new NotImplementedException();
        }

        TComponent IActorComponents.Get<TComponent>(string name)
        {
            throw new NotImplementedException();
        }

        TComponent IComposition<Guid, IActorComponent>.Get<TComponent>(Guid key)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<Guid, IActorComponent>> IEnumerable<KeyValuePair<Guid, IActorComponent>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void IComposition<Guid, IActorComponent>.Set<TComponent>(TComponent component)
        {
            throw new NotImplementedException();
        }
    }
}
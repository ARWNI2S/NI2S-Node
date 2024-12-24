using ARWNI2S.Engine.Actor;
using System.Collections;

namespace ARWNI2S.Engine.Components
{
    public sealed class ActorComponents : ICollection<IActorComponent>
    {
        public int Count { get; private set; }

        public bool IsReadOnly { get; private set; }

        public void Add(ActorComponent item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ActorComponent item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ActorComponent[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ActorComponent> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(ActorComponent item)
        {
            throw new NotImplementedException();
        }

        void ICollection<IActorComponent>.Add(IActorComponent item) => Add((ActorComponent)item);

        bool ICollection<IActorComponent>.Contains(IActorComponent item) => Contains((ActorComponent)item);

        void ICollection<IActorComponent>.CopyTo(IActorComponent[] array, int arrayIndex) => CopyTo((ActorComponent[])array, arrayIndex);

        IEnumerator<IActorComponent> IEnumerable<IActorComponent>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool ICollection<IActorComponent>.Remove(IActorComponent item) => Remove((ActorComponent)item);
    }
}
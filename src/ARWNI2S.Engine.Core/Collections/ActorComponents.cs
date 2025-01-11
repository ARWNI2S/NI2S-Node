using ARWNI2S.Engine.Core;
using System.Collections;

namespace ARWNI2S.Engine.Collections
{
    public class ActorComponents : IEnumerable<IActorComponent>
    {
        public IEnumerator<IActorComponent> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

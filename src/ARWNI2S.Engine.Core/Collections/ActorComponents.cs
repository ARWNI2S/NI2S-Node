// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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

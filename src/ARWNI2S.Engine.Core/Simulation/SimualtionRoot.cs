// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Core.Actor;
using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Simulation
{
    internal class SimualtionRoot : NI2SObject, ISimulation
    {
        internal ActorId ActorId { get; }

        protected ISyncRootGrain Self { get; }
    }
}

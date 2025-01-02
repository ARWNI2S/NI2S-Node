using ARWNI2S.Engine.Core.Object;
using Orleans;

namespace ARWNI2S.Engine.Core.Actor
{
    public abstract class NI2SActor : NI2SObject, INiisActor
    {
        public IEnumerable<IActorComponent> Components => throw new NotImplementedException();

        public Task OnActivateAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

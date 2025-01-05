using Orleans;

namespace ARWNI2S.Engine.Core
{
    public interface INiisActor : INiisObject
    {
        IEnumerable<IActorComponent> Components { get; }

        Task OnActivateAsync(CancellationToken cancellationToken);
        Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken);
    }
}
using ARWNI2S.Node.Runtime.Clustering.Services;

namespace ARWNI2S.Node.Runtime.Clustering
{
    // Implementation of the Local Actor Registry
    public class LocalActorRegistry : ILocalActorRegistry
    {
        private readonly HashSet<Guid> _localActors = new HashSet<Guid>();
        private readonly IGrainFactory _grainFactory;

        public LocalActorRegistry(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        public async Task RegisterLocalActor(Guid actorId)
        {
            if (_localActors.Add(actorId))
            {
                var directory = _grainFactory.GetGrain<IActorDirectoryService>(0);
                var siloAddress = this.GetSiloAddress(); // Method to retrieve the current silo's address
                await directory.RegisterActor(actorId, siloAddress);
            }
        }

        public async Task UnregisterLocalActor(Guid actorId)
        {
            if (_localActors.Remove(actorId))
            {
                var directory = _grainFactory.GetGrain<IActorDirectoryService>(0);
                await directory.UnregisterActor(actorId);
            }
        }

        public Task<IEnumerable<Guid>> GetLocalActors()
        {
            return Task.FromResult(_localActors.AsEnumerable());
        }

        private string GetSiloAddress()
        {
            return this.RuntimeIdentity; // Orleans provides this identity as the silo address
        }
    }
}

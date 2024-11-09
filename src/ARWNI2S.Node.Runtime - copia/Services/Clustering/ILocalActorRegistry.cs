namespace ARWNI2S.Runtime.Services.Clustering
{
    // Interface for the Local Actor Registry
    public interface ILocalActorRegistry
    {
        Task RegisterLocalActor(Guid actorId);
        Task UnregisterLocalActor(Guid actorId);
        Task<IEnumerable<Guid>> GetLocalActors();
    }
}
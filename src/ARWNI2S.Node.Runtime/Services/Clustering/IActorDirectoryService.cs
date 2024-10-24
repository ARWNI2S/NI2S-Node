﻿namespace ARWNI2S.Runtime.Services.Clustering
{
    public interface IActorDirectoryService : IGrainWithIntegerKey
    {
        Task RegisterActor(Guid actorId, string siloAddress);
        Task UnregisterActor(Guid actorId);
        Task<string> GetActorSilo(Guid actorId); // Return the silo where the actor is located
        Task<IEnumerable<Guid>> GetAllActors();  // Retrieve all registered actors
    }
}

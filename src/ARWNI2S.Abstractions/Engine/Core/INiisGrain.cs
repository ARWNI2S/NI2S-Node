using Orleans;

namespace ARWNI2S.Engine.Core
{
    public interface INiisGrain : IGrainWithGuidKey
    {
        Task<ActorState> GetStateAsync();
        Task UpdateStateAsync(string propertyName, object value);
        Task UpdateStateAsync(Dictionary<string, object> values);
        Task UpdateStateAsync(ActorState state);
        Task PersistStateAsync();
    }
}

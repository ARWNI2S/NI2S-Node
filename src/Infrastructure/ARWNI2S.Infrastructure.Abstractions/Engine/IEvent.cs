namespace ARWNI2S.Infrastructure.Engine
{
    public enum EventCacheMode
    {
        Disable,
        NoCache,
        DoNotPersist,
        Default,
        Forced
    }

    public interface IEvent
    {
        Guid SenderId { get; internal set; }

        int EventCode { get; internal set; }

        int EventTypeId { get; internal set; }

        EventCacheMode CacheControl { get; internal set; };

        string Tags { get; internal set; }

        void RegisterForDisposeAsync(object target);
    }
}

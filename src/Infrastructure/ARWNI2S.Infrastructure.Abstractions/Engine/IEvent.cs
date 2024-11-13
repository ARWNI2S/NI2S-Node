namespace ARWNI2S.Infrastructure.Engine
{
    public interface IEvent
    {
        void RegisterForDisposeAsync(object target);
    }
}

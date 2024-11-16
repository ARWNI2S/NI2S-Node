using ARWNI2S.Infrastructure.Engine;

namespace ARWNI2S.Infrastructure
{
    internal class NullCallback : IEvent
    {
        Guid _senderId = Constants._UUID_SYSTEM_HOST;
        int _eventCode = Constants.NULLEVENT_CODE;
        int _eventTypeId = Constants.NULLEVENT_TYPE;
        EventCacheMode _cacheControl = EventCacheMode.Disable;
        string _tags = string.Empty;

        Guid IEvent.SenderId { get => _senderId; set => _senderId = Constants._UUID_SYSTEM_HOST; }
        int IEvent.EventCode { get => _eventCode; set => _eventCode = Constants.NULLEVENT_CODE; }
        int IEvent.EventTypeId { get => _eventTypeId; set => _eventTypeId = Constants.NULLEVENT_TYPE; }
        EventCacheMode IEvent.CacheControl { get => _cacheControl; set => _cacheControl = EventCacheMode.Disable; }
        string IEvent.Tags { get => _tags; set => _tags = value; }

        private void RegisterForDisposeAsync(object target)
        {

        }

        void IEvent.RegisterForDisposeAsync(object target) => RegisterForDisposeAsync(target);
    }
}
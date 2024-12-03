
using ARWNI2S.Engine;

namespace ARWNI2S.Infrastructure
{
    public static class EngineActivator
    {
        public static T CreateInstance<T>() => (T)NI2SEngineContext.Current.ResolveUnregistered(typeof(T));
    }
}
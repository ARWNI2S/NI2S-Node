using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Core.Extensions
{
    public static class CoreINiisObjectExtensions
    {
        public static bool IsValid(this INiisObject obj, bool evenIfPendingKill, bool threadSafeTest = false)
        {
            if (obj is not NI2SObject niisObj)
                throw new ArgumentException($"{nameof(obj)} must inherit from {typeof(NI2SObject)}, but it is a {obj.GetType()} instance.");

            return ((NI2SObject)obj).IsValid(evenIfPendingKill, threadSafeTest);
        }
    }
}

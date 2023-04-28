using System;

namespace NI2S.Node.Core
{
    internal static class GUID64
    {
        private const uint _maxTagValue = 1048575;
        private const uint _maxStampValue = 4095;

        public static void Register(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if(type is not IEntity) throw new ArgumentException("TD", nameof(type));


        }

        public static ulong New<TEntity>()
            where TEntity : class, IEntity
        {
            throw new NotImplementedException();
        }
    }
}

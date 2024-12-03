using ARWNI2S.Core.Entities;

namespace ARWNI2S.Core.Object
{
    internal class NiisObjectFactory<TObject> : INiisObjectFactory where TObject : NiisObject, new()
    {
        private static readonly Dictionary<Type, int> _registeredTypes = [];

        private readonly int _fatoryId;

        public NiisObjectFactory() : this(typeof(TObject)) { }

        private NiisObjectFactory(Type objectType)
        {
            if (!_registeredTypes.TryGetValue(objectType, out _fatoryId))
            {
                _registeredTypes.Add(objectType, _fatoryId = NI2SUUID.GetObjectTypeId(objectType));
            }

        }
    }
}
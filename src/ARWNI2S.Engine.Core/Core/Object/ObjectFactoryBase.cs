// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.


namespace ARWNI2S.Engine.Core.Object
{
    public abstract class ObjectFactoryBase : IObjectFactory
    {
        public abstract TObject CreateInstance<TObject>() where TObject : INiisObject;

        public abstract INiisObject CreateInstance(Type type);
    }
}

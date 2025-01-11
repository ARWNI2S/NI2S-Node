// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Core.Object
{
    internal class DefaultObjectFactory : ObjectFactoryBase, IObjectFactory<INiisObject>
    {
        public override TObject CreateInstance<TObject>()
        {
            return (TObject)CreateInstance(typeof(TObject));
        }

        public override INiisObject CreateInstance(Type type)
        {
            return (INiisObject)Activator.CreateInstance(type);
        }

        public INiisObject CreateInstance() { throw new InvalidOperationException(); }
    }
}

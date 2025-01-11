// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Core.Object
{
    public abstract class NI2SObject : ObjectBase, INiisObject
    {
        public static T New<T>() where T : INiisObject
        {
            // Resolver la factoría específica o usar la predeterminada
            var factory = EngineContext.Current.Resolve<IObjectFactory<T>>();

            // Crear la instancia con el tipo concreto
            return factory is IObjectFactory<T> specificFactory
                ? specificFactory.CreateInstance()
                : factory.CreateInstance<T>();
        }
    }
}

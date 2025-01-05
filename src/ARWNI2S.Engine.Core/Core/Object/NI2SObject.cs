namespace ARWNI2S.Engine.Core.Object
{
    public abstract class NI2SObject : ObjectBase, INiisObject
    {
        public static T New<T>() where T : NI2SObject
        {
            var factory = Singleton<IObjectFactory<T>>.Instance;
            return factory.CreateInstance();
        }
    }
}

namespace ARWNI2S.Engine.Core.Object
{
    public abstract class ObjectFactoryBase : IObjectFactory
    {
        public abstract TObject CreateInstance<TObject>() where TObject : INiisObject;

        public abstract INiisObject CreateInstance(Type type);
    }
}

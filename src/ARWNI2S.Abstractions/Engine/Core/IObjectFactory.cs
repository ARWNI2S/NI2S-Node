namespace ARWNI2S.Engine.Core
{
    public interface IObjectFactory
    {
        TObject CreateInstance<TObject>() where TObject : INiisObject;

        INiisObject CreateInstance(Type type);
    }

    public interface IObjectFactory<TObject> : IObjectFactory where TObject : INiisObject
    {
        TObject CreateInstance();
    }
}
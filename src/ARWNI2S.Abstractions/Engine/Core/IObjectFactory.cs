namespace ARWNI2S.Engine.Core
{
    internal interface IObjectFactory<TObject> where TObject : INiisObject
    {
        TObject CreateInstance();
    }
}
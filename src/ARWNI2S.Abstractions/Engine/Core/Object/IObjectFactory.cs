namespace ARWNI2S.Engine.Core.Object
{
    internal interface IObjectFactory<TObject> where TObject : INiisObject
    {
        TObject CreateInstance();
    }
}
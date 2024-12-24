namespace ARWNI2S.Engine.Core.Object
{
    public class DefaultObjectFactory<TObject> : IObjectFactory<TObject> where TObject : INiisObject
    {
        public TObject CreateInstance()
        {
            return default;
        }
    }
}

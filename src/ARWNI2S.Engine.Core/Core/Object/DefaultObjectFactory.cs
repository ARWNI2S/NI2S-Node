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

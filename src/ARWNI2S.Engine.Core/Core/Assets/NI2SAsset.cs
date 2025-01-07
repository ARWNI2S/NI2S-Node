using ARWNI2S.Engine.Resources;

namespace ARWNI2S.Engine.Core.Assets
{
    public abstract class NI2SAsset : ResourceBase
    {
        private readonly IRepository<TData> _repository;

        public TData Data { get; private set; }

        public DataResource(string key, IRepository<TData> repository) : base(key)
        {
            _repository = repository;
        }

        protected override bool Create()
        {
            throw new NotImplementedException();
        }

        protected override void Destroy()
        {
            throw new NotImplementedException();
        }

        protected override void OnCreated()
        {
            base.OnCreated();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        //public string Name { get; }
        //public AssetId Id { get; }

        //override public int Size { get; }
        //override public void Clear() { }
        //override public bool Create() { return true; }
        //override public void Destroy() { }
        //override public bool Recreate() { return true; }
        //public NI2SAsset() { }
        //override protected void Dispose(bool disposing) { }
        //override public string ToString() { return ""; }
        //override public bool Equals(object obj) { return false; }
        //override public int GetHashCode() { return 0; }

        //object INiisEntity.Id => Id;

    }
}

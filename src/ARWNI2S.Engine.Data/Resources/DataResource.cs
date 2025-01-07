using ARWNI2S.Data;
using ARWNI2S.Engine.Resources;

namespace ARWNI2S.Engine.Data.Resources
{
    public class DataResource<TData> : ResourceBase
        where TData : class, IDataEntity
    {
        private readonly IRepository<TData> _repository;
        public TData Data { get; private set; }
        public DataResource(IRepository<TData> repository)
        {
            _repository = repository;
        }
        protected override bool Create()
        {
            Data = _repository.GetById(GetIdentity<int>());
            return Data is not null;
        }
        protected override void Destroy()
        {
            Data = null;
        }
    }
}

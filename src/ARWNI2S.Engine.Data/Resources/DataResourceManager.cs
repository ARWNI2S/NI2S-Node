using ARWNI2S.Engine.Environment.Internal;
using ARWNI2S.Engine.Resources;
using System.Diagnostics;

namespace ARWNI2S.Engine.Data.Resources
{
    public class DataResourceManager<TData> : ResourceManagerBase<DataResource<TData>>
        where TData : DataEntity
    {
        private readonly IRepository<TData> _repository;

        public DataResourceManager(IRepository<TData> repository)
            : base() { _repository = repository; }

        public TData GetData(int key)
        {
            var handle = Get(key);
            var resource = GetResource(handle);
            return resource.Data;
        }


        protected override DataResource<TData> CreateEmpty()
        {
            return new DataResource<TData>(_repository);
        }
    }
}

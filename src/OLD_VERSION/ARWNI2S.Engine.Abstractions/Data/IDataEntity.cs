using ARWNI2S.Engine.Core;

namespace ARWNI2S.Engine.Data
{
    public interface IDataEntity : INiisEntity
    {
        new int Id { get; }

        object INiisEntity.Id => Id;
    }
}

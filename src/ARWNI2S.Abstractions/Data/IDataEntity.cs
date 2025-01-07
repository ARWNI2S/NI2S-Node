using ARWNI2S.Engine.Core;

namespace ARWNI2S.Data
{
    public interface IDataEntity : INiisEntity
    {
        new int Id { get; set; }
    }
}

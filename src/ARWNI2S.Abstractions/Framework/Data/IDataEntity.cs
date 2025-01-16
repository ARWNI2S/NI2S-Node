using ARWNI2S.Engine.Core;

namespace ARWNI2S.Framework.Data
{
    public interface IDataEntity : INiisEntity
    {
        new int Id { get; set; }
    }
}

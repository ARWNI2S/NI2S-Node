using ARWNI2S.Entities;

namespace ARWNI2S.Core.Object
{
    public interface IObjectInitializer
    {
        IObjectEntity Target { get; set; }
    }
}
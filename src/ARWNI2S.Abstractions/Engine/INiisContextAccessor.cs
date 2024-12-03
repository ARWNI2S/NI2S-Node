using ARWNI2S.Infrastructure;

namespace ARWNI2S.Engine
{
    public interface INiisContextAccessor
    {
        NiisContext NiisContext { get; set; }
    }
}
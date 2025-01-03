
namespace ARWNI2S.Engine
{
    public interface INiisContext
    {
        IServiceProvider ServiceProvider { get; }
        bool IsRequest { get; }
    }
}
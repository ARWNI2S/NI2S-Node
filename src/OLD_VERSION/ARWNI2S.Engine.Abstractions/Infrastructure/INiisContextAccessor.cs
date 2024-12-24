namespace ARWNI2S.Engine.Infrastructure
{
    public interface INiisContextAccessor
    {
        INiisContext ExecutionContext { get; set; }
    }
}

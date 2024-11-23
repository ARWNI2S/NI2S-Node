namespace ARWNI2S.Node.Engine
{
    public interface IScopedContextAccessor
    {
        ScopedContext CurrentContext { get; set; }
    }
}
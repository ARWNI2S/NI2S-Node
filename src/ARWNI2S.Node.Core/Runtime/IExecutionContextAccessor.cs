namespace ARWNI2S.Node.Core.Runtime
{
    public interface IExecutionContextAccessor
    {
        IExecutionContext ExecutionContext { get; set; }
    }
}

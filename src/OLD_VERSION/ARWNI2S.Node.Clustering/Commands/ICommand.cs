namespace ARWNI2S.Clustering.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface IClusterCommand : ICommand
    {
        Task ExecuteAsync();
    }

    public interface INodeCommand : ICommand
    {
        Guid NodeId { get; }
    }
}

namespace ARWNI2S.Infrastructure.Network.Connection
{
    public interface IConnectionFactory
    {
        Task<IConnection> CreateConnection(object connection, CancellationToken cancellationToken);
    }
}
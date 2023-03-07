namespace NI2S.Node.Protocol.Channel
{
    public delegate void NewClientAcceptHandler(IChannelCreator listener, IChannel channel);

    public interface IChannelCreator
    {
        ListenOptions Options { get; }

        bool Start();

        event NewClientAcceptHandler NewClientAccepted;

        Task<IChannel> CreateChannel(object connection);

        Task StopAsync();

        bool IsRunning { get; }
    }
}
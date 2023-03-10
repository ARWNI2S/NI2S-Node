namespace NI2S.Node.Protocol.Channel
{
    public interface IChannelRegister
    {
        Task RegisterChannel(object connection);
    }
}
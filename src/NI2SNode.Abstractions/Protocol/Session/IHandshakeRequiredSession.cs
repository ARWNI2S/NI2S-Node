namespace NI2S.Node.Protocol.Session
{
    public interface IHandshakeRequiredSession
    {
        bool Handshaked { get; }
    }
}
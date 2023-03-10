namespace NI2S.Node.Protocol.Session
{
    public interface ISessionFactory
    {
        ISession Create();

        Type SessionType { get; }
    }
}
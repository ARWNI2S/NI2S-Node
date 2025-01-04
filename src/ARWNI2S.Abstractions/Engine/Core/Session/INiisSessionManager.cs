namespace ARWNI2S.Engine.Core.Session
{
    public interface INiisSessionManager
    {
        INiisSession CreateSession(Guid playerId);
    }
}

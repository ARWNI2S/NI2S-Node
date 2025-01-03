namespace ARWNI2S.Engine.Session
{
    public interface INiisSessionManager
    {
        INiisSession CreateSession(Guid playerId);
    }
}

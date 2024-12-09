using ARWNI2S.Node.Core.Entities.Session;

namespace ARWNI2S.Node.Services.Session
{
    internal class NodeUserSessionHelper
    {
        /// <summary>
        /// Gets if a session has expired.
        /// </summary>
        /// <param name="userSession">session to check</param>
        /// <returns>True if the session has expired</returns>
        internal static bool SessionHasExpired(SessionRecord userSession)
        {
            return !((userSession.State == ConnectionState.Connected || userSession.State == ConnectionState.Playing) && 
                     (userSession.ExpiresOnUtc.HasValue && userSession.ExpiresOnUtc.Value > DateTime.UtcNow));
        }
    }
}

using ARWNI2S.Node.Core.Entities.Session;
using ARWNI2S.Node.Core.Entities.Users;

namespace ARWNI2S.Node.Services.Session
{
    public interface ISessionService
    {
        Task<SessionRecord> GetOrCreateSessionAsync(User user, string connectionId);

        Task<User> GetUserBySessionIdAsync(string sessionId);

        Task<SessionRecord> GetUserSessionAsync(User user);
    }
}

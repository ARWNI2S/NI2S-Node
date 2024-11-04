using ARWNI2S.Node.Services.Authentication;
using System.Security.Claims;

namespace ARWNI2S.Node.Services.Session
{
    public interface ISessionStateService
    {
        Task<AuthenticateResult> AuthenticateAsync(string sessionId, string scheme);
        Task SignInAsync(string sessionId, ClaimsPrincipal principal, AuthenticationProperties authenticationProperties);
        Task SignOutAsync(string sessionId);
        Task<SessionState> GetSessionStateAsync(string sessionId);
        Task UpdateSessionStateAsync(string sessionId, SessionState state);
    }
}

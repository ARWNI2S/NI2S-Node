using ARWNI2S.Node.Services.Authentication;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ARWNI2S.Node.Services.Session
{
    public class SessionStateService : ISessionStateService
    {
        private readonly ConcurrentDictionary<string, SessionState> _sessionStates = new();

        public Task SignInAsync(string sessionId, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            var state = new SessionState
            {
                ConnectionState = SessionConnectionState.Connected,
                UserPrincipal = principal,
                Properties = properties,
                IssuedUtc = DateTime.UtcNow // Establecer la fecha de emisión al momento del inicio de sesión
            };

            _sessionStates[sessionId] = state;
            return Task.CompletedTask;
        }

        public Task SignOutAsync(string sessionId)
        {
            _sessionStates.TryRemove(sessionId, out _);
            return Task.CompletedTask;
        }

        public Task<SessionState> GetSessionStateAsync(string sessionId)
        {
            _sessionStates.TryGetValue(sessionId, out var state);
            return Task.FromResult(state);
        }

        public Task UpdateSessionStateAsync(string sessionId, SessionState state)
        {
            _sessionStates[sessionId] = state;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync(string sessionId, string scheme)
        {
            // Verificar si existe un estado de sesión para el sessionId
            if (_sessionStates.TryGetValue(sessionId, out var state) &&
                state.ConnectionState != SessionConnectionState.Disconnected)
            {
                return Task.FromResult(AuthenticateResult.Success(state.UserPrincipal, state.IssuedUtc));
            }

            return Task.FromResult(AuthenticateResult.Fail("No se encontró la sesión para el ID especificado"));
        }
    }
}

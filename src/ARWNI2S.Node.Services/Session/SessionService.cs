using ARWNI2S.Node.Core.Entities.Session;
using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Extensions;
using ARWNI2S.Node.Services.Common;
using ARWNI2S.Node.Services.Users;

namespace ARWNI2S.Node.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly ISessionStateService _sessionStateService;
        private readonly IUserService _userService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IRepository<SessionRecord> _userSessionRepository;

        public SessionService(
            ISessionStateService sessionStateService,
            IUserService userService,
            IGenericAttributeService genericAttributeService,
            IRepository<SessionRecord> userSessionRepository)
        {
            _sessionStateService = sessionStateService;
            _userService = userService;
            _genericAttributeService = genericAttributeService;
            _userSessionRepository = userSessionRepository;
        }

        public async Task<SessionRecord> GetOrCreateSessionAsync(User user, string connectionId)
        {
            // Comprobar si ya existe una sesión activa para el usuario
            var userSession = await GetUserSessionAsync(user);
            if (userSession == null)
            {
                userSession = new SessionRecord
                {
                    UserId = user.Id,
                    State = ConnectionState.Disconnected,
                    CreatedOnUtc = DateTime.UtcNow
                };
                await _userSessionRepository.InsertAsync(userSession);
            }
            else if (userSession.State == ConnectionState.Error || !NodeUserSessionHelper.SessionHasExpired(userSession))
            {
                var sessionState = await _sessionStateService.GetSessionStateAsync(userSession.SessionId);
                if (sessionState != null)
                {

                }
            }

            userSession.SessionId = connectionId;
            userSession.UpdatedOnUtc = DateTime.UtcNow;
            userSession.ExpiresOnUtc = DateTime.UtcNow.AddHours(1);

            await _userSessionRepository.UpdateAsync(userSession);

            await _genericAttributeService.SaveAttributeAsync(user, SessionServicesDefaults.SessionAttributeKey, userSession.SessionId);

            return userSession;
        }

        public async Task<User> GetUserBySessionIdAsync(string sessionId)
        {
            // Buscar la sesión por sessionId y verificar que no haya expirado
            var session = await _userSessionRepository.Table
                .Where(s => s.SessionId == sessionId && s.ExpiresOnUtc > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (session == null)
            {
                return null; // Sesión no encontrada o expirada
            }

            // Obtener el usuario usando el UserId de la sesión
            return await _userService.GetUserByIdAsync(session.UserId);
        }

        public async Task<SessionRecord> GetUserSessionAsync(User user)
        {
            // Recuperar el sessionId del atributo del usuario si existe
            var sessionId = await _genericAttributeService.GetAttributeAsync<string>(user, SessionServicesDefaults.SessionAttributeKey);

            if (string.IsNullOrEmpty(sessionId))
            {
                return null; // El usuario no tiene una sesión registrada en los atributos
            }

            // Buscar la sesión en la base de datos y verificar que esté activa
            return await _userSessionRepository.Table
                .Where(s => s.UserId == user.Id && s.SessionId == sessionId && s.ExpiresOnUtc > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }
    }
}

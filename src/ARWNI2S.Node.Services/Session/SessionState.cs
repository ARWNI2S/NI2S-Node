using ARWNI2S.Node.Services.Authentication;
using System.Security.Claims;

namespace ARWNI2S.Node.Services.Session
{
    public class SessionState
    {
        public string SessionId { get; set; }
        public ClaimsPrincipal UserPrincipal { get; set; }
        public AuthenticationProperties Properties { get; set; }
        public InGameState GameState { get; set; } = null;
        public DateTime IssuedUtc { get; set; }
    }

    public class InGameState
    {
        public string CharacterId { get; set; }
        public string SceneId { get; set; }
    }
}

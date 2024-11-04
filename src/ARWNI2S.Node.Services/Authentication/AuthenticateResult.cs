using System.Security.Claims;

namespace ARWNI2S.Node.Services.Authentication
{
    public class AuthenticateResult
    {
        public bool Succeeded { get; private set; }
        public ClaimsPrincipal Principal { get; private set; }
        public string ErrorMessage { get; private set; }
        public string WarningMessage { get; private set; }
        public DateTime? IssuedUtc { get; private set; } = null;  // Fecha de emisión

        // Métodos estáticos para crear instancias de resultado
        public static AuthenticateResult Success(ClaimsPrincipal principal, DateTime issuedUtc) =>
            new AuthenticateResult { Succeeded = true, Principal = principal, IssuedUtc = issuedUtc };

        public static AuthenticateResult Fail(string errorMessage) =>
            new AuthenticateResult { Succeeded = false, ErrorMessage = errorMessage };

        public static AuthenticateResult Warn(string warningMessage, ClaimsPrincipal principal, DateTime issuedUtc) =>
            new AuthenticateResult { Succeeded = true, WarningMessage = warningMessage, Principal = principal, IssuedUtc = issuedUtc };
    }
}

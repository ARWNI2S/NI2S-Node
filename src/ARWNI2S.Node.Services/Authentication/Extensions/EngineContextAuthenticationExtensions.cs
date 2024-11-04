using ARWNI2S.Infrastructure.Engine;
using ARWNI2S.Node.Services.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace ARWNI2S.Node.Services.Authentication.Extensions
{
    internal static class EngineContextAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate the current request using the default authentication scheme.
        /// The default authentication scheme can be configured using <see cref="AuthenticationServicesDefaults.AuthenticationScheme"/>.
        /// </summary>
        /// <param name="context">The <see cref="IEngineContext"/> context.</param>
        /// <returns>The <see cref="AuthenticateResult"/>.</returns>
        public static Task<AuthenticateResult> AuthenticateAsync(this IEngineContext context, string sessionId) =>
            context.AuthenticateAsync(sessionId, scheme: null);

        /// <summary>
        /// Authenticate the current request using the specified scheme.
        /// </summary>
        /// <param name="context">The <see cref="IEngineContext"/> context.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <returns>The <see cref="AuthenticateResult"/>.</returns>
        public static Task<AuthenticateResult> AuthenticateAsync(this IEngineContext context, string sessionId, string scheme) =>
            GetSessionStateService(context).AuthenticateAsync(sessionId, scheme);

        public static Task SignInAsync(this IEngineContext context, string sessionId, ClaimsPrincipal principal, AuthenticationProperties properties = null) =>
            GetSessionStateService(context).SignInAsync(sessionId, principal, properties);

        public static Task SignOutAsync(this IEngineContext context, string sessionId) =>
            GetSessionStateService(context).SignOutAsync(sessionId);

        public static Task<SessionState> GetSessionStateAsync(this IEngineContext context, string sessionId) =>
            GetSessionStateService(context).GetSessionStateAsync(sessionId);

        public static Task UpdateSessionStateAsync(this IEngineContext context, string sessionId, SessionState state) =>
            GetSessionStateService(context).UpdateSessionStateAsync(sessionId, state);

        private static ISessionStateService GetSessionStateService(IEngineContext context) =>
            context.ContextServices.GetService<ISessionStateService>() ??
            throw new InvalidOperationException("Unable to find ISessionStateService. Make sure it is registered.");
    }
}

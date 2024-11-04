using ARWNI2S.Infrastructure.Engine;
using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Services.Authentication.Extensions;
using ARWNI2S.Node.Services.Security;
using ARWNI2S.Node.Services.Users;
using System.Security.Claims;

namespace ARWNI2S.Node.Services.Authentication
{
    public partial class TokenAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;
        private readonly IEngineContextAccessor _contextAccessor;

        private User _cachedUser;

        #endregion

        #region Ctor

        internal TokenAuthenticationService(UserSettings userSettings,
            IUserService userService,
            IUserTokenService userTokenService,
            IEngineContextAccessor contextAccessor)
        {
            _userSettings = userSettings;
            _userService = userService;
            _userTokenService = userTokenService;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SignInAsync(User user, bool isPersistent)
        {
            ArgumentNullException.ThrowIfNull(user);

            var token = _userTokenService.GenerateToken(user);

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(token.Claims, AuthenticationServicesDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                Token = _userTokenService.SerializeToken(token),
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _contextAccessor.EngineContext.SignInAsync(AuthenticationServicesDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //cache authenticated user
            _cachedUser = user;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SignOutAsync()
        {
            //reset cached user
            _cachedUser = null;

            //and sign out from the current authentication scheme
            await _contextAccessor.EngineContext.SignOutAsync(AuthenticationServicesDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get authenticated user
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the user
        /// </returns>
        public virtual async Task<User> GetAuthenticatedUserAsync()
        {
            //whether there is a cached user
            if (_cachedUser != null)
                return _cachedUser;

            //try to get authenticated user identity
            var authenticateResult = await _contextAccessor.EngineContext.AuthenticateAsync(AuthenticationServicesDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            User user = null;
            if (_userSettings.UsernamesEnabled)
            {
                //try to get user by username
                var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                    && claim.Issuer.Equals(AuthenticationServicesDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (usernameClaim != null)
                    user = await _userService.GetUserByUsernameAsync(usernameClaim.Value);
            }
            else
            {
                //try to get user by email
                var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(AuthenticationServicesDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (emailClaim != null)
                    user = await _userService.GetUserByEmailAsync(emailClaim.Value);
            }

            //whether the found user is available
            if (user == null || !user.Active || user.RequireReLogin || user.Deleted || !await _userService.IsRegisteredAsync(user))
                return null;

            //get the latest password
            var userPassword = await _userService.GetCurrentPasswordAsync(user.Id);
            //required a user to re-login after password changing
            if (userPassword.CreatedOnUtc.CompareTo(authenticateResult.IssuedUtc.HasValue
                ? authenticateResult.IssuedUtc.Value
                : DateTime.UtcNow) > 0)
                return null;

            //cache authenticated user
            _cachedUser = user;

            return _cachedUser;
        }

        #endregion
    }
}

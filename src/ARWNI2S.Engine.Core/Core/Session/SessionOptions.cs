using ARWNI2S.Engine.Security.Token;

namespace ARWNI2S.Engine.Core.Session
{
    /// <summary>
    /// Represents the session state options for the application.
    /// </summary>
    public class SessionOptions
    {
        private TokenBuilder _tokenBuilder = new SessionTokenBuilder();

        /// <summary>
        /// Determines the settings used to create the token.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description><see cref="TokenBuilder.Name"/> defaults to <see cref="SessionDefaults.TokenName"/>.</description></item>
        /// <item><description><see cref="TokenBuilder.Path"/> defaults to <see cref="SessionDefaults.TokenPath"/>.</description></item>
        /// <item><description><see cref="TokenBuilder.HttpCookie"/> defaults to <c>true</c>.</description></item>
        /// <item><description><see cref="TokenBuilder.IsEssential"/> defaults to <c>false</c>.</description></item>
        /// </list>
        /// </remarks>
        public TokenBuilder Token
        {
            get => _tokenBuilder;
            set => _tokenBuilder = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The IdleTimeout indicates how long the session can be idle before its contents are abandoned. Each session access
        /// resets the timeout. Note this only applies to the content of the session, not the token.
        /// </summary>
        public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromMinutes(20);

        /// <summary>
        /// The maximum amount of time allowed to load a session from the store or to commit it back to the store.
        /// Note this may only apply to asynchronous operations. This timeout can be disabled using <see cref="Timeout.InfiniteTimeSpan"/>.
        /// </summary>
        public TimeSpan IOTimeout { get; set; } = TimeSpan.FromMinutes(1);

        private sealed class SessionTokenBuilder : TokenBuilder
        {
            public SessionTokenBuilder()
            {
                Name = SessionDefaults.TokenName;
                Path = SessionDefaults.TokenPath;
                SecurePolicy = TokenSecurePolicy.None;
                //SameSite = SameSiteMode.Lax;
                HttpCookie = true;
                // HttpOnly = true;
                // Session is considered non-essential as it's designed for ephemeral data.
                IsEssential = false;
            }

            public override TimeSpan? Expiration
            {
                get => null;
                set => throw new InvalidOperationException(nameof(Expiration) + " cannot be set for the token defined by " + nameof(SessionOptions));
            }
        }
    }

    internal class SessionDefaults
    {
        public static string TokenName => ".ARWNI2S.Session";
        public static string TokenPath => "/";
    }
}

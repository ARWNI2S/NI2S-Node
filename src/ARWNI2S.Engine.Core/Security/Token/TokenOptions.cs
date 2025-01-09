namespace ARWNI2S.Engine.Security.Token
{
    /// <summary>
    /// Options used to create a new token.
    /// </summary>
    /// <remarks>
    /// A <see cref="TokenOptions"/> instance is intended to govern the behavior of an individual token.
    /// Reusing the same <see cref="TokenOptions"/> instance across multiple tokens can lead to unintended
    /// consequences, such as modifications affecting multiple tokens. We recommend instantiating a new
    /// <see cref="TokenOptions"/> object for each token to ensure that the configuration is applied
    /// independently.
    /// </remarks>
    public class TokenOptions
    {
        private List<string> _extensions;

        /// <summary>
        /// Creates a default token with a path of '/'.
        /// </summary>
        public TokenOptions()
        {
            Path = "/";
        }

        /// <summary>
        /// Creates a copy of the given <see cref="TokenOptions"/>.
        /// </summary>
        public TokenOptions(TokenOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            Domain = options.Domain;
            Path = options.Path;
            Expires = options.Expires;
            Secure = options.Secure;
            //SameSite = options.SameSite;
            HttpCookie = options.HttpCookie;
            MaxAge = options.MaxAge;
            IsEssential = options.IsEssential;

            if (options._extensions?.Count > 0)
            {
                _extensions = new List<string>(options._extensions);
            }
        }

        /// <summary>
        /// Gets or sets the domain to associate the token with.
        /// </summary>
        /// <returns>The domain to associate the token with.</returns>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the token path.
        /// </summary>
        /// <returns>The token path.</returns>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time for the token.
        /// </summary>
        /// <returns>The expiration date and time for the token.</returns>
        public DateTimeOffset? Expires { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to transmit the token using Secure Sockets Layer (SSL)--that is, over HTTPS only.
        /// </summary>
        /// <returns>true to transmit the token only over an SSL connection (HTTPS); otherwise, false.</returns>
        public bool Secure { get; set; }

        ///// <summary>
        ///// Gets or sets the value for the SameSite attribute of the token. The default value is <see cref="SameSiteMode.Unspecified"/>
        ///// </summary>
        ///// <returns>The <see cref="SameSiteMode"/> representing the enforcement mode of the token.</returns>
        //public SameSiteMode SameSite { get; set; } = SameSiteMode.Unspecified;

        /// <summary>
        /// Gets or sets a value that indicates whether a token is inaccessible by client-side script.
        /// </summary>
        /// <returns>true if a token must not be accessible by client-side script; otherwise, false.</returns>
        public bool HttpCookie { get; set; }

        /// <summary>
        /// Gets or sets the max-age for the token.
        /// </summary>
        /// <returns>The max-age date and time for the token.</returns>
        public TimeSpan? MaxAge { get; set; }

        /// <summary>
        /// Indicates if this token is essential for the application to function correctly. If true then
        /// consent policy checks may be bypassed. The default value is false.
        /// </summary>
        public bool IsEssential { get; set; }

        /// <summary>
        /// Gets a collection of additional values to append to the token.
        /// </summary>
        public IList<string> Extensions
        {
            get => _extensions ??= [];
        }

        ///// <summary>
        ///// Creates a <see cref="SetTokenHeaderValue"/> using the current options.
        ///// </summary>
        //public SetTokenHeaderValue CreateTokenHeader(string name, string value)
        //{
        //    var token = new SetTokenHeaderValue(name, value)
        //    {
        //        Domain = Domain,
        //        Path = Path,
        //        Expires = Expires,
        //        Secure = Secure,
        //        HttpOnly = HttpOnly,
        //        MaxAge = MaxAge,
        //        SameSite = (Net.Http.Headers.SameSiteMode)SameSite,
        //    };

        //    if (_extensions?.Count > 0)
        //    {
        //        foreach (var extension in _extensions)
        //        {
        //            token.Extensions.Add(extension);
        //        }
        //    }

        //    return token;
        //}
    }
}
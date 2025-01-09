namespace ARWNI2S.Engine.Security.Token
{
    /// <summary>
    /// Defines settings used to create a token.
    /// </summary>
    public class TokenBuilder
    {
        private string _name;
        private List<string> _extensions;

        /// <summary>
        /// The name of the token.
        /// </summary>
        public virtual string Name
        {
            get => _name;
            set => _name = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException(ARWNI2S.Resources.ArgumentCannotBeNullOrEmpty, nameof(value));
        }

        /// <summary>
        /// The token path.
        /// </summary>
        /// <remarks>
        /// Determines the value that will be set for <see cref="TokenOptions.Path"/>.
        /// </remarks>
        public virtual string Path { get; set; }

        /// <summary>
        /// The domain to associate the token with.
        /// </summary>
        /// <remarks>
        /// Determines the value that will be set for <see cref="TokenOptions.Domain"/>.
        /// </remarks>
        public virtual string Domain { get; set; }

        /// <summary>
        /// Indicates whether a token is inaccessible by client-side script. The default value is <c>false</c>
        /// but specific components may use a different value.
        /// </summary>
        /// <remarks>
        /// Determines the value that will be set on <see cref="TokenOptions.HttpCookie"/>.
        /// </remarks>
        public virtual bool HttpCookie { get; set; }

        ///// <summary>
        ///// The SameSite attribute of the token. The default value is <see cref="SameSiteMode.Unspecified"/>
        ///// but specific components may use a different value.
        ///// </summary>
        ///// <remarks>
        ///// Determines the value that will be set for <see cref="TokenOptions.SameSite"/>.
        ///// </remarks>
        //public virtual SameSiteMode SameSite { get; set; } = SameSiteMode.Unspecified;

        /// <summary>
        /// The policy that will be used to determine <see cref="TokenOptions.Secure"/>.
        /// This is determined from the <see cref="INiisContext"/> passed to <see cref="Build(INiisContext, DateTimeOffset)"/>.
        /// </summary>
        public virtual TokenSecurePolicy SecurePolicy { get; set; }

        /// <summary>
        /// Gets or sets the lifespan of a token.
        /// </summary>
        public virtual TimeSpan? Expiration { get; set; }

        /// <summary>
        /// Gets or sets the max-age for the token.
        /// </summary>
        public virtual TimeSpan? MaxAge { get; set; }

        /// <summary>
        /// Indicates if this token is essential for the application to function correctly. If true then
        /// consent policy checks may be bypassed. The default value is <c>false</c>
        /// but specific components may use a different value.
        /// </summary>
        public virtual bool IsEssential { get; set; }

        /// <summary>
        /// Gets a collection of additional values to append to the token.
        /// </summary>
        public IList<string> Extensions
        {
            get => _extensions ??= [];
        }

        /// <summary>
        /// Creates the token options from the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The <see cref="INiisContext"/>.</param>
        /// <returns>The token options.</returns>
        public TokenOptions Build(INiisContext context) => Build(context, DateTimeOffset.Now);

        /// <summary>
        /// Creates the token options from the given <paramref name="context"/> with an expiration based on <paramref name="expiresFrom"/> and <see cref="Expiration"/>.
        /// </summary>
        /// <param name="context">The <see cref="INiisContext"/>.</param>
        /// <param name="expiresFrom">The time to use as the base for computing <see cref="TokenOptions.Expires" />.</param>
        /// <returns>The token options.</returns>
        public virtual TokenOptions Build(INiisContext context, DateTimeOffset expiresFrom)
        {
            ArgumentNullException.ThrowIfNull(context);

            var options = new TokenOptions
            {
                Path = Path ?? "/",
                //SameSite = SameSite,
                HttpCookie = HttpCookie,
                MaxAge = MaxAge,
                Domain = Domain,
                IsEssential = IsEssential,
                Secure = SecurePolicy == TokenSecurePolicy.Always || SecurePolicy == TokenSecurePolicy.SameAsContext && context.IsRequest,
                Expires = Expiration.HasValue ? expiresFrom.Add(Expiration.GetValueOrDefault()) : default(DateTimeOffset?)
            };

            if (_extensions?.Count > 0)
            {
                foreach (var extension in _extensions)
                {
                    options.Extensions.Add(extension);
                }
            }
            return options;
        }
    }
}
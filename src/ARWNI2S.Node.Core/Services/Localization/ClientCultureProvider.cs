using ARWNI2S.Infrastructure.Engine;

namespace ARWNI2S.Node.Core.Services.Localization
{
    /// <summary>
    /// An abstract base class provider for determining the culture information of an <see cref="ContextInfo"/>.
    /// </summary>
    public abstract class ClientCultureProvider : IClientCultureProvider
    {
        /// <summary>
        /// Result that indicates that this instance of <see cref="ClientCultureProvider" /> could not determine the
        /// request culture.
        /// </summary>
        protected static readonly Task<CultureProviderResult> NullProviderCultureResult = Task.FromResult(default(CultureProviderResult));

        ///// <summary>
        ///// The current options for the <see cref="RequestLocalizationMiddleware"/>.
        ///// </summary>
        //public RequestLocalizationOptions? Options { get; set; }

        /// <inheritdoc />
        public abstract Task<CultureProviderResult> DetermineProviderCultureResult(IEngineContext context);
    }
}

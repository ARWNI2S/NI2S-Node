using ARWNI2S.Node.Core.Network;
using ARWNI2S.Node.Core.Runtime;

namespace ARWNI2S.Node.Core.Services.Localization
{
    /// <summary>
    /// An abstract base class provider for determining the culture information of an <see cref="ConnectionInfo"/>.
    /// </summary>
    public abstract class RequestCultureProvider : IRequestCultureProvider
    {
        /// <summary>
        /// Result that indicates that this instance of <see cref="RequestCultureProvider" /> could not determine the
        /// request culture.
        /// </summary>
        protected static readonly Task<CultureProviderResult> NullProviderCultureResult = Task.FromResult(default(CultureProviderResult));

        ///// <summary>
        ///// The current options for the <see cref="RequestLocalizationMiddleware"/>.
        ///// </summary>
        //public RequestLocalizationOptions? Options { get; set; }

        /// <inheritdoc />
        public abstract Task<CultureProviderResult> DetermineProviderCultureResult(IRuntimeContext context);
    }
}

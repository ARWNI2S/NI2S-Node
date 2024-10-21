using ARWNI2S.Infrastructure.Engine;

namespace ARWNI2S.Node.Core.Services.Localization
{
    /// <summary>
    /// Details about the cultures obtained from <see cref="IRequestCultureProvider"/>.
    /// </summary>
    public class CultureProviderResult
    {
        /// <summary>
        /// Creates a new <see cref="CultureProviderResult"/> object that has its <see cref="Cultures"/> and
        /// <see cref="UICultures"/> properties set to the same culture value.
        /// </summary>
        /// <param name="culture">The name of the culture to be used for formatting, text, i.e. language.</param>
        public CultureProviderResult(string culture)
            : this([culture], [culture])
        {
        }

        /// <summary>
        /// Creates a new <see cref="CultureProviderResult"/> object has its <see cref="Cultures"/> and
        /// <see cref="UICultures"/> properties set to the respective culture values provided.
        /// </summary>
        /// <param name="culture">The name of the culture to be used for formatting.</param>
        /// <param name="uiCulture"> The name of the ui culture to be used for text, i.e. language.</param>
        public CultureProviderResult(string culture, string uiCulture)
            : this([culture], [uiCulture])
        {
        }

        /// <summary>
        /// Creates a new <see cref="CultureProviderResult"/> object that has its <see cref="Cultures"/> and
        /// <see cref="UICultures"/> properties set to the same culture value.
        /// </summary>
        /// <param name="cultures">The list of cultures to be used for formatting, text, i.e. language.</param>
        public CultureProviderResult(IList<string> cultures)
            : this(cultures, cultures)
        {
        }

        /// <summary>
        /// Creates a new <see cref="CultureProviderResult"/> object has its <see cref="Cultures"/> and
        /// <see cref="UICultures"/> properties set to the respective culture values provided.
        /// </summary>
        /// <param name="cultures">The list of cultures to be used for formatting.</param>
        /// <param name="uiCultures">The list of ui cultures to be used for text, i.e. language.</param>
        public CultureProviderResult(IList<string> cultures, IList<string> uiCultures)
        {
            Cultures = cultures;
            UICultures = uiCultures;
        }

        /// <summary>
        /// Gets the list of cultures to be used for formatting.
        /// </summary>
        public IList<string> Cultures { get; }

        /// <summary>
        /// Gets the list of ui cultures to be used for text, i.e. language;
        /// </summary>
        public IList<string> UICultures { get; }
    }

    /// <summary>
    /// Represents a provider for determining the culture information of an <see cref="ContextInfo"/>.
    /// </summary>
    public interface IRequestCultureProvider
    {
        /// <summary>
        /// Implements the provider to determine the culture of the given request.
        /// </summary>
        /// <param name="context">The <see cref="IEngineContext"/> for the request.</param>
        /// <returns>
        ///     The determined <see cref="CultureProviderResult"/>.
        ///     Returns <c>null</c> if the provider couldn't determine a <see cref="CultureProviderResult"/>.
        /// </returns>
        Task<CultureProviderResult> DetermineProviderCultureResult(IEngineContext context);
    }
}

using ARWNI2S.Node.Core.Entities.Localization;
using ARWNI2S.Node.Core.Runtime;
using ARWNI2S.Node.Core.Services.Localization;

namespace ARWNI2S.Runtime.Services.Localization
{
    /// <summary>
    /// Determines the culture information for a request via the value of a tag.
    /// </summary>
    public class ConnectionTagsCultureProvider : RequestCultureProvider
    {
        private const char _tagSeparator = '|';
        private const string _culturePrefix = "c=";
        private const string _uiCulturePrefix = "uic=";

        /// <summary>
        /// Represent the default tag name used to track the user's preferred culture information, which is ".AspNetCore.Culture".
        /// </summary>
        public static readonly string DefaultTagName = ".AspNetCore.Culture";

        /// <summary>
        /// The name of the tag that contains the user's preferred culture information.
        /// Defaults to <see cref="DefaultTagName"/>.
        /// </summary>
        public string TagName { get; set; } = DefaultTagName;

        /// <inheritdoc />
        public override Task<CultureProviderResult> DetermineProviderCultureResult(IRuntimeContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var tag = context.Connection.Tags[TagName];

            if (string.IsNullOrEmpty(tag))
            {
                return NullProviderCultureResult;
            }

            var providerResultCulture = ParseTagValue(tag);

            return Task.FromResult(providerResultCulture);
        }

        /// <summary>
        /// Creates a string representation of a <see cref="Language"/> for placement in a tag.
        /// </summary>
        /// <param name="language">The <see cref="Language"/>.</param>
        /// <returns>The tag value.</returns>
        public static string MakeTagValue(Language language)
        {
            ArgumentNullException.ThrowIfNull(language);

            return string.Join(_tagSeparator,
                $"{_culturePrefix}{language.LanguageCulture}",
                $"{_uiCulturePrefix}{language.LanguageCulture}");
        }

        /// <summary>
        /// Parses a <see cref="Language"/> from the specified tag value.
        /// Returns <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="value">The tag value to parse.</param>
        /// <returns>The <see cref="Language"/> or <c>null</c> if parsing fails.</returns>
        public static CultureProviderResult ParseTagValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            Span<Range> parts = stackalloc Range[3];
            var valueSpan = value.AsSpan();
            if (valueSpan.Split(parts, _tagSeparator, StringSplitOptions.RemoveEmptyEntries) != 2)
            {
                return null;
            }

            var potentialCultureName = valueSpan[parts[0]];
            var potentialUICultureName = valueSpan[parts[1]];

            if (!potentialCultureName.StartsWith(_culturePrefix, StringComparison.Ordinal) || !
                potentialUICultureName.StartsWith(_uiCulturePrefix, StringComparison.Ordinal))
            {
                return null;
            }

            var cultureName = potentialCultureName.Slice(_culturePrefix.Length);
            var uiCultureName = potentialUICultureName.Slice(_uiCulturePrefix.Length);

            if (cultureName.IsEmpty && uiCultureName.IsEmpty)
            {
                // No values specified for either so no match
                return null;
            }

            if (!cultureName.IsEmpty && uiCultureName.IsEmpty)
            {
                // Value for culture but not for UI culture so default to culture value for both
                uiCultureName = cultureName;
            }
            else if (cultureName.IsEmpty && !uiCultureName.IsEmpty)
            {
                // Value for UI culture but not for culture so default to UI culture value for both
                cultureName = uiCultureName;
            }

            return new CultureProviderResult(cultureName.ToString(), uiCultureName.ToString());
        }
    }
}

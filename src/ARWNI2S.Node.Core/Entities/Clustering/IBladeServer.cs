using ARWNI2S.Node.Core.Entities.Localization;

namespace ARWNI2S.Node.Core.Entities.Clustering
{
    /// <summary>
    /// Represents a server
    /// </summary>
    public partial interface IBladeServer : INodeEntity, ILocalizedEntity, ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets the server name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        string DefaultMetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        string DefaultMetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        string DefaultTitle { get; set; }

        /// <summary>
        /// Home page title
        /// </summary>
        string HomepageTitle { get; set; }

        /// <summary>
        /// Home page description
        /// </summary>
        string HomepageDescription { get; set; }

        /// <summary>
        /// Gets or sets the server URL
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled
        /// </summary>
        bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the comma separated list of possible HTTP_HOST values
        /// </summary>
        string Hosts { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default language for this server; 0 is set when we use the default language display order
        /// </summary>
        int DefaultLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company address
        /// </summary>
        string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the server phone number
        /// </summary>
        string CompanyPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the company VAT (used in Europe Union countries)
        /// </summary>
        string CompanyVat { get; set; }
    }
}

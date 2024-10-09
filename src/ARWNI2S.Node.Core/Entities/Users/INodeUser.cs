namespace ARWNI2S.Node.Core.Entities.Users
{
    public interface INodeUser : INodeEntity, ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets the user GUID
        /// </summary>
        Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        string Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        string Company { get; set; }

        /// <summary>
        /// Gets or sets the street address
        /// </summary>
        string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the street address 2
        /// </summary>
        string StreetAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the zip
        /// </summary>
        string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Gets or sets the county
        /// </summary>
        string County { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state province id
        /// </summary>
        int StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// Gets or sets the fax
        /// </summary>
        string Fax { get; set; }

        /// <summary>
        /// Gets or sets the vat number
        /// </summary>
        string VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the vat number status id
        /// </summary>
        int VatNumberStatusId { get; set; }

        /// <summary>
        /// Gets or sets the time zone id
        /// </summary>
        string TimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets the custom attributes
        /// </summary>
        string CustomUserAttributesXML { get; set; }

        /// <summary>
        /// Gets or sets the currency id
        /// </summary>
        int? CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the token id
        /// </summary>
        int? TokenId { get; set; }

        /// <summary>
        /// Gets or sets the language id
        /// </summary>
        int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the tax display type id
        /// </summary>
        int? TaxDisplayTypeId { get; set; }

        /// <summary>
        /// Gets or sets the email that should be re-validated. Used in scenarios when a user is already registered and wants to change an email address.
        /// </summary>
        string EmailToRevalidate { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is tax exempt
        /// </summary>
        bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        int AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the partner identifier with which this user is associated (maganer)
        /// </summary>
        int PartnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is required to re-login
        /// </summary>
        bool RequireReLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a user cannot login (locked out)
        /// </summary>
        DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is system
        /// </summary>
        bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the user system name
        /// </summary>
        string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        ///  Gets or sets the node identifier in which user registered
        /// </summary>
        int RegisteredInServerId { get; set; }
    }
}

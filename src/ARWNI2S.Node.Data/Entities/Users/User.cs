﻿using ARWNI2S.Node.Core;

namespace ARWNI2S.Node.Data.Entities.Users
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public partial class User : DataEntity, ISoftDeletedEntity
    {
        public User()
        {
            UserGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the user GUID
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the street address 2
        /// </summary>
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the zip
        /// </summary>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the county
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state province id
        /// </summary>
        public int StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the fax
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Gets or sets the vat number
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the vat number status id
        /// </summary>
        public int VatNumberStatusId { get; set; }

        /// <summary>
        /// Gets or sets the time zone id
        /// </summary>
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets the custom attributes
        /// </summary>
        public string CustomUserAttributesXML { get; set; }

        /// <summary>
        /// Gets or sets the currency id
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the token id
        /// </summary>
        public int? TokenId { get; set; }

        /// <summary>
        /// Gets or sets the language id
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the tax display type id
        /// </summary>
        public int? TaxDisplayTypeId { get; set; }

        /// <summary>
        /// Gets or sets the email that should be re-validated. Used in scenarios when a user is already registered and wants to change an email address.
        /// </summary>
        public string EmailToRevalidate { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is tax exempt
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the partner identifier with which this user is associated (maganer)
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is required to re-login
        /// </summary>
        public bool RequireReLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a user cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the user system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        ///  Gets or sets the node identifier in which user registered
        /// </summary>
        public int RegisteredInNodeId { get; set; }

        /// <summary>
        /// Gets or sets the billing address identifier
        /// </summary>
        public int? BillingAddressId { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// </summary>
        public int? ShippingAddressId { get; set; }

    }
}